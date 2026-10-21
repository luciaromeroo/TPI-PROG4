using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services;

// Todo lo de reservas pasa por acá: valida que el cliente exista, que las habitaciones
// existan, estén habilitadas, libres y con capacidad, y el total lo calcula el sistema.
// La reserva siempre nace pendiente y se crea junto con sus habitaciones.
// Permisos: un cliente solo ve y cancela sus propias reservas; el staff, todas.
public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRepository<Room> _roomRepository;

    public ReservationService(
        IReservationRepository reservationRepository,
        IUserRepository userRepository,
        IRepository<Room> roomRepository)
    {
        _reservationRepository = reservationRepository;
        _userRepository = userRepository;
        _roomRepository = roomRepository;
    }

    // Trae todas las reservas.
    public List<ReservationDto> GetAllReservations()
    {
        var reservations = _reservationRepository.List();
        return ReservationDto.CreateList(reservations);
    }

    // Trae una reserva por su id.
    public ReservationDto GetReservationById(int id, CurrentUser currentUser)
    {
        var reservation = _reservationRepository.GetById(id)
            ?? throw new NotFoundException("RESERVATION_NOT_FOUND");

        currentUser.EnsureCanAccess(reservation.UserId);

        return ReservationDto.Create(reservation);
    }

    // Trae las reservas de un cliente.
    public List<ReservationDto> GetReservationsByUser(int userId, CurrentUser currentUser)
    {
        currentUser.EnsureCanAccess(userId);
        GetExistingUser(userId);

        var reservations = _reservationRepository.GetByUserId(userId);
        return ReservationDto.CreateList(reservations);
    }

    // Crea la reserva y sus habitaciones en una sola operación, a nombre del usuario logueado.
    // El código lo genera el servidor, arranca pendiente y el total lo calcula el sistema
    // (nunca lo manda el cliente).
    public ReservationDto CreateReservation(CreateReservationRequest request, CurrentUser currentUser)
    {
        ValidateReservationDates(request.StartDate, request.EndDate);

        GetExistingUser(currentUser.Id);

        if (request.Rooms is null || request.Rooms.Count == 0)
            throw new AppValidationException("RESERVATION_MUST_HAVE_ROOMS");

        // La misma habitación dos veces en el mismo carrito no tiene sentido.
        if (request.Rooms.GroupBy(room => room.RoomId).Any(group => group.Count() > 1))
            throw new AppValidationException("DUPLICATED_ROOM_IN_RESERVATION");

        var reservation = new Reservation(
            "RSV-" + Guid.NewGuid().ToString("N")[..6].ToUpper(),
            request.StartDate,
            request.EndDate,
            currentUser.Id
        );

        foreach (var roomRequest in request.Rooms)
        {
            var room = _roomRepository.GetById(roomRequest.RoomId)
                ?? throw new NotFoundException("ROOM_NOT_FOUND");

            if (!room.State)
                throw new AppValidationException("ROOM_OUT_OF_SERVICE");

            if (roomRequest.GuestsNumber < 1 || roomRequest.GuestsNumber > room.Capacity)
                throw new AppValidationException("GUESTS_EXCEED_ROOM_CAPACITY");

            if (!_reservationRepository.IsRoomAvailable(room.Id, request.StartDate, request.EndDate))
                throw new AppValidationException("ROOM_NOT_AVAILABLE_FOR_DATES");

            reservation.DetailReservations.Add(new DetailReservation
            {
                GuestsNumber = roomRequest.GuestsNumber,
                Room = room
            });
        }

        reservation.CalculateTotalAmount();

        // Un solo Add: la reserva le pasa sus detalles por la navegación y EF los inserta juntos.
        _reservationRepository.Add(reservation);

        return ReservationDto.Create(reservation);
    }

    // Cambia las fechas de una reserva pendiente. Al cambiarlas, las habitaciones tienen que
    // seguir libres (sin contar esta misma reserva) y el nuevo total no puede quedar por
    // debajo de lo que ya se registró en pagos. El nuevo total se recalcula solo.
    public ReservationDto UpdateReservation(int id, UpdateReservationRequest request)
    {
        var reservation = _reservationRepository.GetById(id)
            ?? throw new NotFoundException("RESERVATION_NOT_FOUND");

        // Confirmada o cancelada no se edita: quedaría pisar el historial.
        if (reservation.Type != ReservationState.Pending)
            throw new AppValidationException("RESERVATION_NOT_EDITABLE");

        var startDate = request.StartDate ?? reservation.StartDate;
        var endDate = request.EndDate ?? reservation.EndDate;

        if (startDate == reservation.StartDate && endDate == reservation.EndDate)
            return ReservationDto.Create(reservation);

        ValidateReservationDates(startDate, endDate);

        foreach (var detail in reservation.DetailReservations)
        {
            if (!_reservationRepository.IsRoomAvailable(detail.RoomId, startDate, endDate, reservation.Id))
                throw new AppValidationException("ROOM_NOT_AVAILABLE_FOR_DATES");
        }

        reservation.StartDate = startDate;
        reservation.EndDate = endDate;
        reservation.CalculateTotalAmount();

        // Si ya se registró algo de plata, el total nuevo no puede quedar por debajo de eso.
        // Suma en memoria: SQLite no puede sumar columnas decimal.
        if (reservation.TotalAmount < reservation.Payments.Sum(payment => payment.Amount))
            throw new AppValidationException("RESERVATION_TOTAL_BELOW_REGISTERED_PAYMENTS");

        _reservationRepository.Update(reservation);

        return ReservationDto.Create(reservation);
    }

    // Pasa la reserva a confirmada.
    public ReservationDto ConfirmReservation(int id)
    {
        var reservation = _reservationRepository.GetById(id)
            ?? throw new NotFoundException("RESERVATION_NOT_FOUND");

        reservation.ConfirmReservation();
        _reservationRepository.Update(reservation);

        return ReservationDto.Create(reservation);
    }

    // Pasa la reserva a cancelada.
    public ReservationDto CancelReservation(int id, CurrentUser currentUser)
    {
        var reservation = _reservationRepository.GetById(id)
            ?? throw new NotFoundException("RESERVATION_NOT_FOUND");

        currentUser.EnsureCanAccess(reservation.UserId);

        reservation.CancelReservation();
        _reservationRepository.Update(reservation);

        return ReservationDto.Create(reservation);
    }

    // Borra la reserva, pero solo si no tiene ningún pago registrado: con pagos el
    // historial contable queda y la salida es cancelar la reserva, no borrarla.
    public void DeleteReservation(int id)
    {
        var reservation = _reservationRepository.GetById(id)
            ?? throw new NotFoundException("RESERVATION_NOT_FOUND");

        if (reservation.Payments.Any())
            throw new AppValidationException("RESERVATION_HAS_PAYMENTS");

        _reservationRepository.Delete(reservation);
    }

    // Fechas del documento: la estadía empieza hoy o más tarde y termina después de empezar.
    // Si el cliente no manda la fecha de inicio, llega DateTime.MinValue y este check lo tira afuera.
    private static void ValidateReservationDates(DateTime startDate, DateTime endDate)
    {
        if (startDate < DateTime.Today)
            throw new AppValidationException("START_DATE_IN_THE_PAST");

        if (startDate >= endDate)
            throw new AppValidationException("INVALID_RESERVATION_DATES");
    }

    private User GetExistingUser(int userId)
    {
        return _userRepository.GetById(userId)
            ?? throw new NotFoundException("USER_NOT_FOUND");
    }
}

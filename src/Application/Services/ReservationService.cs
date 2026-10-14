using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services;

// Todo lo de reservas pasa por acá: valida que el cliente exista, que las fechas
// tengan sentido y genera el código. La reserva siempre nace pendiente.
public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IRepository<User> _userRepository;

    public ReservationService(
        IReservationRepository reservationRepository,
        IRepository<User> userRepository)
    {
        _reservationRepository = reservationRepository;
        _userRepository = userRepository;
    }

    // Trae todas las reservas.
    public List<ReservationDto> GetAllReservations()
    {
        var reservations = _reservationRepository.List();
        return ReservationDto.CreateList(reservations);
    }

    // Trae una reserva por su id.
    public ReservationDto GetReservationById(int id)
    {
        var reservation = _reservationRepository.GetById(id)
            ?? throw new NotFoundException("RESERVATION_NOT_FOUND");

        return ReservationDto.Create(reservation);
    }

    // Trae las reservas de un cliente.
    public List<ReservationDto> GetReservationsByUser(int userId)
    {
        GetExistingUser(userId);

        var reservations = _reservationRepository.GetByUserId(userId);
        return ReservationDto.CreateList(reservations);
    }

    // Crea la reserva: el código lo genera el servidor y arranca pendiente.
    public ReservationDto CreateReservation(CreateReservationRequest request)
    {
        GetExistingUser(request.UserId);

        string reservationCode = "RSV-" + Guid.NewGuid().ToString("N")[..6].ToUpper();

        var reservation = new Reservation(
            reservationCode,
            request.StartDate,
            request.EndDate,
            request.TotalAmount,
            request.UserId
        );

        _reservationRepository.Add(reservation);

        return ReservationDto.Create(reservation);
    }

    // Cambia fechas y/o monto. Si no mandan un campo, queda como estaba.
    public ReservationDto UpdateReservation(int id, UpdateReservationRequest request)
    {
        var reservation = _reservationRepository.GetById(id)
            ?? throw new NotFoundException("RESERVATION_NOT_FOUND");

        var startDate = request.StartDate ?? reservation.StartDate;
        var endDate = request.EndDate ?? reservation.EndDate;

        if (startDate >= endDate)
            throw new AppValidationException("INVALID_RESERVATION_DATES");

        reservation.StartDate = startDate;
        reservation.EndDate = endDate;

        if (request.TotalAmount.HasValue)
        {
            if (request.TotalAmount.Value < 0)
                throw new AppValidationException("RESERVATION_AMOUNT_CANNOT_BE_NEGATIVE");

            reservation.TotalAmount = request.TotalAmount.Value;
        }

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
    public ReservationDto CancelReservation(int id)
    {
        var reservation = _reservationRepository.GetById(id)
            ?? throw new NotFoundException("RESERVATION_NOT_FOUND");

        reservation.CancelReservation();
        _reservationRepository.Update(reservation);

        return ReservationDto.Create(reservation);
    }

    // Borra la reserva.
    public void DeleteReservation(int id)
    {
        var reservation = _reservationRepository.GetById(id)
            ?? throw new NotFoundException("RESERVATION_NOT_FOUND");

        _reservationRepository.Delete(reservation);
    }

    private User GetExistingUser(int userId)
    {
        return _userRepository.GetById(userId)
            ?? throw new NotFoundException("USER_NOT_FOUND");
    }
}

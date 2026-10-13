using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services;

// Reglas de negocio de pagos (docx + definición del equipo):
// - Todo pago pertenece a una reserva existente (composición Reservation 1..* Payment).
// - Una reserva admite varios pagos parciales (seña, transferencia, saldo en el hotel...), cada uno con su método.
// - La suma de los pagos registrados nunca puede superar el TotalAmount de la reserva.
// - No se registran ni se confirman pagos de una reserva cancelada.
// - El pago nace con PaymentState = false y pasa a true solo al confirmarse (acreditarse).
// - Un pago confirmado no se puede eliminar.
public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IRepository<Reservation> _reservationRepository;

    public PaymentService(
        IPaymentRepository paymentRepository,
        IRepository<Reservation> reservationRepository)
    {
        _paymentRepository = paymentRepository;
        _reservationRepository = reservationRepository;
    }

    // Obtiene todos los pagos registrados.
    public List<PaymentDto> GetAllPayments()
    {
        var payments = _paymentRepository.List();
        return PaymentDto.CreateList(payments);
    }

    // Obtiene un pago por su identificador.
    public PaymentDto GetPaymentById(int id)
    {
        var payment = _paymentRepository.GetById(id)
            ?? throw new NotFoundException("PAYMENT_NOT_FOUND");

        return PaymentDto.Create(payment);
    }

    // Obtiene los pagos de una reserva.
    public List<PaymentDto> GetPaymentsByReservation(int reservationId)
    {
        GetExistingReservation(reservationId);

        var payments = _paymentRepository.GetByReservation(reservationId);
        return PaymentDto.CreateList(payments);
    }

    // Estado de cuenta de la reserva: acreditado, pendiente de acreditar y lo que falta registrar.
    // Las sumas se hacen en memoria porque SQLite no puede sumar columnas decimal.
    public ReservationBalanceDto GetReservationBalance(int reservationId)
    {
        var reservation = GetExistingReservation(reservationId);
        var payments = _paymentRepository.GetByReservation(reservationId).ToList();

        decimal confirmed = payments.Where(p => p.PaymentState).Sum(p => p.Amount);
        decimal pending = payments.Where(p => !p.PaymentState).Sum(p => p.Amount);
        decimal remaining = reservation.TotalAmount - confirmed - pending;

        return new ReservationBalanceDto(
            reservation.Id,
            reservation.TotalAmount,
            confirmed,
            pending,
            remaining,
            confirmed >= reservation.TotalAmount,
            PaymentDto.CreateList(payments)
        );
    }

    // Registra un pago parcial (seña, saldo, pago en destino...) para una reserva.
    public PaymentDto RegisterPayment(CreatePaymentRequest request)
    {
        var reservation = GetExistingReservation(request.ReservationId);

        if (reservation.Type == ReservationState.Canceled)
            throw new AppValidationException("CANNOT_PAY_CANCELED_RESERVATION");

        // Los pagos pendientes también cuentan: representan dinero ya comprometido
        // (por ejemplo, el saldo que se abonará al llegar al hotel).
        decimal alreadyRegistered = _paymentRepository
            .GetByReservation(reservation.Id)
            .Sum(p => p.Amount);

        if (alreadyRegistered + request.Amount > reservation.TotalAmount)
            throw new AppValidationException("PAYMENT_EXCEEDS_RESERVATION_BALANCE");

        int transactionNumber = _paymentRepository.GetLastTransactionNumber() + 1;

        // El constructor valida monto > 0 y método obligatorio, y deja el pago sin acreditar.
        var payment = new Payment(
            transactionNumber,
            request.Amount,
            request.PaymentMethod,
            reservation.Id
        );

        _paymentRepository.Add(payment);

        return PaymentDto.Create(payment);
    }

    // Acredita un pago (por ejemplo, cuando el huésped paga el saldo en el hotel).
    public PaymentDto ConfirmPayment(int id)
    {
        var payment = _paymentRepository.GetById(id)
            ?? throw new NotFoundException("PAYMENT_NOT_FOUND");

        var reservation = GetExistingReservation(payment.ReservationId);

        if (reservation.Type == ReservationState.Canceled)
            throw new AppValidationException("CANNOT_CONFIRM_PAYMENT_OF_CANCELED_RESERVATION");

        payment.ConfirmPayment();
        _paymentRepository.Update(payment);

        // PENDIENTE DE DEFINIR CON EL EQUIPO: si la seña o el pago total deben pasar
        // la reserva a Confirmed, este es el lugar para llamar a reservation.ConfirmReservation().

        return PaymentDto.Create(payment);
    }

    // Elimina un pago registrado por error. Solo se permite si todavía no fue acreditado.
    public void DeletePayment(int id)
    {
        var payment = _paymentRepository.GetById(id)
            ?? throw new NotFoundException("PAYMENT_NOT_FOUND");

        payment.EnsureCanBeDeleted();
        _paymentRepository.Delete(payment);
    }

    private Reservation GetExistingReservation(int reservationId)
    {
        return _reservationRepository.GetById(reservationId)
            ?? throw new NotFoundException("RESERVATION_NOT_FOUND");
    }
}

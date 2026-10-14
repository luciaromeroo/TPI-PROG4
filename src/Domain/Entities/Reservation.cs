using Domain.Exceptions;

namespace Domain;

// Una reserva es la estadía de un cliente: fechas, monto total y código.
// Arranca pendiente y después se confirma o se cancela.
public class Reservation
{
    public int Id { get; set; }
    public string ReservationCode { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ReservationState Type { get; set; }
    public decimal TotalAmount { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public List<DetailReservation> DetailReservations { get; set; } = new();  // 1..*
    public List<Payment> Payments { get; set; } = new();                       // 1..*

    // Lo pide Entity Framework, sin esto no anda.
    public Reservation() { }

    // El id lo pone la base, el código lo genera el servicio y la fecha la pone el servidor.
    public Reservation(string reservationCode, DateTime startDate, DateTime endDate, decimal totalAmount, int userId)
    {
        if (string.IsNullOrWhiteSpace(reservationCode))
            throw new AppValidationException("RESERVATION_CODE_REQUIRED");

        if (startDate >= endDate)
            throw new AppValidationException("INVALID_RESERVATION_DATES");

        if (totalAmount < 0)
            throw new AppValidationException("RESERVATION_AMOUNT_CANNOT_BE_NEGATIVE");

        ReservationCode = reservationCode.Trim();
        StartDate = startDate;
        EndDate = endDate;
        TotalAmount = totalAmount;
        UserId = userId;
        CreationDate = DateTime.Now;
        Type = ReservationState.Pending;
    }

    // Pasa la reserva a confirmada (cuando se acredita la seña, por ejemplo).
    public void ConfirmReservation()
    {
        if (Type == ReservationState.Confirmed)
            throw new AppValidationException("RESERVATION_ALREADY_CONFIRMED");

        if (Type == ReservationState.Canceled)
            throw new AppValidationException("CANNOT_CONFIRM_CANCELED_RESERVATION");

        Type = ReservationState.Confirmed;
    }

    // Pasa la reserva a cancelada, después ya no se puede confirmar ni cobrar.
    public void CancelReservation()
    {
        if (Type == ReservationState.Canceled)
            throw new AppValidationException("RESERVATION_ALREADY_CANCELED");

        Type = ReservationState.Canceled;
    }
}

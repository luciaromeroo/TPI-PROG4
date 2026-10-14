using Domain.Exceptions;

namespace Domain;

// Una Reservation es la cabecera de la estadía: rango de fechas, monto total
// y cliente. Nace en Pending y pasa a Confirmed o Canceled.
// Los detalles (DetailReservations) y los pagos (Payments) cuelgan de ella.
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

    // Constructor vacío requerido por Entity Framework.
    public Reservation() { }

    // Constructor de negocio: el Id lo genera la base, la reserva siempre nace
    // en Pending con la fecha de creación que pone el servidor.
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

    // Confirma la reserva (por ejemplo, al acreditarse la seña).
    public void ConfirmReservation()
    {
        if (Type == ReservationState.Confirmed)
            throw new AppValidationException("RESERVATION_ALREADY_CONFIRMED");

        if (Type == ReservationState.Canceled)
            throw new AppValidationException("CANNOT_CONFIRM_CANCELED_RESERVATION");

        Type = ReservationState.Confirmed;
    }

    // Cancela la reserva. Una reserva cancelada no acepta pagos ni confirmaciones.
    public void CancelReservation()
    {
        if (Type == ReservationState.Canceled)
            throw new AppValidationException("RESERVATION_ALREADY_CANCELED");

        Type = ReservationState.Canceled;
    }
}

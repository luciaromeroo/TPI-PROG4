namespace Domain;

public class Payment
{
    public int Id { get; set; }
    public int TransactionNumber { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public bool PaymentState { get; set; }
    public DateTime PaymentDate { get; set; }

    public int ReservationId { get; set; }
    public Reservation Reservation { get; set; } = null!;
}
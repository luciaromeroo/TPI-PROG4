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

    public Payment(int id, int transactionNumber, decimal amount, string paymentMethod, bool paymentState, DateTime paymentDate, int reservationId)
    {
        Id = id;
        TransactionNumber = transactionNumber;
        Amount = amount;
        PaymentMethod = paymentMethod;
        PaymentState = paymentState;
        PaymentDate = paymentDate;
        ReservationId = reservationId;
    }

    public void ConfirmPayment()
    {
        PaymentState = true;
    }
}
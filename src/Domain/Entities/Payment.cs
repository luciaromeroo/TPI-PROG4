namespace Domain;

public class Payment
{
    public int TransactionNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public bool PaymentState { get; set; }
    public DateTime PaymentDate { get; set; }
}
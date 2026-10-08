namespace Domain;

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
}

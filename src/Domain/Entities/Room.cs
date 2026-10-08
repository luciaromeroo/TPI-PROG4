namespace Domain;

public class Room
{
    public int Id { get; set; }
    public int Number { get; set; }
    public RoomType Type { get; set; }
    public decimal PricePerNight { get; set; }
    public bool State { get; set; }
    public int Capacity { get; set; }

    public List<DetailReservation> DetailReservations { get; set; } = new();   // 0..*
}
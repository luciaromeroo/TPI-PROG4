namespace Domain;


public class DetailReservation
{
    public int Id { get; set; }
    public int GuestsNumber { get; set; }

    public int ReservationId { get; set; }
    public Reservation Reservation { get; set; } = null!;
    public int RoomId { get; set; }
    public Room Room { get; set; } = null!;
}
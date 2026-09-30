namespace Domain;

public class Reservation
{
    public int ReservationCode { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreationDate { get; set; }
    public ReservationState Type { get; set; }

   
    // private List<ReservaDetalle> _reservaDetalles { get; set; } = new();

    // public IReadOnlyList<ReservaDetalle> ReservaDetalles => _reservaDetalles.AsReadOnly();

}
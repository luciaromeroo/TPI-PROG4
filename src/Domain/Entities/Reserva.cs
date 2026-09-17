namespace Domain;

public class Reserva
{
    public int CodigoReserva { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public DateTime FechaCreacion { get; set; }
    public decimal MontoTotal { get; set; }
    public bool EstadoReserva { get; set; }

   
    // private List<ReservaDetalle> _reservaDetalles { get; set; } = new();

    // public IReadOnlyList<ReservaDetalle> ReservaDetalles => _reservaDetalles.AsReadOnly();

}
namespace Domain;

public class Reserva
{
    public int CodigoReserva { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public DateTime FechaCreacion { get; set; }
    public decimal MontoTotal { get; set; }
    public bool EstadoReserva { get; set; }

    // // Relación: 1 Cliente (asociado)
    // public Cliente Cliente { get; set; } = null!;

    // // Relación agregación: 0..* Habitaciones asignadas
    // public List<Habitacion> Habitaciones { get; set; } = new();

    // // Relación composición: 0..1 Pago registrado
    // public Pago? Pago { get; set; }
}
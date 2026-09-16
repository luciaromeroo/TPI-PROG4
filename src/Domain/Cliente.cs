namespace Domain;

public class Cliente
{
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Dni { get; set; } = string.Empty;
    public string Mail { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;

    
    
    
    // Relación: Un cliente puede tener múltiples reservas
    // public List<Reserva> Reservas { get; set; } = new();
}
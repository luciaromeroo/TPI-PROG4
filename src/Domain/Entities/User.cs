namespace Domain;

public class User
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Dni { get; set; } = string.Empty;
    public string Mail { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;

    public User(int id, string nombre, string apellido, string dni, string mail, string telefono)
    {
        Id = id;
        Nombre = nombre;
        Apellido = apellido;
        Dni = dni;
        Mail = mail;
        Telefono = telefono;
    }

    
    
    
    // Relación: Un cliente puede tener múltiples reservas
    // public List<Reserva> Reservas { get; set; } = new();
}
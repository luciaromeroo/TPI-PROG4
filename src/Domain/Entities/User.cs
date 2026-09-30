namespace Domain;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Mail { get; set; } = string.Empty;    
    public string Password { get; set;} = string.Empty;
    public string Dni { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public Role Type { get; set;}

    public User(int id, string name, string lastname, string mail, string password, string dni, string phone, Role role)
    {
        Id = id;
        Name = name;
        LastName = lastname;
        Mail = mail;
        Password = password;
        Dni = dni;
        Phone = phone;
        Type = role;
    }

    
    
    
    // Relación: Un cliente puede tener múltiples reservas
    // public List<Reserva> Reservas { get; set; } = new();
}
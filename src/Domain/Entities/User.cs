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

    public User(int id, string name, string lastName, string mail, string password, string dni, string phone, Role type)
    {
        Id = id;
        Name = name;
        LastName = lastName;
        Mail = mail;
        Password = password;
        Dni = dni;
        Phone = phone;
        Type = type;
    }

    
    
    
    // Relación: Un cliente puede tener múltiples reservas
       public List<Reservation> Reservations { get; set; } = new();
}
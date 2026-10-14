using Domain.Exceptions;

namespace Domain;

// Un usuario del sistema: cliente, administrador o superadmin (Role).
public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Mail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Dni { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public Role Type { get; set; }

    // Relación: un cliente puede tener múltiples reservas.
    public List<Reservation> Reservations { get; set; } = new();

    // Constructor vacío requerido por Entity Framework.
    public User() { }

    // Constructor de negocio: el Id lo genera la base, por eso no se recibe.
    // No se puede crear un usuario con datos vacíos o con un rol que no existe.
    public User(string name, string lastName, string mail, string password, string dni, string phone, Role type)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new AppValidationException("USER_NAME_REQUIRED");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new AppValidationException("USER_LASTNAME_REQUIRED");

        if (string.IsNullOrWhiteSpace(mail))
            throw new AppValidationException("USER_MAIL_REQUIRED");

        if (string.IsNullOrWhiteSpace(password))
            throw new AppValidationException("USER_PASSWORD_REQUIRED");

        if (string.IsNullOrWhiteSpace(dni))
            throw new AppValidationException("USER_DNI_REQUIRED");

        if (string.IsNullOrWhiteSpace(phone))
            throw new AppValidationException("USER_PHONE_REQUIRED");

        if (!Enum.IsDefined(type))
            throw new AppValidationException("INVALID_USER_ROLE");

        Name = name.Trim();
        LastName = lastName.Trim();
        Mail = mail.Trim().ToLowerInvariant();   // en minúscula para que "Ana@Mail.com" y "ana@mail.com" sean el mismo
        Password = password;
        Dni = dni.Trim();
        Phone = phone.Trim();
        Type = type;
    }
}

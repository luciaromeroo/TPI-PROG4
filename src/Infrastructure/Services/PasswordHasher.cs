using Application.Interfaces;

namespace Infrastructure.Services;

// Hashea contraseñas con BCrypt (paquete BCrypt.Net-Next), igual que el proyecto Turnando de la cátedra.
// BCrypt es lento a propósito (dificulta probar millones de contraseñas) y guarda la "sal"
// aleatoria adentro del mismo hash, así que no hay que manejarla a mano.
public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);

    public bool Verify(string password, string passwordHash)
    {
        // Si lo guardado no es un hash BCrypt (por ejemplo, una contraseña vieja en texto plano),
        // BCrypt lanza una excepción: en ese caso el login simplemente falla.
        try
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
        catch (BCrypt.Net.SaltParseException)
        {
            return false;
        }
    }
}

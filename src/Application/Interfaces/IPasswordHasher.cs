namespace Application.Interfaces;

// Contrato para guardar contraseñas de forma segura (nunca en texto plano).
// La implementación vive en Infrastructure (PasswordHasher).
public interface IPasswordHasher
{
    // Devuelve el hash que se guarda en la base.
    string Hash(string password);

    // Compara la contraseña que escribió el usuario con el hash guardado.
    bool Verify(string password, string passwordHash);
}

using Domain;

namespace Application.Interfaces;

// Contrato para generar el token de sesión (JWT) de un usuario.
// La implementación vive en Infrastructure (JwtTokenService).
public interface ITokenService
{
    string GenerateToken(User user);
}

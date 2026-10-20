namespace Application.Models;

// Lo que devuelve el login: el token para mandar en cada request
// (header Authorization: Bearer <token>) y los datos del usuario para el frontend.
public record LoginResponse(
    string Token,
    UserDto User
);

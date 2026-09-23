namespace Application.Models;

public record PostUserRequest(
    int Id,
    string Nombre,
    string Apellido,
    string Dni,
    string Mail,
    string Telefono);
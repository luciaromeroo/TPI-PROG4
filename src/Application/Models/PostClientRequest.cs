namespace Application.Models;

public record PostClientRequest(
    int Id,
    string Nombre,
    string Apellido,
    string Dni,
    string Mail,
    string Telefono);
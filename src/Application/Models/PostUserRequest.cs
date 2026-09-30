using Domain;

namespace Application.Models;

public record PostUserRequest(
    int Id,
    string Name,
    string LastName,
    string Mail,
    string Password,
    string Dni,
    string Phone,
    Role Role
);

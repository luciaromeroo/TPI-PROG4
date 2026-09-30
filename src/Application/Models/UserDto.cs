using Domain;

namespace Application.Models;

public record UserDto(
    int Id,
    string Name,
    string LastName,
    string Mail,
    string Dni,
    string Phone,
    Role Type
)
{
    public static UserDto Create(User entity)
    {
        return new UserDto(
            entity.Id,
            entity.Name,
            entity.LastName,
            entity.Mail,
            entity.Dni,
            entity.Phone,
            entity.Type
        );
    }

    public static List<UserDto> Create(IEnumerable<User> entities)
    {
        return entities.Select(Create).ToList();
    }
}
using Domain;

namespace Application.Models;


public record UserDto(int Id, string Nombre, string Apellido, string Dni, string Mail, string Telefono)
{
    
    public static UserDto Create(User entity)
    {
        var dto = new UserDto(
                    entity.Id,
                    entity.Nombre,
                    entity.Apellido,
                    entity.Dni,
                    entity.Mail,
                    entity.Telefono
                );

        return dto;
    }

    public static List<UserDto> Create(IEnumerable<User> entities)
    {
        var listDto = new List<UserDto>();
        foreach (var entity in entities)
        {
            listDto.Add(Create(entity));
        }
        return listDto;
    }

   
}
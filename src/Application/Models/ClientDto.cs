using Domain;

namespace Application.Models;


public record ClientDto(int Id, string Nombre, string Apellido, string Dni, string Mail, string Telefono)
{
    
    public static ClientDto Create(Cliente entity)
    {
        var dto = new ClientDto(
                    entity.Id,
                    entity.Nombre,
                    entity.Apellido,
                    entity.Dni,
                    entity.Mail,
                    entity.Telefono
                );

        return dto;
    }

    public static List<ClientDto> Create(IEnumerable<Cliente> entities)
    {
        var listDto = new List<ClientDto>();
        foreach (var entity in entities)
        {
            listDto.Add(Create(entity));
        }
        return listDto;
    }

   
}
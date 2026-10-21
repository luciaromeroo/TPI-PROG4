using Domain;

namespace Application.Models;

public record RoomDto(
    int Id,
    int Number,
    RoomType Type,
    decimal PricePerNight,
    bool State,
    int Capacity
)
{
    public static RoomDto Create(Room entity)
    {
        return new RoomDto(
            entity.Id,
            entity.Number,
            entity.Type,
            entity.PricePerNight,
            entity.State,
            entity.Capacity
        );
    }

    public static List<RoomDto> CreateList(IEnumerable<Room> entities)
    {
        return entities.Select(Create).ToList();
    }
}
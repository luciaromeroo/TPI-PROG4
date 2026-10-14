using Domain;

namespace Application.Models;

public record ReservationDto(
    int Id,
    string ReservationCode,
    DateTime CreationDate,
    DateTime StartDate,
    DateTime EndDate,
    ReservationState Type,
    decimal TotalAmount,
    int UserId
)
{
    public static ReservationDto Create(Reservation entity)
    {
        return new ReservationDto(
            entity.Id,
            entity.ReservationCode,
            entity.CreationDate,
            entity.StartDate,
            entity.EndDate,
            entity.Type,
            entity.TotalAmount,
            entity.UserId
        );
    }

    public static List<ReservationDto> CreateList(IEnumerable<Reservation> entities)
    {
        return entities.Select(Create).ToList();
    }
}

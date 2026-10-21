using Domain;

namespace Application.Models;

// La reserva con sus datos y la lista de habitaciones que tiene adentro (Details),
// así el cliente ve qué reservó exactamente.
public record ReservationDto(
    int Id,
    string ReservationCode,
    DateTime CreationDate,
    DateTime StartDate,
    DateTime EndDate,
    ReservationState Type,
    decimal TotalAmount,
    int UserId,
    List<DetailReservationDto> Details
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
            entity.UserId,
            DetailReservationDto.CreateList(entity.DetailReservations)
        );
    }

    public static List<ReservationDto> CreateList(IEnumerable<Reservation> entities)
    {
        return entities.Select(Create).ToList();
    }
}

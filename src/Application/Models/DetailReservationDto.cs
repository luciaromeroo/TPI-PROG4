using Domain;

namespace Application.Models;

// Una habitación que va dentro de la reserva: número, cuántos huéspedes van y lo que sale por noche.
// La requiere que el include traiga la Room cargada, si no no hay número ni precio que mostrar.
public record DetailReservationDto(
    int RoomId,
    int RoomNumber,
    int GuestsNumber,
    decimal PricePerNight
)
{
    public static DetailReservationDto Create(DetailReservation entity)
    {
        return new DetailReservationDto(
            entity.RoomId,
            entity.Room.Number,
            entity.GuestsNumber,
            entity.Room.PricePerNight
        );
    }

    public static List<DetailReservationDto> CreateList(IEnumerable<DetailReservation> entities)
    {
        return entities.Select(Create).ToList();
    }
}

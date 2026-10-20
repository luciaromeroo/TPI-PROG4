namespace Domain.Interfaces;

// El CRUD ya viene del genérico, acá solo lo propio de reservas.
public interface IReservationRepository : IRepository<Reservation>
{
    IEnumerable<Reservation> GetByUserId(int userId);

    // true si la habitación está libre entre esas fechas (ninguna reserva sin cancelar la tapa).
    // excludeReservationId sirve para el update: la reserva no se cuenta a sí misma como ocupante.
    bool IsRoomAvailable(int roomId, DateTime start, DateTime end, int? excludeReservationId = null);
}

using Domain;
using Domain.Interfaces;

namespace Infrastructure.Data;

public class RoomRepository : Repository<Room>, IRoomRepository
{
    public RoomRepository(ApplicationContext context) : base(context) { }

    public IEnumerable<Room> GetAvailableRooms(DateTime start, DateTime end)
    {
        return _dbSet
            .Where(room =>
                room.State
                && !room.DetailReservations.Any(detail =>
                    detail.Reservation.Type != ReservationState.Canceled
                    && start < detail.Reservation.EndDate
                    && end > detail.Reservation.StartDate))
            .OrderBy(room => room.Number)
            .ToList();
    }

    public bool ExistsByNumber(int number, int? excludeRoomId = null)
    {
        return _dbSet.Any(room =>
            room.Number == number
            && (excludeRoomId == null || room.Id != excludeRoomId));
    }

    public bool HasReservations(int roomId)
    {
        return _context.DetailReservations.Any(detail => detail.RoomId == roomId);
    }
}

namespace Domain.Interfaces;

public interface IRoomRepository : IRepository<Room>
{
    IEnumerable<Room> GetAvailableRooms(DateTime start, DateTime end);
    bool ExistsByNumber(int number, int? excludeRoomId = null);
    bool HasReservations(int roomId);
}
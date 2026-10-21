using Application.Models;
using Application.Models.Requests;

namespace Application.Interfaces;

public interface IRoomService
{
    List<RoomDto> GetAllRooms();
    List<RoomDto> GetAvailableRooms(DateTime start, DateTime end);
    RoomDto GetRoomById(int id);
    RoomDto CreateRoom(RoomRequest request);
    RoomDto UpdateRoom(int id, RoomRequest request);
    void DeleteRoom(int id);
}
using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RoomService(IRoomRepository roomRepository, IUnitOfWork unitOfWork)
    {
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
    }

    public List<RoomDto> GetAllRooms()
    {
        return RoomDto.CreateList(_roomRepository.List().OrderBy(room => room.Number));
    }

    public List<RoomDto> GetAvailableRooms(DateTime start, DateTime end)
    {
        ValidateDates(start, end);
        return RoomDto.CreateList(_roomRepository.GetAvailableRooms(start, end));
    }

    public RoomDto GetRoomById(int id)
    {
        var room = _roomRepository.GetById(id)
            ?? throw new NotFoundException("ROOM_NOT_FOUND");

        return RoomDto.Create(room);
    }

    public RoomDto CreateRoom(RoomRequest request)
    {
        ValidateRoom(request.Number, request.Type, request.PricePerNight, request.Capacity);
        var state = request.State
            ?? throw new AppValidationException("ROOM_STATE_REQUIRED");
        EnsureRoomNumberIsAvailable(request.Number);

        var room = new Room
        {
            Number = request.Number,
            Type = request.Type,
            PricePerNight = request.PricePerNight,
            State = state,
            Capacity = request.Capacity
        };

        _roomRepository.Add(room);
        _unitOfWork.SaveChanges();
        return RoomDto.Create(room);
    }

    public RoomDto UpdateRoom(int id, RoomRequest request)
    {
        var room = _roomRepository.GetById(id)
            ?? throw new NotFoundException("ROOM_NOT_FOUND");

        ValidateRoom(request.Number, request.Type, request.PricePerNight, request.Capacity);
        var state = request.State
            ?? throw new AppValidationException("ROOM_STATE_REQUIRED");
        EnsureRoomNumberIsAvailable(request.Number, id);

        room.Number = request.Number;
        room.Type = request.Type;
        room.PricePerNight = request.PricePerNight;
        room.State = state;
        room.Capacity = request.Capacity;

        _roomRepository.Update(room);
        _unitOfWork.SaveChanges();
        return RoomDto.Create(room);
    }

    public void DeleteRoom(int id)
    {
        var room = _roomRepository.GetById(id)
            ?? throw new NotFoundException("ROOM_NOT_FOUND");

        if (_roomRepository.HasReservations(id))
            throw new AppValidationException("ROOM_HAS_RESERVATION_HISTORY");

        _roomRepository.Delete(room);
        _unitOfWork.SaveChanges();
    }

    private void EnsureRoomNumberIsAvailable(int number, int? excludeRoomId = null)
    {
        if (_roomRepository.ExistsByNumber(number, excludeRoomId))
            throw new AppValidationException("ROOM_NUMBER_ALREADY_EXISTS");
    }

    private static void ValidateRoomType(RoomType type)
    {
        if (!Enum.IsDefined(type))
            throw new AppValidationException("ROOM_TYPE_INVALID");
    }

    private static void ValidateRoom(int number, RoomType type, decimal pricePerNight, int capacity)
    {
        ValidateRoomType(type);

        if (number <= 0)
            throw new AppValidationException("ROOM_NUMBER_INVALID");

        if (pricePerNight <= 0)
            throw new AppValidationException("ROOM_PRICE_INVALID");

        if (capacity <= 0)
            throw new AppValidationException("ROOM_CAPACITY_INVALID");
    }

    private static void ValidateDates(DateTime start, DateTime end)
{
    if (start == default || end == default || end <= start)
        throw new AppValidationException("INVALID_ROOM_AVAILABILITY_DATES");

    if (start < DateTime.Today)
        throw new AppValidationException("START_DATE_IN_THE_PAST");
}
}

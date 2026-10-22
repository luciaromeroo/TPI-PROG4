using Domain.Exceptions;

namespace Domain;

// Una habitación dentro de una reserva, con la cantidad de huéspedes.
// Solo la crea Reservation.AddRoom (constructor internal): un detalle no existe sin su reserva.
public class DetailReservation
{
    public int Id { get; private set; }
    public int GuestsNumber { get; private set; }

    public int ReservationId { get; private set; }
    public Reservation Reservation { get; private set; } = null!;
    public int RoomId { get; private set; }
    public Room Room { get; private set; } = null!;

    // Constructor vacío requerido por Entity Framework.
    private DetailReservation() { }

    // Regla "Restricción de capacidad": entre 1 huésped y la capacidad de la habitación.
    internal DetailReservation(Room room, int guestsNumber)
    {
        if (guestsNumber < 1 || guestsNumber > room.Capacity)
            throw new AppValidationException("GUESTS_EXCEED_ROOM_CAPACITY");

        Room = room;
        RoomId = room.Id;
        GuestsNumber = guestsNumber;
    }
}

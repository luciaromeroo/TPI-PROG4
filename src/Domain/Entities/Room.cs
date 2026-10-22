using Domain.Exceptions;

namespace Domain;

// Una habitación del hotel: número, categoría, tarifa por noche, estado operativo y capacidad.
// State: true = habilitada para reservas, false = fuera de servicio (por ejemplo, en mantenimiento).
// Las propiedades tienen "private set": se crean con el constructor y se modifican con Update,
// que validan los datos.
public class Room
{
    private readonly List<DetailReservation> _detailReservations = new();

    public int Id { get; private set; }
    public int Number { get; private set; }
    public RoomType Type { get; private set; }
    public decimal PricePerNight { get; private set; }
    public bool State { get; private set; }
    public int Capacity { get; private set; }

    public IReadOnlyCollection<DetailReservation> DetailReservations => _detailReservations;   // 0..*

    // Constructor vacío requerido por Entity Framework.
    private Room() { }

    public Room(int number, RoomType type, decimal pricePerNight, bool state, int capacity)
    {
        Validate(number, type, pricePerNight, capacity);

        Number = number;
        Type = type;
        PricePerNight = pricePerNight;
        State = state;
        Capacity = capacity;
    }

    // Modifica los datos de la habitación (ABM), con las mismas validaciones que el alta.
    public void Update(int number, RoomType type, decimal pricePerNight, bool state, int capacity)
    {
        Validate(number, type, pricePerNight, capacity);

        Number = number;
        Type = type;
        PricePerNight = pricePerNight;
        State = state;
        Capacity = capacity;
    }

    private static void Validate(int number, RoomType type, decimal pricePerNight, int capacity)
    {
        if (!Enum.IsDefined(type))
            throw new AppValidationException("ROOM_TYPE_INVALID");

        if (number <= 0)
            throw new AppValidationException("ROOM_NUMBER_INVALID");

        if (pricePerNight <= 0)
            throw new AppValidationException("ROOM_PRICE_INVALID");

        if (capacity <= 0)
            throw new AppValidationException("ROOM_CAPACITY_INVALID");
    }
}

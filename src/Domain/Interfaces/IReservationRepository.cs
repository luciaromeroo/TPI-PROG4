namespace Domain.Interfaces;

// El CRUD ya viene del genérico, acá solo lo propio de reservas: buscar las de un cliente.
public interface IReservationRepository : IRepository<Reservation>
{
    IEnumerable<Reservation> GetByUserId(int userId);
}

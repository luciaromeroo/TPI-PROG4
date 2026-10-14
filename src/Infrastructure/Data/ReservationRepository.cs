using Domain;
using Domain.Interfaces;

namespace Infrastructure.Data;

// Lo básico (GetById, List, Add, Update, Delete) ya viene hecho de Repository.
// Si no encuentra nada devuelve null y el service decide qué hacer con eso.
public class ReservationRepository : Repository<Reservation>, IReservationRepository
{
    public ReservationRepository(ApplicationContext context) : base(context) { }

    // Trae las reservas de un cliente, de la más próxima a la más lejana.
    public IEnumerable<Reservation> GetByUserId(int userId)
    {
        return _dbSet
            .Where(reservation => reservation.UserId == userId)
            .OrderBy(reservation => reservation.StartDate)
            .ToList();
    }
}

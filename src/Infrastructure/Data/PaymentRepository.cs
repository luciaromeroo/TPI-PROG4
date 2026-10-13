using Domain;
using Domain.Interfaces;

namespace Infrastructure.Data;

// GetById, List, Add, Update y Delete vienen de Repository<Payment>.
// Igual que en el ejemplo, GetById devuelve null si no existe: decidir qué hacer es tarea del servicio.
public class PaymentRepository : Repository<Payment>, IPaymentRepository
{
    public PaymentRepository(ApplicationContext context) : base(context) { }

    public IEnumerable<Payment> GetByReservation(int reservationId)
    {
        return _dbSet
            .Where(payment => payment.ReservationId == reservationId)
            .OrderBy(payment => payment.PaymentDate)
            .ToList();
    }

    public int GetLastTransactionNumber()
    {
        return _dbSet.Max(payment => (int?)payment.TransactionNumber) ?? 0;
    }
}

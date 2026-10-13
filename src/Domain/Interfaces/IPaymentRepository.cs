namespace Domain.Interfaces;

// Hereda el CRUD del repositorio genérico y agrega solo las consultas propias de Payment.
public interface IPaymentRepository : IRepository<Payment>
{
    IEnumerable<Payment> GetByReservation(int reservationId);
    int GetLastTransactionNumber();
}

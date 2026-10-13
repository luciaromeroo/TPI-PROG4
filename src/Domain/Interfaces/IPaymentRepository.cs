using Domain;
using Domain.Interfaces;

public interface IPaymentRepository
{
    Payment GetById(int id);
    List<Payment> List();
    Payment Add(Payment entity);
    void Update(Payment entity);
    void Delete(Payment entity);
}
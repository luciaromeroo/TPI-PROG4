using Domain;
using Domain.Interfaces;

namespace Infrastructure.Data;
public class PaymentRepository : IPaymentRepository
{
    private readonly ApplicationContext _context;

    public PaymentRepository(ApplicationContext context)
    {
        _context = context;
    }

    public Payment GetById(int id)
    {
        return _context.Payments.FirstOrDefault(payment => payment.Id == id)
            ?? throw new KeyNotFoundException($"Payment with id {id} not found.");
    }

    public List<Payment> List()
    {
        return _context.Payments.ToList();
    }

    public Payment Add(Payment entity)
    {
        _context.Payments.Add(entity);
        _context.SaveChanges();
        return entity;
    }

    public void Update(Payment entity)
    {
        if (!_context.Payments.Any(payment => payment.Id == entity.Id))
        {
            throw new KeyNotFoundException($"Payment with id {entity.Id} not found.");
        }

        _context.Payments.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(Payment entity)
    {
        var payment = _context.Payments.Find(entity.Id)
            ?? throw new KeyNotFoundException($"Payment with id {entity.Id} not found.");

        _context.Payments.Remove(payment);
        _context.SaveChanges();
    }
}
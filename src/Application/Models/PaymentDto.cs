using Domain;

namespace Application.Models;

public record PaymentDto(
    int Id,
    int TransactionNumber,
    decimal Amount,
    string PaymentMethod,
    bool PaymentState,
    DateTime PaymentDate,
    int ReservationId
)
{
    public static PaymentDto Create(Payment entity)
    {
        return new PaymentDto(
            entity.Id,
            entity.TransactionNumber,
            entity.Amount,
            entity.PaymentMethod,
            entity.PaymentState,
            entity.PaymentDate,
            entity.ReservationId
        );
    }

    public static List<PaymentDto> CreateList(IEnumerable<Payment> entities)
    {
        return entities.Select(Create).ToList();
    }
}

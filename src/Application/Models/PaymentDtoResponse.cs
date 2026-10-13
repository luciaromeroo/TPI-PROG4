using Domain;

namespace Application.Models;
public record PaymentDtoResponse(
    int Id,
    int TransactionNumber,
    decimal Amount,
    string PaymentMethod,
    bool PaymentState,
    DateTime PaymentDate,
    int ReservationId
)
{
    public static PaymentDtoResponse Create(Payment entity)
    {
        return new PaymentDtoResponse(
            entity.Id,
            entity.TransactionNumber,
            entity.Amount,
            entity.PaymentMethod,
            entity.PaymentState,
            entity.PaymentDate,
            entity.ReservationId
        );
    }

    public static List<PaymentDtoResponse> Create(IEnumerable<Payment> entities)
    {
        return entities.Select(Create).ToList();
    }
} 
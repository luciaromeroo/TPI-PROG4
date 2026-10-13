using Domain;

namespace Application.Models;
public record PaymentDtoRequest(
    int Id,
    int TransactionNumber,
    decimal Amount,
    string PaymentMethod,
    bool PaymentState,
    DateTime PaymentDate,
    int ReservationId
);
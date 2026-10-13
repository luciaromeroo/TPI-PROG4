namespace Application.Models;

// Estado de cuenta de una reserva: cuánto se pagó (acreditado), cuánto está registrado
// pero sin acreditar (por ejemplo, el saldo a pagar en el hotel) y cuánto falta registrar.
public record ReservationBalanceDto(
    int ReservationId,
    decimal TotalAmount,
    decimal ConfirmedAmount,
    decimal PendingAmount,
    decimal RemainingAmount,
    bool IsFullyPaid,
    List<PaymentDto> Payments
);

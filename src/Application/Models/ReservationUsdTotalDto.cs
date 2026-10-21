namespace Application.Models;

// Total de una reserva en pesos y su equivalente en dólares, con la cotización usada.
public record ReservationUsdTotalDto(
    int ReservationId,
    string ReservationCode,
    decimal TotalAmountArs,
    decimal TotalAmountUsd,
    decimal ExchangeRate,
    DateTime ExchangeRateUpdatedAt
);

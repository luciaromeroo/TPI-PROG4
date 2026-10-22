using Application.Interfaces;
using Application.Models;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services;

// Convierte el total de una reserva (en pesos) a dólares con la cotización del día,
// que se obtiene del servicio externo DolarApi a través de IDolarApiService.
public class ReservationUsdService : IReservationUsdService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IDolarApiService _dolarApiService;

    public ReservationUsdService(
        IReservationRepository reservationRepository,
        IDolarApiService dolarApiService)
    {
        _reservationRepository = reservationRepository;
        _dolarApiService = dolarApiService;
    }

    public async Task<ReservationUsdTotalDto> GetTotalInUsdAsync(int reservationId, CurrentUser currentUser)
    {
        var reservation = _reservationRepository.GetById(reservationId)
            ?? throw new NotFoundException("RESERVATION_NOT_FOUND");

        // Mismos permisos que ver la reserva: el dueño o el staff.
        currentUser.EnsureCanAccess(reservation.UserId);

        var dolar = await _dolarApiService.GetDolarOficialAsync();

        if (dolar is null || dolar.Venta <= 0)
            throw new AppValidationException("INVALID_EXCHANGE_RATE");

        // Se usa el precio de venta: lo que cuesta cada dólar en pesos.
        decimal totalUsd = Math.Round(reservation.TotalAmount / dolar.Venta, 2);

        return new ReservationUsdTotalDto(
            reservation.Id,
            reservation.ReservationCode,
            reservation.TotalAmount,
            totalUsd,
            dolar.Venta,
            dolar.FechaActualizacion
        );
    }
}

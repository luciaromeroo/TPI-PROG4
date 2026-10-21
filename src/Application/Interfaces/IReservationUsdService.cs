using Application.Models;

namespace Application.Interfaces;

public interface IReservationUsdService
{
    Task<ReservationUsdTotalDto> GetTotalInUsdAsync(int reservationId, CurrentUser currentUser);
}

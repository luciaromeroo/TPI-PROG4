using Application.Models;
using Application.Models.Requests;

namespace Application.Interfaces;

public interface IReservationService
{
    List<ReservationDto> GetAllReservations();
    ReservationDto GetReservationById(int id);
    List<ReservationDto> GetReservationsByUser(int userId);
    ReservationDto CreateReservation(CreateReservationRequest request);
    ReservationDto UpdateReservation(int id, UpdateReservationRequest request);
    ReservationDto ConfirmReservation(int id);
    ReservationDto CancelReservation(int id);
    void DeleteReservation(int id);
}

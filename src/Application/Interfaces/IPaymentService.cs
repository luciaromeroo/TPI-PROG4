using Application.Models;
using Application.Models.Requests;

namespace Application.Interfaces;

public interface IPaymentService
{
    List<PaymentDto> GetAllPayments();
    PaymentDto GetPaymentById(int id);
    List<PaymentDto> GetPaymentsByReservation(int reservationId);
    ReservationBalanceDto GetReservationBalance(int reservationId);
    PaymentDto RegisterPayment(CreatePaymentRequest request);
    PaymentDto ConfirmPayment(int id);
    void DeletePayment(int id);
}

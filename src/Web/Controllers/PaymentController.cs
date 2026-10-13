using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

// Controller delgado: solo traduce HTTP <-> servicio. Los errores los resuelve
// GlobalExceptionHandlingMiddleware (NotFoundException -> 404, AppValidationException -> 400).
[Route("api/payments")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    public ActionResult<List<PaymentDto>> GetAll()
    {
        return Ok(_paymentService.GetAllPayments());
    }

    [HttpGet("{id}")]
    public ActionResult<PaymentDto> GetById([FromRoute] int id)
    {
        return Ok(_paymentService.GetPaymentById(id));
    }

    [HttpGet("reservation/{reservationId}")]
    public ActionResult<List<PaymentDto>> GetByReservation([FromRoute] int reservationId)
    {
        return Ok(_paymentService.GetPaymentsByReservation(reservationId));
    }

    [HttpGet("reservation/{reservationId}/balance")]
    public ActionResult<ReservationBalanceDto> GetReservationBalance([FromRoute] int reservationId)
    {
        return Ok(_paymentService.GetReservationBalance(reservationId));
    }

    [HttpPost]
    public ActionResult<PaymentDto> Register([FromBody] CreatePaymentRequest request)
    {
        var created = _paymentService.RegisterPayment(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPatch("{id}/confirm")]
    public ActionResult<PaymentDto> Confirm([FromRoute] int id)
    {
        return Ok(_paymentService.ConfirmPayment(id));
    }

    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute] int id)
    {
        _paymentService.DeletePayment(id);
        return NoContent();
    }
}

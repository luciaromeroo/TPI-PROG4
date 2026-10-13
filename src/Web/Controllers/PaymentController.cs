using Microsoft.AspNetCore.Mvc;
using Application.Models;
using Domain.Interfaces;

[Route("api/payments")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentRepository _PaymentRepository;

    public PaymentController(IPaymentRepository PaymentRepository)
    {
        _PaymentRepository = PaymentRepository;
    }

    [HttpPost]
    public ActionResult<PaymentDtoResponse> Post([FromBody] PaymentDtoRequest prPaymentDtoRequest)
    {
        var entity = new Domain.Payment(
            prPaymentDtoRequest.Id,
            prPaymentDtoRequest.TransactionNumber,
            prPaymentDtoRequest.Amount,
            prPaymentDtoRequest.PaymentMethod,
            prPaymentDtoRequest.PaymentState,
            prPaymentDtoRequest.PaymentDate,
            prPaymentDtoRequest.ReservationId
        );

        var result = _PaymentRepository.Add(entity);

        return PaymentDtoResponse.Create(result);
    }

    [HttpGet]
    public ActionResult<List<PaymentDtoResponse>> Get()
    {
        var result = _PaymentRepository.List();
        return PaymentDtoResponse.Create(result);
    }

    [HttpGet("{prId}")]
    public ActionResult<PaymentDtoResponse> GetById([FromRoute] int prId)
    {
        var result = _PaymentRepository.GetById(prId);
        return PaymentDtoResponse.Create(result);
    }
}
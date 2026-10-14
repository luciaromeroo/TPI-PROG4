using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

// Solo traduce HTTP <-> servicio, acá no hay lógica.
// Los errores los resuelve el middleware (no encontrado -> 404, validación -> 400).
[Route("api/reservations")]
[ApiController]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpGet]
    public ActionResult<List<ReservationDto>> GetAll()
    {
        return Ok(_reservationService.GetAllReservations());
    }

    [HttpGet("{id}")]
    public ActionResult<ReservationDto> GetById([FromRoute] int id)
    {
        return Ok(_reservationService.GetReservationById(id));
    }

    [HttpGet("user/{userId}")]
    public ActionResult<List<ReservationDto>> GetByUser([FromRoute] int userId)
    {
        return Ok(_reservationService.GetReservationsByUser(userId));
    }

    [HttpPost]
    public ActionResult<ReservationDto> Create([FromBody] CreateReservationRequest request)
    {
        var created = _reservationService.CreateReservation(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public ActionResult<ReservationDto> Update([FromRoute] int id, [FromBody] UpdateReservationRequest request)
    {
        return Ok(_reservationService.UpdateReservation(id, request));
    }

    [HttpPatch("{id}/confirm")]
    public ActionResult<ReservationDto> Confirm([FromRoute] int id)
    {
        return Ok(_reservationService.ConfirmReservation(id));
    }

    [HttpPatch("{id}/cancel")]
    public ActionResult<ReservationDto> Cancel([FromRoute] int id)
    {
        return Ok(_reservationService.CancelReservation(id));
    }

    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute] int id)
    {
        _reservationService.DeleteReservation(id);
        return NoContent();
    }
}

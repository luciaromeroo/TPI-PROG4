using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

// Cotización del dólar oficial del día (servicio externo DolarApi).
// Pública: el frontend la puede mostrar sin que el usuario esté logueado.
[Route("api/dolar")]
[ApiController]
[AllowAnonymous]
public class DolarApiController : ControllerBase
{
    private readonly IDolarApiService _dolarApiService;

    public DolarApiController(IDolarApiService dolarApiService)
    {
        _dolarApiService = dolarApiService;
    }

    [HttpGet("oficial")]
    public async Task<IActionResult> GetDolarOficial()
    {
        return Ok(await _dolarApiService.GetDolarOficialAsync());
    }
}

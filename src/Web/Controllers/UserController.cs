using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

// Solo traduce HTTP <-> servicio, acá no hay lógica.
// Los errores los resuelve el middleware (no encontrado -> 404, validación -> 400).
[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public ActionResult<List<UserDto>> GetAll()
    {
        return Ok(_userService.GetAllUsers());
    }

    [HttpGet("{id}")]
    public ActionResult<UserDto> GetById([FromRoute] int id)
    {
        return Ok(_userService.GetUserById(id));
    }

    [HttpPost]
    public ActionResult<UserDto> Create([FromBody] CreateUserRequest request)
    {
        var created = _userService.CreateUser(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
}

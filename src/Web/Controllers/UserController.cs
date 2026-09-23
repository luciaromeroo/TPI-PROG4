using Microsoft.AspNetCore.Mvc;
using Application.Models;
using Domain.Interfaces;
using System.Buffers;
using Infrastructure.Data;
using Domain;


namespace User.Controllers;


[ApiController]

[Route("[controller]")]
public class UserController : ControllerBase
{
    private IUserRepository _UserRepository;

        public UserController(IUserRepository UserRepository)
    {
        _UserRepository = UserRepository;
    }

    
    [HttpPost]
    public ActionResult<UserDto> Post([FromBody] PostUserRequest prPostUserRequest)
    {

        var entity = new Domain.User(
            prPostUserRequest.Id,
            prPostUserRequest.Nombre,
            prPostUserRequest.Apellido,
            prPostUserRequest.Dni,
            prPostUserRequest.Mail,
            prPostUserRequest.Telefono
        );

        var result = _UserRepository.Add(entity);

        return UserDto.Create(result);
        
    }

    [HttpGet]
     public ActionResult<List<UserDto>> Get()
    {

       
        var result =  _UserRepository.List();
        
        return UserDto.Create(result);
        
    }

    [HttpGet("{prId}")]
     public ActionResult<UserDto> GetById([FromRoute] int prId)
    {
        var result =  _UserRepository.GetById(prId);
        
        return UserDto.Create(result);
        
    }


   
}

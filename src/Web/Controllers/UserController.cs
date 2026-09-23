using Microsoft.AspNetCore.Mvc;
using Application.Models;
using Domain.Interfaces;
using System.Buffers;
using Infrastructure.Data;
using Domain;


namespace Client.Controllers;


[ApiController]

[Route("[controller]")]
public class ClientController : ControllerBase
{
    private IUserRepository _UserRepository;

        public ClientController(IUserRepository CRepository)
    {
        _UserRepository = CRepository;
    }

    
    [HttpPost]
    public ActionResult<UserDto> Post([FromBody] PostUserRequest prPostClientRequest)
    {

        var entity = new User(
            prPostClientRequest.Id,
            prPostClientRequest.Nombre,
            prPostClientRequest.Apellido,
            prPostClientRequest.Dni,
            prPostClientRequest.Mail,
            prPostClientRequest.Telefono
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

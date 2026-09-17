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
    private IClientRepository _ClientRepository;

        public ClientController(IClientRepository CRepository)
    {
        _ClientRepository = CRepository;
    }

    
    [HttpPost]
    public ActionResult<ClientDto> Post([FromBody] PostClientRequest prPostClientRequest)
    {

        var entity = new Cliente(
            prPostClientRequest.Id,
            prPostClientRequest.Nombre,
            prPostClientRequest.Apellido,
            prPostClientRequest.Dni,
            prPostClientRequest.Mail,
            prPostClientRequest.Telefono
        );

        var result = _ClientRepository.Add(entity);

        return ClientDto.Create(result);
        
    }

    [HttpGet]
     public ActionResult<List<ClientDto>> Get()
    {

       
        var result =  _ClientRepository.List();
        
        return ClientDto.Create(result);
        
    }

    [HttpGet("{prId}")]
     public ActionResult<ClientDto> GetById([FromRoute] int prId)
    {
        var result =  _ClientRepository.GetById(prId);
        
        return ClientDto.Create(result);
        
    }


   
}

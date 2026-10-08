using Microsoft.AspNetCore.Mvc;
using Application.Models;
using Domain.Interfaces;







[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _UserRepository;

        public UserController(IUserRepository UserRepository)
    {
        _UserRepository = UserRepository;
    }

    
    [HttpPost]
    public ActionResult<UserDto> Post([FromBody] PostUserRequest prPostUserRequest)
    {

        var entity = new Domain.User(
            prPostUserRequest.Id,
            prPostUserRequest.Name,
            prPostUserRequest.LastName,
            prPostUserRequest.Mail,
            prPostUserRequest.Password,
            prPostUserRequest.Dni,
            prPostUserRequest.Phone,
            prPostUserRequest.Role
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

using Application.Models;
using Application.Models.Requests;

namespace Application.Interfaces;

public interface IUserService
{
    List<UserDto> GetAllUsers();
    UserDto GetUserById(int id);
    UserDto CreateUser(CreateUserRequest request);
}

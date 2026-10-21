using Application.Models;
using Application.Models.Requests;

namespace Application.Interfaces;

public interface IUserService
{
    List<UserDto> GetAllUsers();
    UserDto GetUserById(int id, CurrentUser currentUser);
    UserDto CreateUser(CreateUserRequest request);
    UserDto CreateStaffUser(CreateStaffUserRequest request);
}

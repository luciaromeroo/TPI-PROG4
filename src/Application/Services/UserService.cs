using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services;

// Reglas de usuarios:
// - El Id lo genera la base.
// - Los datos obligatorios los valida la entidad User en su constructor.
// - No puede haber dos usuarios con el mismo mail.
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // Trae todos los usuarios.
    public List<UserDto> GetAllUsers()
    {
        var users = _userRepository.List();
        return UserDto.CreateList(users);
    }

    // Trae un usuario por su id.
    public UserDto GetUserById(int id)
    {
        var user = _userRepository.GetById(id)
            ?? throw new NotFoundException("USER_NOT_FOUND");

        return UserDto.Create(user);
    }

    // Registra un usuario nuevo.
    public UserDto CreateUser(CreateUserRequest request)
    {
        // El constructor valida los campos y deja el mail normalizado (trim + minúsculas).
        var user = new User(
            request.Name,
            request.LastName,
            request.Mail,
            request.Password,
            request.Dni,
            request.Phone,
            request.Role
        );

        if (_userRepository.GetByMail(user.Mail) is not null)
            throw new AppValidationException("USER_MAIL_ALREADY_EXISTS");

        _userRepository.Add(user);

        return UserDto.Create(user);
    }
}

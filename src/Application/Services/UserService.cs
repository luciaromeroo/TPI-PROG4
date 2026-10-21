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
// - La contraseña se guarda hasheada, nunca en texto plano.
// - El registro público crea siempre un Client; otros roles los crea un SuperAdmin.
// - Un cliente solo puede ver su propio perfil; el staff puede ver cualquiera.
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    // Trae todos los usuarios (solo staff, lo controla el controller).
    public List<UserDto> GetAllUsers()
    {
        var users = _userRepository.List();
        return UserDto.CreateList(users);
    }

    // Trae un usuario por su id.
    public UserDto GetUserById(int id, CurrentUser currentUser)
    {
        currentUser.EnsureCanAccess(id);

        var user = _userRepository.GetById(id)
            ?? throw new NotFoundException("USER_NOT_FOUND");

        return UserDto.Create(user);
    }

    // Registro público (SignUp): siempre Client.
    public UserDto CreateUser(CreateUserRequest request)
    {
        return Register(request.Name, request.LastName, request.Mail, request.Password,
            request.Dni, request.Phone, Role.Client);
    }

    // Alta hecha por un SuperAdmin, con el rol que elija.
    public UserDto CreateStaffUser(CreateStaffUserRequest request)
    {
        return Register(request.Name, request.LastName, request.Mail, request.Password,
            request.Dni, request.Phone, request.Role);
    }

    private UserDto Register(string name, string lastName, string mail, string password,
        string dni, string phone, Role role)
    {
        // El constructor valida los campos y deja el mail normalizado (trim + minúsculas).
        var user = new User(
            name,
            lastName,
            mail,
            _passwordHasher.Hash(password),
            dni,
            phone,
            role
        );

        if (_userRepository.GetByMail(user.Mail) is not null)
            throw new AppValidationException("USER_MAIL_ALREADY_EXISTS");

        _userRepository.Add(user);

        return UserDto.Create(user);
    }
}

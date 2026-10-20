using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services;

// Login: valida mail + contraseña y, si están bien, pide el token.
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public LoginResponse Login(LoginRequest request)
    {
        var user = _userRepository.GetByMail(request.Mail.Trim().ToLowerInvariant());

        // Mismo error si no existe el mail o si la contraseña está mal:
        // así no se le revela a nadie qué mails están registrados.
        if (user is null || !_passwordHasher.Verify(request.Password, user.Password))
            throw new InvalidCredentialsException("INVALID_CREDENTIALS");

        var token = _tokenService.GenerateToken(user);

        return new LoginResponse(token, UserDto.Create(user));
    }
}

using Application.Models;
using Application.Models.Requests;

namespace Application.Interfaces;

// Se llama IAuthService (y no IAuthenticationService) para no chocar con
// Microsoft.AspNetCore.Authentication.IAuthenticationService de ASP.NET.
public interface IAuthService
{
    LoginResponse Login(LoginRequest request);
}

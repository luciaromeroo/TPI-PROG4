namespace Application.Models;

// Nombres de los datos (claims) que viajan dentro del token.
// Los usa JwtTokenService para escribirlos y Web para leerlos, así no hay strings sueltos.
public static class AuthClaimTypes
{
    public const string UserId = "sub";          // estándar JWT: identificador del usuario
    public const string Role = "role";
    public const string GivenName = "given_name";
    public const string FamilyName = "family_name";
    public const string Email = "email";
}

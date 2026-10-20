namespace Infrastructure.Services;

// Configuración del token. Se lee de appsettings.json, sección "Jwt".
public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string SecretKey { get; set; } = string.Empty;   // mínimo 32 caracteres (HMAC-SHA256)
    public string Issuer { get; set; } = string.Empty;      // quién emite el token (esta API)
    public string Audience { get; set; } = string.Empty;    // para quién es el token (el frontend)
    public int ExpirationMinutes { get; set; } = 60;
}

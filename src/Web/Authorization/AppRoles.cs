using Domain;

namespace Web.Authorization;

// Nombres de los roles para usar en [Authorize(Roles = ...)].
// Salen del enum Role con nameof, así si se renombra un rol el compilador avisa.
public static class AppRoles
{
    public const string SuperAdmin = nameof(Role.SuperAdmin);
    public const string Administrator = nameof(Role.Administrator);
    public const string Client = nameof(Role.Client);

    // "Staff" = cualquiera de los dos roles administrativos.
    public const string Staff = Administrator + "," + SuperAdmin;
}

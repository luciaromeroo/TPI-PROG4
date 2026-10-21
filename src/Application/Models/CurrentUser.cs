using Domain;
using Domain.Exceptions;

namespace Application.Models;

// Quién está haciendo el request, armado a partir del token.
// El controller lo construye y se lo pasa al servicio, que decide si tiene permiso.
public record CurrentUser(int Id, Role Role)
{
    // Administrator y SuperAdmin pueden ver y gestionar todo.
    public bool IsStaff => Role is Role.Administrator or Role.SuperAdmin;

    // Un cliente solo puede acceder a lo suyo; el staff, a todo.
    public void EnsureCanAccess(int ownerUserId)
    {
        if (!IsStaff && ownerUserId != Id)
            throw new NotAllowedException("ACCESS_DENIED");
    }
}

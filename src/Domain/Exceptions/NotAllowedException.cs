namespace Domain.Exceptions;

// El usuario está autenticado pero no tiene permiso sobre ese recurso
// (por ejemplo, un cliente que intenta ver la reserva de otro). El middleware la traduce a 403.
public class NotAllowedException : Exception
{
    public NotAllowedException() : base() { }

    public NotAllowedException(string message) : base(message) { }
}

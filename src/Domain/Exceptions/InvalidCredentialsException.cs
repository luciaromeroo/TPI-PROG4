namespace Domain.Exceptions;

// Mail o contraseña incorrectos al iniciar sesión. El middleware la traduce a 401.
public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException() : base() { }

    public InvalidCredentialsException(string message) : base(message) { }
}

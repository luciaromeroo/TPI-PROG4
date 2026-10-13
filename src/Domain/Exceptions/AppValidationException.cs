namespace Domain.Exceptions;

public class AppValidationException : Exception
{
    public string ErrorCode { get; private set; } = string.Empty;

    public AppValidationException() : base() { }

    public AppValidationException(string message, string errorCode = "") : base(message)
    {
        ErrorCode = errorCode;
    }
}

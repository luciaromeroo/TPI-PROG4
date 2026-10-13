namespace Domain.Exceptions;

public class NotFoundException : Exception
{
    public string ErrorCode { get; private set; } = string.Empty;

    public NotFoundException() : base() { }

    public NotFoundException(string message, string errorCode = "") : base(message)
    {
        ErrorCode = errorCode;
    }
}

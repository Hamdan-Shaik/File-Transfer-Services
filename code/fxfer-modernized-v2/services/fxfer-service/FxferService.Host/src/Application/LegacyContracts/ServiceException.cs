namespace FxferService.Application.LegacyContracts;

public sealed class ServiceException : Exception
{
    public ServiceException()
    {
    }

    public ServiceException(string? message) : base(message)
    {
        Reason = message;
    }

    public ServiceException(string? message, Exception? innerException) : base(message, innerException)
    {
        Reason = message;
    }

    public ServiceException(Exception innerException) : base(innerException.Message, innerException)
    {
        Reason = innerException.Message;
    }

    public string? Reason { get; }
}


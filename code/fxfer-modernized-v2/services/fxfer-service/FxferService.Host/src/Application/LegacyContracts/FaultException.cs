namespace FxferService.Application.LegacyContracts;

public class FaultException : Exception
{
    public FaultException()
    {
    }

    public FaultException(string? message) : base(message)
    {
    }

    public FaultException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

internal sealed class FaultReason
{
    public FaultReason(string? reason)
    {
        Reason = reason;
    }

    public string? Reason { get; }
}

internal sealed class FaultException<T> : FaultException
{
    public FaultException(T detail, FaultReason reason) : base(reason.Reason)
    {
        Detail = detail;
        Reason = reason;
    }

    public T Detail { get; }
    public FaultReason Reason { get; }
}


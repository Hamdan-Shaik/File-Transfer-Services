namespace Fxfer.DomainApi.Contracts.Exceptions;

public sealed class FxferAuthorizationException : Exception
{
    public FxferAuthorizationException(string message)
        : base(message)
    {
    }
}


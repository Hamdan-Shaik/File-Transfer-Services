namespace Fxfer.TransferWorker.Domain.Exceptions;

public sealed class DuplicateFileNameException : Exception
{
    public DuplicateFileNameException(string message) : base(message)
    {
    }
}


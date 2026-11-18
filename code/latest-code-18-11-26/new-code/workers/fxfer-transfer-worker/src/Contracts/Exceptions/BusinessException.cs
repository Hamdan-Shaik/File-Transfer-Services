namespace Fxfer.TransferWorker.Contracts.Exceptions;

public sealed class BusinessException : Exception
{
    public BusinessException(
        IReadOnlyList<BusinessExceptionMessage> validations,
        string message,
        string businessErrorCode)
        : base(message)
    {
        Validations = validations;
        BusinessErrorCode = businessErrorCode;
    }

    public IReadOnlyList<BusinessExceptionMessage> Validations { get; }

    public string BusinessErrorCode { get; }
}


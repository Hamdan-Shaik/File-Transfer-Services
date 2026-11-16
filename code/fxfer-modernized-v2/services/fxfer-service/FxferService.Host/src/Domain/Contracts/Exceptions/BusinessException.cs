namespace FxferService.Domain.Contracts.Exceptions;

public class BusinessException : Exception
{
    public BusinessException(IReadOnlyCollection<BusinessExceptionMessage> businessMessages, string errorType, string processingSystem)
        : base(string.Empty)
    {
        BusinessMessages = businessMessages;
        ErrorType = errorType;
        ProcessingSystem = processingSystem;
    }

    public IReadOnlyCollection<BusinessExceptionMessage> BusinessMessages { get; }

    public string ErrorType { get; }

    public string ProcessingSystem { get; }
}

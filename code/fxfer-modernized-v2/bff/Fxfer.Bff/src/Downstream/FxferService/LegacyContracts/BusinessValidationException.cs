namespace FileTransferConfig;

public sealed class BusinessValidationException
{
    public BusinessExceptionMessage[] BusinessMessages { get; set; } = Array.Empty<BusinessExceptionMessage>();
}

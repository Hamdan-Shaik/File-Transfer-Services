namespace FileTransferSvcQueryService;

public sealed class ServiceException
{
    public string? ErrorCode { get; set; }
    public List<string>? ErrorMessages { get; set; }
}

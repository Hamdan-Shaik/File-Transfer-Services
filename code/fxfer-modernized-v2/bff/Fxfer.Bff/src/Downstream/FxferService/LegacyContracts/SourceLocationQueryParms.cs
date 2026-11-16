namespace FileTransferSvcQueryService;

public sealed class SourceLocationQueryParms
{
    public int ConfigId { get; set; }
    public string? ConfigName { get; set; }
    public string? OwnerName { get; set; }
    public string? OwnerSubsystem { get; set; }
    public string? Protocol { get; set; }
    public bool IsActive { get; set; }
}


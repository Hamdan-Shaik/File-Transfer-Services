namespace Fxfer.DomainApi.Contracts.Domain;

public sealed class FileTransferTrackingModel
{
    public Guid FileId { get; set; }

    public Guid FileSourceLocationId { get; set; }

    public string? FileName { get; set; }

    public string? FileStatus { get; set; }

    public string? FileDirection { get; set; }

    public string? FileTransportType { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? TradingPartner { get; set; }

    public string? CheckSumCode { get; set; }

    public decimal FileSize { get; set; }

    public string? SourceLocAddress { get; set; }

    public string? SourceLocConfigName { get; set; }

    public string? OriginatingServerName { get; set; }

    public List<FileEventModel>? Events { get; set; }

    public List<FileExceptionModel>? Exceptions { get; set; }
}


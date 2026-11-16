namespace FxferService.Infrastructure.Persistence.Batch.Contracts;

public sealed class FileManagementRequestModel
{
    public Guid FileID { get; set; }

    public Guid SourceLocationId { get; set; }

    public string FileName { get; set; } = "";

    public string FileStatus { get; set; } = "";

    public decimal FileSize { get; set; }

    public string ChecksumCode { get; set; } = "";

    public string OperatorId { get; set; } = "";

    public string TransactionId { get; set; } = "";
}

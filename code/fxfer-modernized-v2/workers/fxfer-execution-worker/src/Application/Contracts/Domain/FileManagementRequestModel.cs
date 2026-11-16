using System;

namespace Fxfer.ExecutionWorker.Application.Contracts.Domain;

public class FileManagementRequestModel
{
    public Guid FileID { get; set; }

    public Guid SourceLocationId { get; set; }

    public string FileName { get; set; } = null!;

    public string FileStatus { get; set; } = null!;

    public decimal FileSize { get; set; }

    public Guid GroupId { get; set; }

    public string ChecksumCode { get; set; } = null!;

    public string OperatorId { get; set; } = null!;

    public string TransactionId { get; set; } = null!;
}
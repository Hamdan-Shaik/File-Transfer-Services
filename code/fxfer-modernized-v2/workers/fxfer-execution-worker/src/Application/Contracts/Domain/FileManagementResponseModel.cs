using System;

namespace Fxfer.ExecutionWorker.Application.Contracts.Domain;

public class FileManagementResponseModel
{
    public Guid FileId { get; set; }

    public Guid FileTrackId { get; set; }
}
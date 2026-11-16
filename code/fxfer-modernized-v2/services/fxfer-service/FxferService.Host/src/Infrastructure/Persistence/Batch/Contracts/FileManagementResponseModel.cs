namespace FxferService.Infrastructure.Persistence.Batch.Contracts;

public sealed class FileManagementResponseModel
{
    public Guid FileId { get; set; }

    public Guid FileTrackId { get; set; }
}

namespace Fxfer.ProjectionWorker.Infrastructure.DataAccess.Entities;

public sealed record FtsFileDelivery
{
    public required Guid DeliveryId { get; init; }
    public required Guid FileId { get; init; }
    public required Guid DestinationId { get; init; }
    public required DateTimeOffset DeliveryTs { get; init; }
    public string? DestinationFileName { get; init; }
    public required string OperatorId { get; init; }
    public required DateTimeOffset LastModifiedTs { get; init; }
    public required DateTimeOffset CreatedTs { get; init; }
}


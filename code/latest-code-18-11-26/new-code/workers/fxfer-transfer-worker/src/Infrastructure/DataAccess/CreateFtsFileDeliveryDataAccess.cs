using System.Net.Http.Json;
using Fxfer.TransferWorker.Application.DataAccess;
using Fxfer.TransferWorker.Contracts.Batch;

namespace Fxfer.TransferWorker.Infrastructure.DataAccess;

public sealed class CreateFtsFileDeliveryDataAccess : DataAccessBase<Guid>
{
    private readonly Guid _fileId;
    private readonly Guid _destinationId;
    private readonly string _destinationFileName;
    private readonly string _operatorId;

    public CreateFtsFileDeliveryDataAccess(
        IFxferDomainApiClient domainApiClient,
        IRequestContextAccessor requestContextAccessor,
        Guid fileId,
        Guid destinationId,
        string destinationFileName,
        string operatorId)
        : base(domainApiClient, requestContextAccessor)
    {
        _fileId = fileId;
        _destinationId = destinationId;
        _destinationFileName = destinationFileName;
        _operatorId = operatorId;
    }

    protected override async Task<Guid> ExecuteQueryAsync(CancellationToken cancellationToken)
    {
        var deliveryId = Guid.NewGuid();

        var deliveryRequest = new FileDeliveryCreateRequest
        {
            DeliveryId = deliveryId,
            FileId = _fileId,
            DestinationId = _destinationId,
            DestinationFileName = _destinationFileName,
            DeliveryTs = DateTime.UtcNow,
            OperatorId = _operatorId,
            CreatedDate = DateTime.UtcNow,
            LastModifiedTs = DateTime.UtcNow
        };

        using var httpRequest = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/fxfer-domain/v1/file-tracking/" + _fileId + "/deliveries")
        {
            Content = JsonContent.Create(deliveryRequest)
        };

        using var response = await DomainApiClient.SendAsync(httpRequest, cancellationToken);
        response.EnsureSuccessStatusCode();

        return deliveryId;
    }

    private sealed class FileDeliveryCreateRequest
    {
        public Guid DeliveryId { get; init; }
        public Guid FileId { get; init; }
        public Guid DestinationId { get; init; }
        public string? DestinationFileName { get; init; }
        public DateTime DeliveryTs { get; init; }
        public string? OperatorId { get; init; }
        public DateTime CreatedDate { get; init; }
        public DateTime LastModifiedTs { get; init; }
    }
}


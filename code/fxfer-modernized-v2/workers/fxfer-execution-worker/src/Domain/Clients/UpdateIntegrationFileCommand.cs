using System;

namespace Fxfer.ExecutionWorker.Domain.Clients;

public sealed record UpdateIntegrationFileCommand
{
    public RequestorModel? Requestor { get; init; }

    public Guid FtsIntegrationFileId { get; init; }

    public int? ConfigId { get; init; }

    public bool IsValid { get; init; }
}

using System;

namespace Fxfer.ExecutionWorker.Domain.Clients;

public sealed class FtsIntegrationFileModel
{
    public Guid FtsIntegrationFileId { get; set; }

    public Guid? GroupId { get; set; }

    public int? ConfigId { get; set; }

    public string FileName { get; set; } = string.Empty;

    public Guid ETFOutboundMetadataDefnId { get; set; }

    public bool IsProcessed { get; set; }

    public bool IsValid { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime LastModifiedDate { get; set; }
}


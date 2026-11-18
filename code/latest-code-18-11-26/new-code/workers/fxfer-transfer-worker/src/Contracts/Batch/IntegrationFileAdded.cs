namespace Fxfer.TransferWorker.Contracts.Batch;

public class IntegrationFileAdded
{
    public Guid FtsIntegrationFileId { get; set; }

    public List<GroupInfo> GroupInfo { get; set; } = [];

    public int? ConfigId { get; set; }

    public string FileName { get; set; } = string.Empty;

    public Guid ETFOutboundMetadataId { get; set; }

    public bool IsProcessed { get; set; }

    public string OperatorId { get; set; } = string.Empty;

    public bool IsValid { get; set; }
}


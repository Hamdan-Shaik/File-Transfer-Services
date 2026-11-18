namespace Fxfer.DomainApi.Infrastructure.DataAccess.Entities;

// Legacy entity: HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess.Entities.FtsIntegrationFile
// Table: FTS_INTEGRATION_FILE
// Columns (legacy casing): FTS_INTEGRATION_FILE_ID, GROUP_ID, CONFIG_ID, FILE_NAME,
// ETF_OUTBOUND_METADATA_DEFN_ID, IS_PROCESSED, OPERATOR_ID, IS_VALID, CREATED_TS, LAST_MODIFIED_TS
public sealed record FtsIntegrationFile
{
    public Guid FtsIntegrationFileId { get; init; }
    public Guid? GroupId { get; init; }
    public int? ConfigId { get; init; }
    public required string FileName { get; init; }
    public Guid ETFOutboundMetadataDefnId { get; init; }
    public bool IsProcessed { get; init; }
    public required string OperatorId { get; init; }
    public bool IsValid { get; init; }
    public DateTimeOffset CreationDate { get; init; }
    public DateTimeOffset LastModifiedDate { get; init; }
}


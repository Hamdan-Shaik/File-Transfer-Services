namespace Fxfer.DomainApi.Infrastructure.DataAccess.Entities;

// Legacy entity: HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess.Entities.FtsInboundTarget
// Table: FTS_INBOUND_TARGET
// Columns (legacy casing): FTS_INBOUND_TARGET_ID, EXTERNAL_FILE_DEFN_ID, IMPORT_JOB_NAME, CONTRACT_NAME,
// OPERATOR_ID, IS_VALID, CREATED_TS, LAST_MODIFIED_TS
// Legacy-to-target column naming (fxfer-db-schema): EXTERNAL_FILE_DEFN_ID -> fte_metadata_id
public sealed record FtsInboundTarget
{
    public Guid FtsInboundTargetId { get; init; }
    public Guid FTEMetadataId { get; init; }
    public required string ImportJobName { get; init; }
    public required string ContractName { get; init; }
    public required string OperatorId { get; init; }
    public bool IsValid { get; init; }
    public DateTimeOffset CreationDate { get; init; }
    public DateTimeOffset LastModifiedDate { get; init; }
}


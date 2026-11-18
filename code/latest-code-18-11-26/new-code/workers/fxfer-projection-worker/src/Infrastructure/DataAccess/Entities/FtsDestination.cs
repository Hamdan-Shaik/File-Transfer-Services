namespace Fxfer.ProjectionWorker.Infrastructure.DataAccess.Entities;

public sealed record FtsDestination
{
    public required Guid DestinationId { get; init; }
    public required Guid FtsSourceLocationId { get; init; }
    public required string DestinationAddress { get; init; }
    public required string DestProtocolCd { get; init; }
    public required bool OverwriteAtDestInd { get; init; }
    public required bool CompressAtDestInd { get; init; }
    public required bool RenameAtDestInd { get; init; }
    public required bool DestIsArchiveInd { get; init; }
    public required bool IsValid { get; init; }
    public Guid? FtpServerId { get; init; }
    public string? RenamePattern { get; init; }
    public string? ContactName { get; init; }
    public string? ContactPhone { get; init; }
    public string? ContactEmail { get; init; }
    public required string OperatorId { get; init; }
    public required DateTimeOffset CreatedTs { get; init; }
    public required DateTimeOffset LastModifiedTs { get; init; }
    public string? CdeEncryption { get; init; }
    public string? EncryptionPublicKey { get; init; }
    public string? EncryptionPrivateKeySecretRef { get; init; }
    public string? PrivateKeyPasswordSecretRef { get; init; }
    public Guid? ExternalFileDefnId { get; init; }
    public string? DestinationType { get; init; }
    public string? ContractName { get; init; }
}


namespace Fxfer.DomainApi.Contracts.Domain;

public sealed class FtsInboundTargetModel
{
    public Guid FtsInboundTargetId { get; set; }

    public Guid FTEMetadataId { get; set; }

    public string? ImportJobName { get; set; }

    public string? ContractName { get; set; }

    public bool IsValid { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime LastModifiedDate { get; set; }
}


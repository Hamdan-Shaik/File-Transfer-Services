namespace Fxfer.TransferWorker.Contracts.Domain;

public sealed class DestinationModel
{
    public string? DestinationAddress { get; set; }

    public Guid? FTEMetadataId { get; set; }

    public string? ContractName { get; set; }
}
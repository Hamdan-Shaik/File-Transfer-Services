namespace Fxfer.TransferWorker.Contracts.Domain;

public sealed class SourceLocationModel
{
    public SourceLocationModel()
    {
        Destinations = new List<DestinationModel>();
    }

    public Guid SourceLocationId { get; set; }

    public string? SourceProtocolCode { get; set; }

    public string? SourceAddress { get; set; }

    public string? FileMask { get; set; }

    public string? OwnerName { get; set; }

    public long ConfigurationId { get; set; }

    public string? ConfigurationName { get; set; }

    public string? ConfigurationDescription { get; set; }

    public string? FileDirection { get; set; }

    public List<DestinationModel> Destinations { get; set; }
}
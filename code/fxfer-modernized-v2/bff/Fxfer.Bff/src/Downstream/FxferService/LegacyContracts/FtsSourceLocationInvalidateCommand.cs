namespace FileTransferConfig;

public sealed class FtsSourceLocationInvalidateCommand
{
    public int ConfigId { get; set; }
    public RequestorModel? Requestor { get; set; }
    public string? OperatorId { get; set; }
    public string? FtsSourceLocationId { get; set; }
    public FtsDestinationLocationAdded[] DestinationAddress { get; set; } = Array.Empty<FtsDestinationLocationAdded>();
}

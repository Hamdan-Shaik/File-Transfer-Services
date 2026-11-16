namespace Fxfer.Bff.Downstream.FxferService.Models;

public sealed class TrackingDetailModel
{
    public TrackingInformationModel? Information { get; set; }
    public List<TrackingEventModel>? Events { get; set; }
    public List<TrackingExceptionModel>? Exceptions { get; set; }
}


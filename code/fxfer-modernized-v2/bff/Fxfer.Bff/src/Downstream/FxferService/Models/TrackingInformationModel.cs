using System;

namespace Fxfer.Bff.Downstream.FxferService.Models;

public sealed class TrackingInformationModel
{
    public string? FileId { get; set; }
    public string? FileName { get; set; }
    public DateTime CreateDate { get; set; }
    public ListItemModel Direction { get; set; } = new();
    public ListItemModel TransportType { get; set; } = new();
    public ListItemModel FileStatus { get; set; } = new();
    public decimal Size { get; set; }
    public string? SourceLocation { get; set; }
    public string? TradingPartnerId { get; set; }
}


using System;

namespace Fxfer.Bff.Downstream.FxferService.Models;

public sealed class TrackingSearchResultModel
{
    public string? FileId { get; set; }
    public string? FileName { get; set; }

    public ListItemModel Direction { get; set; } = new();
    public ListItemModel TransportType { get; set; } = new();
    public ListItemModel FileStatus { get; set; } = new();

    public DateTime CreateDate { get; set; }
    public decimal Size { get; set; }
    public string? TradingPartnerId { get; set; }
}
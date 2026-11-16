using System;

namespace Fxfer.Bff.Downstream.FxferService.Models;

public sealed class TrackingExceptionModel
{
    public DateTime Date { get; set; }
    public string? ExceptionMessage { get; set; }
}


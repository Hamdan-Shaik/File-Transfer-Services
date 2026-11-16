using System;

namespace Fxfer.Bff.Downstream.FxferService.Models;

public sealed class InboundTargetModel
{
    public string? ImportJobName { get; set; }
    public Guid FTEMetadataId { get; set; }
}
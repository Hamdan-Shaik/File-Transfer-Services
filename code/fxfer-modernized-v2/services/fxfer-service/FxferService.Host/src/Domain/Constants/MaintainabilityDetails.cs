using System;

namespace FxferService.Domain.Constants;

public class MaintainabilityDetails : Attribute
{
    public int ReviewedOnSprint { get; set; }

    public string? ReviewerName { get; set; }

    public bool IsApproved { get; set; }

    public int ApprovedIndex { get; set; }

    public bool NeedsAttention { get; set; }

    public int TechnicalDebtNumber { get; set; }

    public string? Notes { get; set; }
}

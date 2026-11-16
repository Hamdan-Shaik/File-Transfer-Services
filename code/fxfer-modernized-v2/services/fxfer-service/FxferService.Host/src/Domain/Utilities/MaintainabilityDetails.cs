namespace FxferService.Domain.Utilities;

public sealed class MaintainabilityDetails : Attribute
{
    public int ReviewedOnSprint { get; set; }

    public string ReviewerName { get; set; } = null!;

    public bool IsApproved { get; set; }

    public int ApprovedIndex { get; set; }

    public bool NeedsAttention { get; set; }

    public int TechnicalDebtNumber { get; set; }

    public string Notes { get; set; } = null!;
}


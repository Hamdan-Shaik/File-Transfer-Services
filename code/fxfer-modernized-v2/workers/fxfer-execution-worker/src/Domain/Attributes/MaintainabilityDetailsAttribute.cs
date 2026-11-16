using System;

namespace Fxfer.ExecutionWorker.Domain.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
internal sealed class MaintainabilityDetailsAttribute : Attribute
{
    public int ApprovedIndex { get; set; }
    public int ReviewedOnSprint { get; set; }
    public string ReviewerName { get; set; } = string.Empty;
}

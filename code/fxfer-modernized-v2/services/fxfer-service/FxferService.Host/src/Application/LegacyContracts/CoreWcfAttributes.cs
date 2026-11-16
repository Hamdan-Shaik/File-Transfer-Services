using System;

namespace CoreWCF;

[AttributeUsage(AttributeTargets.Interface)]
public sealed class ServiceContractAttribute : Attribute
{
    public string? Namespace { get; set; }
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class OperationContractAttribute : Attribute;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class FaultContractAttribute : Attribute
{
    public FaultContractAttribute(Type detailType)
    {
        DetailType = detailType;
    }

    public Type DetailType { get; }
}

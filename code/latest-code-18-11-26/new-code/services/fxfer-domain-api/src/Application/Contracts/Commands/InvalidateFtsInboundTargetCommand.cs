using System.Runtime.Serialization;

namespace Fxfer.DomainApi.Contracts.Commands;

[DataContract]
public sealed class InvalidateFtsInboundTargetCommand : AddFtsInboundTargetCommand
{
    public InvalidateFtsInboundTargetCommand()
    {
    }
}


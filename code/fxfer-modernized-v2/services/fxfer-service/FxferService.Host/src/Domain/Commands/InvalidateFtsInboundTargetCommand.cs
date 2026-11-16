using System.Runtime.Serialization;

namespace FxferService.Domain.Commands;

[DataContract]
public class InvalidateFtsInboundTargetCommand : AddFtsInboundTargetCommand
{
    public InvalidateFtsInboundTargetCommand()
    {
    }
}


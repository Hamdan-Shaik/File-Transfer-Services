using System.Runtime.Serialization;

namespace Fxfer.DomainApi.Contracts.Commands;

// Rules: F02-034
[DataContract]
public sealed class InvalidateFtpServerCommand : UpdateFtpServerCommand
{
    public InvalidateFtpServerCommand()
    {
    }
}


using System.Runtime.Serialization;

namespace FxferService.Domain.Commands;

[DataContract]
public class InvalidateFtpServerCommand : UpdateFtpServerCommand
{
    public InvalidateFtpServerCommand()
    {
    }
}


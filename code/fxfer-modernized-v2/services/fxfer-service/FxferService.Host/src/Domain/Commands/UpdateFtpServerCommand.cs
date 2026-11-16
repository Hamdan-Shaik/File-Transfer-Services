using System.Runtime.Serialization;

namespace FxferService.Domain.Commands;

[DataContract]
public class UpdateFtpServerCommand : AddFtpServerCommand
{
    public UpdateFtpServerCommand()
    {
    }
}


using System.Runtime.Serialization;

namespace Fxfer.DomainApi.Contracts.Commands;

// Rules: F02-024
[DataContract]
public class UpdateFtpServerCommand : AddFtpServerCommand
{
    public UpdateFtpServerCommand()
    {
    }
}


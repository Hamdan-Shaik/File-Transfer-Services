using AutoMapper;
using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Events;

namespace Fxfer.DomainApi.Application;

public sealed class FileTransferSvcService(IMapper mapper) : IFileTransferSvcService
{
    public FtsSourceLocationAdded FileTransferAdd(FtsSourceLocationAddCommand command)
    {
        var result = mapper.Map<FtsSourceLocationAdded>(command);
        return result with { OperatorId = GetOperatorId(command) };
    }

    public FtsSourceLocationUpdated UpdateFileTransferSourceLocation(FtsSourceLocationUpdateCommand command)
    {
        var result = mapper.Map<FtsSourceLocationUpdated>(command);
        return result with { OperatorId = GetOperatorId(command) };
    }

    public FtsSourceLocationInvalidated InvalidateFileTransferSourceLocation(FtsSourceLocationInvalidateCommand command)
    {
        return new FtsSourceLocationInvalidated
        {
            FtsSourceLocationsId = command.FtsSourceLocationId,
            ConfigId = command.ConfigId,
            OperatorId = GetOperatorId(command) ?? command.OperatorId,
            DestinationAddress = command.DestinationAddress
        };
    }

    public FtpServerAdded AddFtpServer(AddFtpServerCommand command)
    {
        var result = mapper.Map<FtpServerAdded>(command);
        return result with { IsValid = true, OperatorId = GetOperatorId(command) };
    }

    public FtpServerUpdated UpdateFtpServer(UpdateFtpServerCommand command)
    {
        var result = mapper.Map<FtpServerUpdated>(command);
        return result with { OperatorId = GetOperatorId(command) };
    }

    public FtpServerInvalidated InvalidateFtpServer(InvalidateFtpServerCommand command)
    {
        var result = mapper.Map<FtpServerInvalidated>(command);
        return result with { IsValid = false, OperatorId = GetOperatorId(command) };
    }

    private static string? GetOperatorId(Command command)
    {
        return command.Requestor?.IdentifierId;
    }
}

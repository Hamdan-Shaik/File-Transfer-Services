using System.Collections.Generic;

using FxferService.Application.Contracts.Queries;
using FxferService.Domain.Contracts.Domain;

namespace FxferService.Infrastructure.Persistence;

public interface IFileTransferReadOnlyRepository
{
    IReadOnlyList<FtpServerModel> GetFtpServer(FtpServerQuery query);

    IReadOnlyList<FtsInboundTargetModel> GetInboundTarget(InboundTargetQuery query);
}
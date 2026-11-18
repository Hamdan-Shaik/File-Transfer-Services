using Fxfer.DomainApi.Contracts.Domain;
using Fxfer.DomainApi.Contracts.Queries;

namespace Fxfer.DomainApi.Infrastructure.DataAccess;

public interface IFileTransferReadOnlyRepository
{
    Task<IReadOnlyList<FtpServerModel>> GetFtpServerAsync(FtpServerQuery query, CancellationToken cancellationToken);

    Task<IReadOnlyList<FtsInboundTargetModel>> GetInboundTargetAsync(InboundTargetQuery query, CancellationToken cancellationToken);
}

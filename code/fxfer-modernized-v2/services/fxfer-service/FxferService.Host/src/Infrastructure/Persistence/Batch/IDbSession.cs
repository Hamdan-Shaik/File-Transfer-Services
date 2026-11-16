using System.Data;
using FxferService.Infrastructure.Persistence;

namespace FxferService.Infrastructure.Persistence.Batch;

public interface IDbSession : IDisposable
{
    FxferDbContext DbContext { get; }

    IDbTransaction BeginTransaction();
}

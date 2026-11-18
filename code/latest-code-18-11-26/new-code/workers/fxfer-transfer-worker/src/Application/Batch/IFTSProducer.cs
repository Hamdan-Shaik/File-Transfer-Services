using System.Collections.Concurrent;
using Fxfer.TransferWorker.Contracts.Batch;

namespace Fxfer.TransferWorker.Application.Batch;

public interface IFTSProducer
{
    BlockingCollection<FileToDeliverModel> ProcessAllFilesSources(string tenantId);

    BlockingCollection<FileToDeliverModel> ProcessFailedTransfers(int? topXFailedTransfers, Guid? specificSakFileTracking, int? specificConfigId, string tenantId);
}


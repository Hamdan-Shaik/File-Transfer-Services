using System.Collections.Concurrent;
using Fxfer.TransferWorker.Contracts.Batch;

namespace Fxfer.TransferWorker.Application.Batch;

public interface IFTSConsumer
{
    bool ProcessDeliveryQueue(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, string tenantId);
}


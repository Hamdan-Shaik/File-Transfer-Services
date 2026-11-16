using System.Collections.Concurrent;
using Fxfer.ExecutionWorker.Application.Contracts.Domain;

namespace Fxfer.ExecutionWorker.Batch.Business;

public interface IFTSConsumer
{
    bool ProcessDeliveryQueue(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, string tenantId);
}

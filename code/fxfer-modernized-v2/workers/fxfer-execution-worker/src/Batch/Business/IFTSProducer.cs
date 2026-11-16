using System;
using System.Collections.Concurrent;
using Fxfer.ExecutionWorker.Application.Contracts.Domain;

namespace Fxfer.ExecutionWorker.Batch.Business;

public interface IFTSProducer
{
    BlockingCollection<FileToDeliverModel> ProcessAllFilesSources(string tenantId);

    BlockingCollection<FileToDeliverModel> ProcessFailedTransfers(int? topXFailedTransfers, Guid? specificSakFileTracking, int? specificConfigId, string tenantId);
}


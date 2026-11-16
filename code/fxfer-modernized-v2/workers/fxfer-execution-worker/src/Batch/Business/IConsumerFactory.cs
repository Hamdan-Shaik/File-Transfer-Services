namespace Fxfer.ExecutionWorker.Batch.Business;

public interface IConsumerFactory
{
    IFTSConsumer CreateConsumer(int consumerIdentifier);
}


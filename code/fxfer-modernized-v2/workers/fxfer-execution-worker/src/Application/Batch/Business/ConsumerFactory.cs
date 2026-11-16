namespace Fxfer.ExecutionWorker.Batch.Business;

public sealed class ConsumerFactory : IConsumerFactory
{
    public IFTSConsumer CreateConsumer(int consumerIdentifier)
    {
        return new FTSConsumer(consumerIdentifier);
    }
}

namespace Fxfer.TransferWorker.Application.Batch;

public interface IConsumerFactory
{
    IFTSConsumer CreateConsumer(int consumerIdentifier);
}


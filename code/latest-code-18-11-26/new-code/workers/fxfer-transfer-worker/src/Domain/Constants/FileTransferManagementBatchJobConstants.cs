namespace Fxfer.TransferWorker.Domain.Constants;

public static class FileTransferManagementBatchJobConstants
{
    public const string CommandConfigId = "configId";
    public const string CommandSakTrack = "sakTrack";
    public const string CommandTop = "top";
    public const string EDI = "EDI";
    public const string EventStore = "EventStore";
    public const string IntegrationAggregate = "IntegrationAggregate";
    public const string EventPublisherModule = "Integration";
    public const string OperatorId = "FileTransferBatchProcess";
    public const string ProcessDeliveryQueue = "ProcessDeliveryQueue";

    public static class EventFilters
    {
        public const string UsedByFTE = "UsedByFTE";

        public static class EventMessageNodes
        {
            public const string Item = "Item";
            public const string Key = "Key";
            public const string Value = "Value";
        }
    }
}


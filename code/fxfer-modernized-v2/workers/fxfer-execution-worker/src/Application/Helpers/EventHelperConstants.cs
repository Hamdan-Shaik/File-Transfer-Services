namespace Fxfer.ExecutionWorker.Application.Helpers;

internal static class EventHelperConstants
{
    public const string EventStore = "EventStore";
    public const string IntegrationAggregate = "IntegrationAggregate";
    public const string EventPublisherModule = "Integration";
    public const string OperatorId = "FileTransferBatchProcess";

    public static class EventFilters
    {
        public const string UsedByFTE = "UsedByFTE";

        public class EventMessageNodes
        {
            public const string Item = "Item";
            public const string Key = "Key";
            public const string Value = "Value";
        }
    }
}


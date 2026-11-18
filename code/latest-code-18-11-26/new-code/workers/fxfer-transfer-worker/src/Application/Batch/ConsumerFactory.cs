using Fxfer.TransferWorker.Application.DataAccess;
using Fxfer.TransferWorker.Contracts.NotificationService;
using Fxfer.TransferWorker.Worker.Configuration;

namespace Fxfer.TransferWorker.Application.Batch;

public sealed class ConsumerFactory : IConsumerFactory
{
    private readonly IFileTransferOperations _fileTransferOperations;
    private readonly IDataAccessFactory _dataAccessFactory;
    private readonly INotificationService _notificationService;
    private readonly ITenantAppSettingsManager _configurationManager;
    private readonly IRequestContextAccessor _requestContextAccessor;

    public ConsumerFactory(
        IFileTransferOperations fileTransferOperations,
        IDataAccessFactory dataAccessFactory,
        INotificationService notificationService,
        ITenantAppSettingsManager configurationManager,
        IRequestContextAccessor requestContextAccessor)
    {
        _fileTransferOperations = fileTransferOperations;
        _dataAccessFactory = dataAccessFactory;
        _notificationService = notificationService;
        _configurationManager = configurationManager;
        _requestContextAccessor = requestContextAccessor;
    }

    public IFTSConsumer CreateConsumer(int consumerIdentifier)
    {
        return new FTSConsumer(
            consumerIdentifier,
            _fileTransferOperations,
            _dataAccessFactory,
            _notificationService,
            _configurationManager,
            _requestContextAccessor);
    }
}


using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fxfer.ExecutionWorker.Application.Contracts.Domain;
using Fxfer.ExecutionWorker.Application.Helpers;
using Fxfer.ExecutionWorker.Batch.Business;
using Fxfer.ExecutionWorker.Batch.Business.DataAccess;
using Fxfer.ExecutionWorker.BatchProcess;
using Fxfer.ExecutionWorker.Domain.Batch.Business;
using Fxfer.ExecutionWorker.Domain.Clients;
using Fxfer.ExecutionWorker.Domain.Constants;
using Fxfer.ExecutionWorker.Domain.Exceptions;
using Fxfer.ExecutionWorker.Infrastructure.Tenancy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Fxfer.ExecutionWorker.UnitTests.Specs;

public static class RunFileTransferBatchSteps
{
    public sealed class World
    {
        public string[] RunDetails { get; set; } = Array.Empty<string>();
        public int? Result { get; set; }

        public List<string> Logs { get; } = new();
        public Dictionary<string, string?> AppSettings { get; set; } = new(StringComparer.Ordinal);
        public string ConsoleText => ConsoleBuffer.ToString();

        public RunFileTransferBatchEntrypoint? Entrypoint { get; set; }
        public FakeBatchExecutor? FakeExecutor { get; set; }

        public FakeFileTransferOperations? FileTransferOperations { get; set; }
        public FakeDataAccessFactory? DataAccessFactory { get; set; }
        public FTSConsumer? Consumer { get; set; }
        public FileToDeliverModel? SelectedFile { get; set; }

        internal StringWriter ConsoleBuffer { get; } = new();
        internal TextWriter? OriginalConsoleOut { get; set; }

        public void CaptureConsole()
        {
            OriginalConsoleOut = Console.Out;
            Console.SetOut(ConsoleBuffer);
        }

        public void RestoreConsole()
        {
            if (OriginalConsoleOut is not null)
            {
                Console.SetOut(OriginalConsoleOut);
            }
        }
    }

    public sealed record StepDefinition(Regex Pattern, Func<World, Match, Task> Run);

    public static IReadOnlyList<StepDefinition> Create()
    {
        return new List<StepDefinition>
        {
            new(new Regex(@"^operations schedule a delivery run with some run information$", RegexOptions.Compiled),
                async (world, _) =>
                {
                    var executor = new FakeBatchExecutor(result: 0);
                    world.FakeExecutor = executor;
                    world.RunDetails = new[] { BatchJobConstants.CommandTop, "1" };
                    world.AppSettings = DefaultStandardSettings();
                    world.Entrypoint = BuildEntrypoint(world, executor, settings: world.AppSettings);
                    await Task.CompletedTask;
                }),

            new(new Regex(@"^operations schedule a delivery run with (\d+) pieces of run information$", RegexOptions.Compiled),
                async (world, match) =>
                {
                    var count = int.Parse(match.Groups[1].Value);
                    world.RunDetails = Enumerable.Range(1, count).Select(i => $"arg{i}").ToArray();

                    // Ensure these are not interpreted as management-scheduled arguments (tenant GUID).
                    if (world.RunDetails.Length > 0)
                    {
                        world.RunDetails[^1] = "NOT-A-TENANT";
                    }

                    var appSettings = DefaultStandardSettings();
                    var ftsProducer = new FakeFtsProducer();
                    var consumerFactory = new FakeConsumerFactory();
                    var innerExecutor = new FileTransferBatchExecutor(consumerFactory, ftsProducer, new FakeAppSettings(appSettings));
                    var executor = new BatchExecutorAdapter(innerExecutor);

                    world.AppSettings = appSettings;
                    world.Entrypoint = BuildEntrypoint(world, executor, settings: appSettings);
                    await Task.CompletedTask;
                }),

            new(new Regex(@"^at least one required setting is missing for delivery processing$", RegexOptions.Compiled),
                async (world, _) =>
                {
                    var appSettings = DefaultStandardSettings();
                    appSettings.Remove(AppSettingsConstants.WorkingDirectory);

                    var ftsProducer = new FakeFtsProducer();
                    var consumerFactory = new FakeConsumerFactory();
                    var innerExecutor = new FileTransferBatchExecutor(consumerFactory, ftsProducer, new FakeAppSettings(appSettings));
                    var executor = new BatchExecutorAdapter(innerExecutor);

                    world.RunDetails = Array.Empty<string>();
                    world.AppSettings = appSettings;
                    world.Entrypoint = BuildEntrypoint(world, executor, settings: appSettings);
                    await Task.CompletedTask;
                }),

            new(new Regex(@"^the standard scheduled run starts$", RegexOptions.Compiled),
                async (world, _) =>
                {
                    world.CaptureConsole();
                    try
                    {
                        Assert.NotNull(world.Entrypoint);
                        world.Result = world.Entrypoint!.RunStandardScheduled(world.RunDetails);
                    }
                    finally
                    {
                        world.RestoreConsole();
                    }

                    await Task.CompletedTask;
                }),

            new(new Regex(@"^delivery processing is carried out using that run information$", RegexOptions.Compiled),
                async (world, _) =>
                {
                    Assert.NotNull(world.FakeExecutor);
                    Assert.NotNull(world.FakeExecutor!.LastArgs);
                    Assert.Equal(world.RunDetails, world.FakeExecutor.LastArgs);
                    await Task.CompletedTask;
                }),

            new(new Regex(@"^operations are told the run completed$", RegexOptions.Compiled),
                async (world, _) =>
                {
                    Assert.Equal(0, world.Result);
                    await Task.CompletedTask;
                }),

            new(new Regex(@"^the run is reported as failed$", RegexOptions.Compiled),
                async (world, _) =>
                {
                    Assert.Equal(-1, world.Result);
                    await Task.CompletedTask;
                }),

            new(new Regex(@"^support staff are given guidance on how to start it$", RegexOptions.Compiled),
                async (world, _) =>
                {
                    Assert.Contains("Usage:", world.ConsoleText);
                    await Task.CompletedTask;
                }),

            new(new Regex(@"^support staff are told which setting is missing$", RegexOptions.Compiled),
                async (world, _) =>
                {
                    Assert.Contains(AppSettingsConstants.WorkingDirectory, string.Join("\n", world.Logs));
                    await Task.CompletedTask;
                }),

            new(new Regex(@"^the run hits an unexpected problem before it can complete$", RegexOptions.Compiled),
                async (world, _) =>
                {
                    var executor = new FakeBatchExecutor(throws: new InvalidOperationException("boom"));
                    world.FakeExecutor = executor;
                    world.RunDetails = new[] { BatchJobConstants.CommandTop, "1" };
                    world.AppSettings = DefaultStandardSettings();
                    world.Entrypoint = BuildEntrypoint(world, executor, settings: world.AppSettings);

                    world.CaptureConsole();
                    try
                    {
                        world.Result = world.Entrypoint.RunStandardScheduled(world.RunDetails);
                    }
                    finally
                    {
                        world.RestoreConsole();
                    }

                    await Task.CompletedTask;
                }),

            new(new Regex(@"^a support record is created describing the problem$", RegexOptions.Compiled),
                async (world, _) =>
                {
                    Assert.Contains("boom", string.Join("\n", world.Logs));
                    await Task.CompletedTask;
                }),

            new(new Regex(@"^a selected file cannot be delivered because the destination already has a file with that name$", RegexOptions.Compiled),
                async (world, _) =>
                {
                    var appSettings = DefaultStandardSettings();
                    world.AppSettings = appSettings;

                    var ops = new FakeFileTransferOperations();
                    var dataAccessFactory = new FakeDataAccessFactory();

                    var consumer = new FTSConsumer(
                        fileTransferOperations: ops,
                        dataAccessFactory: dataAccessFactory,
                        appSettings: new FakeAppSettings(appSettings),
                        tenantContext: new TenantContext(),
                        eventHelper: new EventHelper(new FakeDistributedEventStorage()),
                        queryService: new FakeFileTransferSvcQueryServiceProvider(),
                        service: new FakeFileTransferSvcServiceProvider());

                    var destination = new DestinationModel
                    {
                        DestinationProtocolCode = ProtocolCodeConstant.UNC,
                        DestinationAddress = "/dest",
                        DestinationId = Guid.NewGuid(),
                    };

                    var source = new SourceLocationModel
                    {
                        ConfigurationId = 123,
                        SourceProtocolCode = ProtocolCodeConstant.UNC,
                        SourceAddress = "C:\\source",
                        SendNotificationsIndicator = true,
                        FileMask = "*",
                        FtpServerDetail = new FtpServerModel
                        {
                            FtpServerIp = "127.0.0.1",
                            FtpPort = 21,
                            FtpUser = "user",
                            FtpPassword = "pass",
                        },
                        Destinations = new List<DestinationModel> { destination },
                    };

                    world.SelectedFile = new FileToDeliverModel
                    {
                        FileId = Guid.NewGuid(),
                        FileName = "C:\\source\\file.txt",
                        SourceLocation = source,
                    };

                    dataAccessFactory.ConfigMetaDataToReturn = source;

                    world.FileTransferOperations = ops;
                    world.DataAccessFactory = dataAccessFactory;
                    world.Consumer = consumer;

                    await Task.CompletedTask;
                }),

            new(new Regex(@"^duplicate-rename handling is enabled$", RegexOptions.Compiled),
                async (world, _) =>
                {
                    Assert.NotNull(world.FileTransferOperations);
                    world.AppSettings[AppSettingsConstants.ActivateDuplicateRenameLogic] = "true";
                    await Task.CompletedTask;
                }),

            new(new Regex(@"^duplicate-rename handling is not enabled$", RegexOptions.Compiled),
                async (world, _) =>
                {
                    Assert.NotNull(world.FileTransferOperations);
                    world.AppSettings[AppSettingsConstants.ActivateDuplicateRenameLogic] = "false";
                    await Task.CompletedTask;
                }),

            new(new Regex(@"^delivery processing attempts the delivery$", RegexOptions.Compiled),
                async (world, _) =>
                {
                    Assert.NotNull(world.Consumer);
                    Assert.NotNull(world.SelectedFile);

                    var queue = new BlockingCollection<FileToDeliverModel>();
                    queue.Add(world.SelectedFile!);

                    world.Consumer!.ProcessDeliveryQueue(queue, tenantId: "tenant");

                    await Task.CompletedTask;
                }),

            new(new Regex(@"^the source file is renamed with the configured duplicate marker$", RegexOptions.Compiled),
                async (world, _) =>
                {
                    Assert.NotNull(world.FileTransferOperations);
                    Assert.True(world.FileTransferOperations!.RenameWasCalled);
                    Assert.Equal(DefaultDuplicateMarker(), world.FileTransferOperations!.LastRenameExtension);
                    await Task.CompletedTask;
                }),

            new(new Regex(@"^the tracked item is treated as completed$", RegexOptions.Compiled),
                async (world, _) =>
                {
                    Assert.NotNull(world.DataAccessFactory);
                    Assert.Equal(DataAccessConstants.FileStatusConstant.Completed, world.DataAccessFactory!.LastStatus);
                    await Task.CompletedTask;
                }),

            new(new Regex(@"^the tracked item is marked as an error$", RegexOptions.Compiled),
                async (world, _) =>
                {
                    Assert.NotNull(world.DataAccessFactory);
                    Assert.Equal(DataAccessConstants.FileStatusConstant.Error, world.DataAccessFactory!.LastStatus);
                    await Task.CompletedTask;
                }),

            new(new Regex(@"^a support record is created describing the duplicate$", RegexOptions.Compiled),
                async (world, _) =>
                {
                    Assert.NotNull(world.FileTransferOperations);
                    Assert.True(world.FileTransferOperations!.PostEventWasCalled);
                    Assert.Contains("duplicate", world.FileTransferOperations!.LastPostedBody, StringComparison.OrdinalIgnoreCase);
                    await Task.CompletedTask;
                }),
        };
    }

    private static RunFileTransferBatchEntrypoint BuildEntrypoint(World world, Fxfer.ExecutionWorker.BatchProcess.IBatchExecutor<int> executor, Dictionary<string, string?> settings)
    {
        var entryLogger = new CollectingLogger<RunFileTransferBatchEntrypoint>(world.Logs);
        var configLogger = new CollectingLogger<RunFileTransferBatchConfigurationValidation>(world.Logs);

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();

        return new RunFileTransferBatchEntrypoint(
            entryLogger,
            new TenantContext(),
            new RunFileTransferBatchConfigurationValidation(configLogger),
            executor,
            configuration);
    }

    private static Dictionary<string, string?> DefaultStandardSettings()
    {
        return new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            [AppSettingsConstants.WorkingDirectory] = "C:\\work",
            [AppSettingsConstants.MaxConcurrentDeliveryProcesses] = "0",
            [AppSettingsConstants.MaxFilesPerCycle] = "1",
            [AppSettingsConstants.MinsToWaitBeforeRetryFailedFile] = "1",
            [AppSettingsConstants.MinsToWaitBeforeRetrySuspendedConfig] = "1",
            [AppSettingsConstants.DisableNotifications] = "true",
            [AppSettingsConstants.EnvironmentName] = "test",
            [AppSettingsConstants.DeleteArchiveIfExists] = "true",
            [AppSettingsConstants.DuplicateFileExtension] = DefaultDuplicateMarker(),
            [AppSettingsConstants.ActivateDuplicateRenameLogic] = "true",
        };
    }

    private static string DefaultDuplicateMarker() => ".dup";

    private sealed class CollectingLogger<T> : ILogger<T>
    {
        private readonly List<string> sink;

        public CollectingLogger(List<string> sink)
        {
            this.sink = sink;
        }

        public IDisposable BeginScope<TState>(TState state) where TState : notnull => NullScope.Instance;
        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            sink.Add(formatter(state, exception));
            if (exception is not null)
            {
                sink.Add(exception.Message);
            }
        }

        private sealed class NullScope : IDisposable
        {
            public static readonly NullScope Instance = new();
            public void Dispose() { }
        }
    }

    public sealed class FakeBatchExecutor : Fxfer.ExecutionWorker.BatchProcess.IBatchExecutor<int>
    {
        private readonly int result;
        private readonly Exception? throws;

        public FakeBatchExecutor(int result = 0, Exception? throws = null)
        {
            this.result = result;
            this.throws = throws;
        }

        public string[]? LastArgs { get; private set; }

        public int Execute(string[] inputArgs)
        {
            LastArgs = inputArgs;
            if (throws is not null)
            {
                throw throws;
            }

            return result;
        }
    }

    private sealed class BatchExecutorAdapter : Fxfer.ExecutionWorker.BatchProcess.IBatchExecutor<int>
    {
        private readonly Fxfer.ExecutionWorker.Batch.Business.FileTransferBatchExecutor inner;

        public BatchExecutorAdapter(Fxfer.ExecutionWorker.Batch.Business.FileTransferBatchExecutor inner)
        {
            this.inner = inner;
        }

        public int Execute(string[] runDetails) => inner.Execute(runDetails);
    }

    private sealed class FakeAppSettings : IAppSettings
    {
        private readonly IReadOnlyDictionary<string, string?> values;

        public FakeAppSettings(IReadOnlyDictionary<string, string?> values)
        {
            this.values = values;
        }

        public string? this[string key] => values.TryGetValue(key, out var value) ? value : null;
    }

    private sealed class FakeFtsProducer : IFTSProducer
    {
        public BlockingCollection<FileToDeliverModel> ProcessAllFilesSources(string tenantId) => new();

        public BlockingCollection<FileToDeliverModel> ProcessFailedTransfers(int? topXFailedTransfers, Guid? specificSakFileTracking, int? specificConfigId, string tenantId) => new();
    }

    private sealed class FakeConsumerFactory : IConsumerFactory
    {
        public IFTSConsumer CreateConsumer(int consumerIdentifier) => new FakeConsumer();

        private sealed class FakeConsumer : IFTSConsumer
        {
            public bool ProcessDeliveryQueue(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, string tenantId) => true;
        }
    }

    public sealed class FakeFileTransferOperations : IFileTransferOperations
    {
        public bool RenameWasCalled { get; private set; }
        public string? LastRenameExtension { get; private set; }

        public bool PostEventWasCalled { get; private set; }
        public string LastPostedBody { get; private set; } = string.Empty;

        public bool DownloadFile(SourceLocationModel source, string sourceFileName, DestinationModel destination) => true;

        public bool DownloadFile(SourceLocationModel source, string sourceFileName, DestinationModel destination, string tenantId) => true;

        public string UploadFile(string fullyQualifiedSourceFileName, string fullyQualifiedDestinationFileName, DestinationModel destinationLocation)
        {
            throw new DuplicateFileNameException("duplicate name at destination");
        }

        public bool DeleteFile(SourceLocationModel source, string fullyQualifedSourceFile) => true;

        public bool RenameSourceFile(SourceLocationModel source, string fullyQualifedSourceFile, string renameExtension)
        {
            RenameWasCalled = true;
            LastRenameExtension = renameExtension;
            return true;
        }

        public List<FileBasicInfo> GetListOfFileNamesFromSource(SourceLocationModel location, int maxFilesPerCycle) => new();

        public string GetDestinationFileName(string fullyQualifiedSourceFileName, DestinationModel destinationLocation, Guid fileId, string crc32, string uploadedSFTPUserName) => fullyQualifiedSourceFileName;

        public string CreateZipFile(string inputFileName) => inputFileName;

        public void SendNotification(long configId, string subject, string body, bool isErrorNotification) { }

        public void SendNotification(SourceLocationModel source, string subject, string body, bool isErrorNotification, string tenantId) { }

        public void PostEvent(string eventName, string subSystem, string messageBody, bool isArchived, string operatorId)
        {
            PostEventWasCalled = true;
            LastPostedBody = messageBody;
        }

        public string GetCrc32(string filePath) => "";
    }

    public sealed class FakeDataAccessFactory : IDataAccessFactory
    {
        public string? LastStatus { get; private set; }

        public IDataAccess<List<FtsNotificationModel>> CreateQueryFtsNotificationDataAccess(GetNotificationSearchCriteria searchQuery) => new FakeDataAccess<List<FtsNotificationModel>>(new());

        public IDataAccess<List<PendingFileModel>> CreateGetPendingFileListDataAccess() => new FakeDataAccess<List<PendingFileModel>>(new());

        public IDataAccess<List<SourceLocationModel>> CreateGetSourceLocationsDataAccess() => new FakeDataAccess<List<SourceLocationModel>>(new());

        public IDataAccess<FileManagementResponseModel> CreateFileManagementDataAccess(FileManagementRequestModel request) => new FakeDataAccess<FileManagementResponseModel>(new());

        public IDataAccess<int> CreateErrorLogDataAccess(ErrorLogDetailModel errorLog) => new FakeDataAccess<int>(1);

        public SourceLocationModel? ConfigMetaDataToReturn { get; set; }

        public IDataAccess<SourceLocationModel>? CreateGetConfigMetaDataDataAccess(Guid fileId)
        {
            return ConfigMetaDataToReturn is null
                ? null
                : new FakeDataAccess<SourceLocationModel>(ConfigMetaDataToReturn);
        }

        public IDataAccess<Guid> CreateUpdateFileManagementStatusDataAccess(Guid fileId, string fileStatus)
        {
            LastStatus = fileStatus;
            return new FakeDataAccess<Guid>(fileId);
        }

        public IDataAccess<Guid> CreateUpdateSourceSuspendStatusDataAccess(Guid sourceLocationId) => new FakeDataAccess<Guid>(sourceLocationId);

        public IDataAccess<int> CreateFtsSuspensionDataAccess(FtsSuspensionSearchCriteria criteria) => new FakeDataAccess<int>(1);

        public IDataAccess<Guid> CreateFtsFileDeliveryDataAccess(Guid fileId, Guid destinationId, string destinationFileName, string operatorId, List<DestinationModel> destinations) => new FakeDataAccess<Guid>(Guid.NewGuid());

        public IDataAccess<Guid> CreateFtsFileExceptionEventDataAccess(Guid fileId, Guid destinationId, string destinationFileName, string operatorId, ErrorLogDetailModel errorLog) => new FakeDataAccess<Guid>(Guid.NewGuid());

        private sealed class FakeDataAccess<T> : IDataAccess<T>
        {
            private readonly T value;

            public FakeDataAccess(T value)
            {
                this.value = value;
            }

            public T Execute() => value;
        }
    }

    private sealed class FakeFileTransferSvcQueryServiceProvider : IFileTransferSvcQueryServiceProvider
    {
        public FtsIntegrationFileModel? GetIntegrationFile(GetIntegrationFileQuery query) => null;
    }

    private sealed class FakeFileTransferSvcServiceProvider : IFileTransferSvcServiceProvider
    {
        public IntegrationFileUpdated UpdateIntegrationFile(UpdateIntegrationFileCommand command) => new();
    }

    private sealed class FakeDistributedEventStorage : IDistributedEventStorage
    {
        public void Add(EventMessage message) { }
    }
}
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using Fxfer.ExecutionWorker.Application.Contracts.Domain;
using Fxfer.ExecutionWorker.Batch.Business;
using Fxfer.ExecutionWorker.Batch.Business.DataAccess;
using Fxfer.ExecutionWorker.BatchProcess;
using Fxfer.ExecutionWorker.Domain.Batch.Business;
using Fxfer.ExecutionWorker.Domain.Constants;
using Xunit;

namespace Fxfer.ExecutionWorker.UnitTests;

public sealed class BackendBatchBusinessPart2RulesTests
{
    private const string TenantId = "11111111-1111-1111-1111-111111111111";

    [Fact]
    public void F03_012_executor_reports_success_as_zero()
    {
        var producer = new CapturingProducer();
        var executor = CreateExecutor(producer: producer, maxConcurrentDeliveryProcesses: "0");

        var result = executor.Execute(new[] { TenantId });

        Assert.Equal(0, result);
        Assert.Equal(1, producer.AllLocationsRunCount);
    }

    [Fact]
    public void F03_014_unrecognised_command_logs_usage_and_still_runs_all_locations()
    {
        var producer = new CapturingProducer();
        var executor = CreateExecutor(producer: producer, maxConcurrentDeliveryProcesses: "0");

        var (result, output) = CaptureConsole(() => executor.Execute(new[] { "not-a-command", "12", TenantId }));

        Assert.Equal(0, result);
        Assert.Contains("Usage: FileTransferService <command> <command_value>", output);
        Assert.Equal(1, producer.AllLocationsRunCount);
        Assert.Equal(0, producer.FailedTransfersRunCount);
    }

    [Fact]
    public void F03_015_all_locations_run_processes_only_active_eligible_and_scheduled_sources()
    {
        var locationShouldRun = new SourceLocationModel
            {
                SourceLocationId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                ConfigurationId = 1,
                PreviousConfigurationId = 0,
                SourceProtocolCode = "UNC",
                SourceAddress = "x",
                FileMask = "*",
                IsActive = true,
                IsSuspended = false,
            };

            var locationInactive = new SourceLocationModel
            {
                SourceLocationId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                ConfigurationId = 2,
                PreviousConfigurationId = 0,
                SourceProtocolCode = "UNC",
                SourceAddress = "x",
                FileMask = "*",
                IsActive = false,
                IsSuspended = false,
            };

            var locationSuspendedEligible = new SourceLocationModel
            {
                SourceLocationId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                ConfigurationId = 3,
                PreviousConfigurationId = 0,
                SourceProtocolCode = "UNC",
                SourceAddress = "x",
                FileMask = "*",
                IsActive = true,
                IsSuspended = true,
                SuspendedIndChanged = DateTime.Now.AddMinutes(-10),
            };

            var locationSuspendedNotEligible = new SourceLocationModel
            {
                SourceLocationId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                ConfigurationId = 4,
                PreviousConfigurationId = 0,
                SourceProtocolCode = "UNC",
                SourceAddress = "x",
                FileMask = "*",
                IsActive = true,
                IsSuspended = true,
                SuspendedIndChanged = DateTime.Now,
            };

            var locationNotScheduled = new SourceLocationModel
            {
                SourceLocationId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                ConfigurationId = 5,
                PreviousConfigurationId = 0,
                SourceProtocolCode = "UNC",
                SourceAddress = "x",
                FileMask = "*",
                IsActive = true,
                IsSuspended = false,
                Schedule = new SourceScheduleModel
                {
                    StartTime = DateTime.Now.AddHours(1).TimeOfDay,
                    EndTime = DateTime.Now.AddHours(2).TimeOfDay,
                    DaysToRun = new List<DayOfWeek> { DateTime.Now.DayOfWeek },
                    MonthlyProcessDayToRun = new List<int> { DateTime.Now.Day },
                },
            };

            var dataAccessFactory = new CapturingDataAccessFactory
            {
                PendingFiles = new List<PendingFileModel>(),
                SourceLocations = new List<SourceLocationModel>
                {
                    locationShouldRun,
                    locationInactive,
                    locationSuspendedEligible,
                    locationSuspendedNotEligible,
                    locationNotScheduled,
                },
            };

        var fileTransferOperations = new CapturingFileTransferOperations(_ => new List<FileBasicInfo> { new("C:/a/file.txt", 0) });
            var producer = CreateProducer(dataAccessFactory, fileTransferOperations);

        var queue = producer.ProcessAllFilesSources(TenantId);

        Assert.Equal(2, queue.Count);
        Assert.Equal(1, dataAccessFactory.UpdateSourceSuspendStatusExecuteCount);
    }

    [Fact]
    public void F03_016_duplicate_marker_files_are_skipped_when_duplicate_rename_is_active()
    {
        var dataAccessFactory = new CapturingDataAccessFactory
        {
            PendingFiles = new List<PendingFileModel>(),
            SourceLocations = new List<SourceLocationModel>
            {
                new SourceLocationModel
                {
                    SourceLocationId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    ConfigurationId = 10,
                    SourceProtocolCode = "UNC",
                    SourceAddress = "x",
                    FileMask = "*",
                    IsActive = true,
                    IsSuspended = false,
                },
            },
        };

        var fileTransferOperations = new CapturingFileTransferOperations((maxFilesPerCycle) =>
        {
            Assert.Equal(7, maxFilesPerCycle);
            return new List<FileBasicInfo> { new("C:/a/dupe.dup", 0), new("C:/a/keep.txt", 0) };
        });

        var producer = CreateProducer(
            dataAccessFactory,
            fileTransferOperations,
            overrides: s =>
            {
                s[AppSettingsConstants.ActivateDuplicateRenameLogic] = "True";
                s[AppSettingsConstants.DuplicateFileExtension] = ".dup";
                s[AppSettingsConstants.MaxFilesPerCycle] = "7";
            });

        var queue = producer.ProcessAllFilesSources(TenantId);

        Assert.Equal(1, queue.Count);
        Assert.Equal("C:/a/keep.txt", queue.Take().FileName);
    }

    [Fact]
    public void F03_017_tracked_files_are_only_retried_after_retry_window_and_marked_transfering()
    {
        var location = new SourceLocationModel
        {
            SourceLocationId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            ConfigurationId = 20,
            SourceProtocolCode = "UNC",
            SourceAddress = "x",
            FileMask = "*",
            IsActive = true,
            IsSuspended = false,
        };

        var pendingFile = new PendingFileModel
        {
            FileId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
            FileSourceLocationId = location.SourceLocationId,
            FileName = "retry.txt",
            FileStatus = DataAccessConstants.FileStatusConstant.Error,
            FileTrackId = 12,
            StatusDate = DateTime.UtcNow,
        };

        var dataAccessFactory = new CapturingDataAccessFactory
        {
            PendingFiles = new List<PendingFileModel> { pendingFile },
            SourceLocations = new List<SourceLocationModel> { location },
        };

        var fileTransferOperations = new CapturingFileTransferOperations(_ => new List<FileBasicInfo> { new("retry.txt", 0) });
        var producer = CreateProducer(
            dataAccessFactory,
            fileTransferOperations,
            overrides: s => s[AppSettingsConstants.MinsToWaitBeforeRetryFailedFile] = "5");

        var queue = producer.ProcessAllFilesSources(TenantId);

        Assert.Equal(0, queue.Count);
        Assert.Equal(0, dataAccessFactory.UpdateFileStatusExecuteCount);

        pendingFile.StatusDate = DateTime.UtcNow.AddMinutes(-10);
        queue = producer.ProcessAllFilesSources(TenantId);

        Assert.Equal(1, queue.Count);
        Assert.Equal(1, dataAccessFactory.UpdateFileStatusExecuteCount);
        Assert.Equal(pendingFile.FileId, queue.Take().FileId);
    }

    [Fact]
    public void F03_018_top_x_failed_transfers_selects_oldest_non_completed_and_does_not_execute_transfering_update()
    {
        var location = new SourceLocationModel
        {
            SourceLocationId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
            ConfigurationId = 30,
            SourceProtocolCode = "UNC",
            SourceAddress = "x",
            FileMask = "*",
        };

        var older = new PendingFileModel
        {
            FileId = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
            ConfigId = 1,
            FileSourceLocationId = location.SourceLocationId,
            FileName = "older.txt",
            FileStatus = DataAccessConstants.FileStatusConstant.Error,
            FileTrackId = 1,
            StatusDate = DateTime.Today.AddDays(-1),
        };

        var newer = new PendingFileModel
        {
            FileId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"),
            ConfigId = 1,
            FileSourceLocationId = location.SourceLocationId,
            FileName = "newer.txt",
            FileStatus = DataAccessConstants.FileStatusConstant.Error,
            FileTrackId = 2,
            StatusDate = DateTime.Today,
        };

        var dataAccessFactory = new CapturingDataAccessFactory
        {
            PendingFiles = new List<PendingFileModel> { newer, older },
            SourceLocations = new List<SourceLocationModel> { location },
        };

        var fileTransferOperations = new CapturingFileTransferOperations(_ => new List<FileBasicInfo> { new("C:/a/older.txt", 0), new("C:/a/newer.txt", 0) });
        var producer = CreateProducer(dataAccessFactory, fileTransferOperations);

        var queue = producer.ProcessFailedTransfers(topXFailedTransfers: 1, specificSakFileTracking: null, specificConfigId: null, tenantId: TenantId);

        Assert.Equal(1, queue.Count);
        Assert.Equal(older.FileId, queue.Take().FileId);
        Assert.Equal(0, dataAccessFactory.UpdateFileStatusExecuteCount);
    }

    [Fact]
    public void F03_018_missing_source_file_records_error_and_does_not_requeue()
    {
        var location = new SourceLocationModel
        {
            SourceLocationId = Guid.Parse("12121212-1212-1212-1212-121212121212"),
            ConfigurationId = 40,
            SourceProtocolCode = "UNC",
            SourceAddress = "x",
            FileMask = "*",
        };

        var pending = new PendingFileModel
        {
            FileId = Guid.Parse("34343434-3434-3434-3434-343434343434"),
            ConfigId = 1,
            FileSourceLocationId = location.SourceLocationId,
            FileName = "missing.txt",
            FileStatus = DataAccessConstants.FileStatusConstant.Error,
            FileTrackId = 99,
            StatusDate = DateTime.Today.AddDays(-2),
        };

        var dataAccessFactory = new CapturingDataAccessFactory
        {
            PendingFiles = new List<PendingFileModel> { pending },
            SourceLocations = new List<SourceLocationModel> { location },
        };

        var fileTransferOperations = new CapturingFileTransferOperations(_ => new List<FileBasicInfo>());
        var producer = CreateProducer(dataAccessFactory, fileTransferOperations);

        var queue = producer.ProcessFailedTransfers(topXFailedTransfers: null, specificSakFileTracking: pending.FileId, specificConfigId: null, tenantId: TenantId);

        Assert.Equal(0, queue.Count);
        Assert.Equal(1, dataAccessFactory.ErrorLogExecuteCount);
        Assert.Equal(0, dataAccessFactory.UpdateFileStatusExecuteCount);
    }

    [Fact]
    public void F03_019_worker_aggregate_exceptions_are_logged_and_do_not_fail_batch_run()
    {
        var producer = new CapturingProducer();
        producer.QueueToReturn.Add(new FileToDeliverModel { FileName = "x" });

        var consumerFactory = new CapturingConsumerFactory(id =>
        {
            if (id == 0)
            {
                return new ThrowingConsumer(new InvalidOperationException("boom"));
            }

            return new NoOpConsumer();
        });

        var executor = CreateExecutor(consumerFactory: consumerFactory, producer: producer, maxConcurrentDeliveryProcesses: "2");

        var (result, output) = CaptureConsole(() => executor.Execute(new[] { TenantId }));

        Assert.Equal(0, result);
        Assert.Contains("boom", output);
    }

    [Fact]
    public void F03_022_sak_track_only_sets_guid_when_value_is_guid_and_numeric_commands_treat_non_int_as_zero()
    {
        var producer = new CapturingProducer();
        var executor = CreateExecutor(producer: producer, maxConcurrentDeliveryProcesses: "0");

        _ = executor.Execute(new[] { BatchJobConstants.CommandSakTrack, "12", TenantId });
        Assert.Equal(1, producer.AllLocationsRunCount);

        producer = new CapturingProducer();
        executor = CreateExecutor(producer: producer, maxConcurrentDeliveryProcesses: "0");
        var guid = Guid.Parse("99999999-9999-9999-9999-999999999999").ToString("D");
        _ = executor.Execute(new[] { BatchJobConstants.CommandConfigId, guid, TenantId });

        Assert.Equal(1, producer.FailedTransfersRunCount);
        Assert.Equal(0, producer.LastSpecificConfigId);
    }

    [Fact]
    public void F03_024_location_scanning_failure_suspends_location_and_sends_notification_when_enabled()
    {
        var location = new SourceLocationModel
        {
            SourceLocationId = Guid.Parse("abababab-abab-abab-abab-abababababab"),
            ConfigurationId = 50,
            SourceProtocolCode = "FTP",
            SourceAddress = "x",
            FileMask = "*",
            IsActive = true,
            IsSuspended = false,
            FtpServerDetail = new FtpServerModel { FtpServerIp = "1.2.3.4", FtpUser = "user" },
        };

        var dataAccessFactory = new CapturingDataAccessFactory
        {
            PendingFiles = new List<PendingFileModel>(),
            SourceLocations = new List<SourceLocationModel> { location },
        };

        var fileTransferOperations = new CapturingFileTransferOperations(_ => throw new Exception("no access"));
        var producer = CreateProducer(
            dataAccessFactory,
            fileTransferOperations,
            overrides: s => s[AppSettingsConstants.DisableNotifications] = "False");

        _ = producer.ProcessAllFilesSources(TenantId);

        Assert.Equal(1, dataAccessFactory.SuspensionExecuteCount);
        Assert.Equal(1, fileTransferOperations.SendNotificationCount);
    }

    [Fact]
    public void F03_026_previous_configuration_carries_forward_sak_file_track_prefix()
    {
        var location = new SourceLocationModel
        {
            SourceLocationId = Guid.Parse("cdcdcdcd-cdcd-cdcd-cdcd-cdcdcdcdcdcd"),
            ConfigurationId = 60,
            PreviousConfigurationId = 1,
            SourceProtocolCode = "UNC",
            SourceAddress = "x",
            FileMask = "*",
            IsActive = true,
            IsSuspended = false,
        };

        var dataAccessFactory = new CapturingDataAccessFactory
        {
            PendingFiles = new List<PendingFileModel>(),
            SourceLocations = new List<SourceLocationModel> { location },
        };

        var fileTransferOperations = new CapturingFileTransferOperations(_ => new List<FileBasicInfo> { new("123_abc.txt", 0) });
        var producer = CreateProducer(dataAccessFactory, fileTransferOperations);

        var queue = producer.ProcessAllFilesSources(TenantId);
        var queued = queue.Take();

        Assert.Equal(123, queued.SakFileTrack);
    }

    private static FileTransferBatchExecutor CreateExecutor(
        IConsumerFactory? consumerFactory = null,
        CapturingProducer? producer = null,
        string? maxConcurrentDeliveryProcesses = null,
        Action<Dictionary<string, string?>>? overrides = null)
    {
        consumerFactory ??= new CapturingConsumerFactory(_ => new NoOpConsumer());
        producer ??= new CapturingProducer();
        var settings = CreateAllMandatorySettings();

        if (maxConcurrentDeliveryProcesses != null)
        {
            settings[AppSettingsConstants.MaxConcurrentDeliveryProcesses] = maxConcurrentDeliveryProcesses;
        }

        overrides?.Invoke(settings);

        return new FileTransferBatchExecutor(consumerFactory, producer, new DictionaryAppSettings(settings));
    }

    private static FTSProducer CreateProducer(
        CapturingDataAccessFactory dataAccessFactory,
        CapturingFileTransferOperations fileTransferOperations,
        Action<Dictionary<string, string?>>? overrides = null)
    {
        var settings = CreateAllMandatorySettings();
        overrides?.Invoke(settings);
        return new FTSProducer(dataAccessFactory, fileTransferOperations, new DictionaryAppSettings(settings));
    }

    private static Dictionary<string, string?> CreateAllMandatorySettings() => new(StringComparer.Ordinal)
    {
        [AppSettingsConstants.WorkingDirectory] = "x",
        [AppSettingsConstants.MaxConcurrentDeliveryProcesses] = "0",
        [AppSettingsConstants.MaxFilesPerCycle] = "5",
        [AppSettingsConstants.MinsToWaitBeforeRetryFailedFile] = "5",
        [AppSettingsConstants.MinsToWaitBeforeRetrySuspendedConfig] = "5",
        [AppSettingsConstants.DisableNotifications] = "True",
        [AppSettingsConstants.EnvironmentName] = "DEV",
        [AppSettingsConstants.DeleteArchiveIfExists] = "False",
        [AppSettingsConstants.DuplicateFileExtension] = ".dup",
        [AppSettingsConstants.ActivateDuplicateRenameLogic] = "False",
    };

    private static (T Result, string Output) CaptureConsole<T>(Func<T> action)
    {
        var original = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);
        try
        {
            return (action(), writer.ToString());
        }
        finally
        {
            Console.SetOut(original);
        }
    }

    private sealed class CapturingProducer : IFTSProducer
    {
        public int AllLocationsRunCount { get; private set; }

        public int FailedTransfersRunCount { get; private set; }

        public int? LastSpecificConfigId { get; private set; }

        public BlockingCollection<FileToDeliverModel> QueueToReturn { get; } = new();

        public BlockingCollection<FileToDeliverModel> ProcessAllFilesSources(string tenantId)
        {
            AllLocationsRunCount++;
            return QueueToReturn;
        }

        public BlockingCollection<FileToDeliverModel> ProcessFailedTransfers(int? topXFailedTransfers, Guid? specificSakFileTracking, int? specificConfigId, string tenantId)
        {
            FailedTransfersRunCount++;
            LastSpecificConfigId = specificConfigId;
            return QueueToReturn;
        }
    }

    private sealed class CapturingConsumerFactory : IConsumerFactory
    {
        private readonly Func<int, IFTSConsumer> _create;

        public CapturingConsumerFactory(Func<int, IFTSConsumer> create)
        {
            _create = create;
        }

        public IFTSConsumer CreateConsumer(int consumerIdentifier) => _create(consumerIdentifier);
    }

    private sealed class NoOpConsumer : IFTSConsumer
    {
        public bool ProcessDeliveryQueue(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, string tenantId) => true;
    }

    private sealed class ThrowingConsumer : IFTSConsumer
    {
        private readonly Exception _exception;

        public ThrowingConsumer(Exception exception)
        {
            _exception = exception;
        }

        public bool ProcessDeliveryQueue(BlockingCollection<FileToDeliverModel> filesToDeliverQueue, string tenantId) => throw _exception;
    }

    private sealed class CapturingDataAccess<T> : IDataAccess<T>
    {
        private readonly Func<T> _result;

        public CapturingDataAccess(Func<T> result)
        {
            _result = result;
        }

        public int ExecuteCount { get; private set; }

        public T Execute()
        {
            ExecuteCount++;
            return _result();
        }
    }

    private sealed class CapturingDataAccessFactory : IDataAccessFactory
    {
        public List<PendingFileModel> PendingFiles { get; set; } = new();

        public List<SourceLocationModel> SourceLocations { get; set; } = new();

        public int UpdateSourceSuspendStatusExecuteCount { get; private set; }

        public int SuspensionExecuteCount { get; private set; }

        public int ErrorLogExecuteCount { get; private set; }

        public int UpdateFileStatusExecuteCount { get; private set; }

        public IDataAccess<List<PendingFileModel>> CreateGetPendingFileListDataAccess() => new CapturingDataAccess<List<PendingFileModel>>(() => PendingFiles);

        public IDataAccess<List<SourceLocationModel>> CreateGetSourceLocationsDataAccess() => new CapturingDataAccess<List<SourceLocationModel>>(() => SourceLocations);

        public IDataAccess<Guid> CreateUpdateSourceSuspendStatusDataAccess(Guid sourceLocationId)
        {
            return new CapturingDataAccess<Guid>(() =>
            {
                UpdateSourceSuspendStatusExecuteCount++;
                return sourceLocationId;
            });
        }

        public IDataAccess<int> CreateFtsSuspensionDataAccess(FtsSuspensionSearchCriteria criteria)
        {
            return new CapturingDataAccess<int>(() =>
            {
                SuspensionExecuteCount++;
                return 1;
            });
        }

        public IDataAccess<FileManagementResponseModel> CreateFileManagementDataAccess(FileManagementRequestModel request) => throw new NotSupportedException();

        public IDataAccess<int> CreateErrorLogDataAccess(ErrorLogDetailModel errorLog)
        {
            return new CapturingDataAccess<int>(() =>
            {
                ErrorLogExecuteCount++;
                return 1;
            });
        }

        public IDataAccess<SourceLocationModel>? CreateGetConfigMetaDataDataAccess(Guid fileId) => throw new NotSupportedException();

        public IDataAccess<Guid> CreateUpdateFileManagementStatusDataAccess(Guid fileId, string fileStatus)
        {
            return new CapturingDataAccess<Guid>(() =>
            {
                UpdateFileStatusExecuteCount++;
                return fileId;
            });
        }

        public IDataAccess<Guid> CreateFtsFileDeliveryDataAccess(Guid fileId, Guid destinationId, string destinationFileName, string operatorId, List<DestinationModel> destinations) => throw new NotSupportedException();

        public IDataAccess<Guid> CreateFtsFileExceptionEventDataAccess(Guid fileId, Guid destinationId, string destinationFileName, string operatorId, ErrorLogDetailModel errorLog) => throw new NotSupportedException();

        public IDataAccess<List<FtsNotificationModel>> CreateQueryFtsNotificationDataAccess(GetNotificationSearchCriteria searchQuery) => throw new NotSupportedException();
    }

    private sealed class CapturingFileTransferOperations : IFileTransferOperations
    {
        private readonly Func<int, List<FileBasicInfo>> _getList;

        public CapturingFileTransferOperations(Func<int, List<FileBasicInfo>> getList)
        {
            _getList = getList;
        }

        public int SendNotificationCount { get; private set; }

        public bool DownloadFile(SourceLocationModel source, string sourceFileName, DestinationModel destination) => throw new NotSupportedException();

        public bool DownloadFile(SourceLocationModel source, string sourceFileName, DestinationModel destination, string tenantId) => throw new NotSupportedException();

        public string UploadFile(string fullyQualifiedSourceFileName, string fullyQualifiedDestinationFileName, DestinationModel destinationLocation) => throw new NotSupportedException();

        public bool DeleteFile(SourceLocationModel source, string fullyQualifedSourceFile) => throw new NotSupportedException();

        public bool RenameSourceFile(SourceLocationModel source, string fullyQualifedSourceFile, string renameExtension) => throw new NotSupportedException();

        public List<FileBasicInfo> GetListOfFileNamesFromSource(SourceLocationModel location, int maxFilesPerCycle) => _getList(maxFilesPerCycle);

        public string GetDestinationFileName(string fullyQualifiedSourceFileName, DestinationModel destinationLocation, Guid fileId, string crc32, string uploadedSFTPUserName) => throw new NotSupportedException();

        public string CreateZipFile(string inputFileName) => throw new NotSupportedException();

        public void SendNotification(long configId, string subject, string body, bool isErrorNotification) => SendNotificationCount++;

        public void SendNotification(SourceLocationModel source, string subject, string body, bool isErrorNotification, string tenantId) => SendNotificationCount++;

        public void PostEvent(string eventName, string subSystem, string messageBody, bool isArchived, string operatorId) => throw new NotSupportedException();

        public string GetCrc32(string filePath) => throw new NotSupportedException();
    }
}


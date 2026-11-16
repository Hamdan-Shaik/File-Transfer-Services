using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using Fxfer.ExecutionWorker.Application.Contracts.Domain;
using Fxfer.ExecutionWorker.Application.Helpers;
using Fxfer.ExecutionWorker.Batch.Business;
using Fxfer.ExecutionWorker.Batch.Business.DataAccess;
using Fxfer.ExecutionWorker.Domain.Batch.Business;
using Fxfer.ExecutionWorker.Domain.Clients;
using Fxfer.ExecutionWorker.Domain.Constants;
using Fxfer.ExecutionWorker.Domain.Exceptions;
using Fxfer.ExecutionWorker.Infrastructure.Tenancy;
using Xunit;

namespace Fxfer.ExecutionWorker.UnitTests;

public sealed class FtsConsumerRulesTests
{
    [Fact]
    public void F03_020_processing_a_queued_file_delivers_updates_tracking_and_may_notify()
    {
        var settings = new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            [AppSettingsConstants.WorkingDirectory] = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")),
            [AppSettingsConstants.EnvironmentName] = "ENV",
            [AppSettingsConstants.HostName] = "https://host",
        };

        var appSettings = new DictionaryAppSettings(settings);

        var transferOps = new CapturingFileTransferOperations();
        var dataAccess = new CapturingDataAccessFactory
        {
            ConfigMetaData = CreateSourceLocationWithOneDestination(),
        };

        var consumer = new FTSConsumer(
            transferOps,
            dataAccess,
            appSettings,
            new TenantContext { TenantId = "tenant" },
            new EventHelper(new NoopDistributedEventStorage()),
            new NullQueryProvider(),
            new NullServiceProvider());

        var queue = new BlockingCollection<FileToDeliverModel>();
        queue.Add(new FileToDeliverModel
        {
            FileId = Guid.Empty,
            FileName = "C:\\in\\file.txt",
            SakFileTrack = 123,
            FileSize = 10,
            GroupId = Guid.Empty,
            SourceLocation = new SourceLocationModel
            {
                ConfigurationId = 1,
                SourceLocationId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                SendNotificationsIndicator = true,
                FtpServerDetail = new FtpServerModel { FtpServerIp = "ip", FtpUser = "user", FtpPassword = "pw", FtpServerName = "name", FtpTransferMode = "m" },
                SourceAddress = "C:\\src",
                SourceProtocolCode = ProtocolCodeConstant.UNC,
                FileMask = "*",
            },
        });

        Assert.True(consumer.ProcessDeliveryQueue(queue, "tenant"));

        Assert.Equal(DataAccessConstants.FileStatusConstant.Completed, dataAccess.LastStatus);
        Assert.NotNull(dataAccess.LastFtsDelivery);
        Assert.True(transferOps.DeleteFileCalled);
        Assert.NotNull(transferOps.LastSendNotification);
        Assert.NotNull(transferOps.LastPostEvent);
        Assert.Contains("File Delivery Success", transferOps.LastPostEvent!.Value.EventName);
    }

    [Fact]
    public void F03_029_duplicate_name_when_rename_enabled_renames_source_and_marks_completed()
    {
        var settings = new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            [AppSettingsConstants.WorkingDirectory] = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")),
            [AppSettingsConstants.EnvironmentName] = "ENV",
            [AppSettingsConstants.ActivateDuplicateRenameLogic] = "true",
            [AppSettingsConstants.DuplicateFileExtension] = ".dup",
        };

        var transferOps = new CapturingFileTransferOperations
        {
            Crc32ExceptionToThrow = new DuplicateFileNameException("dup"),
        };

        var dataAccess = new CapturingDataAccessFactory
        {
            ConfigMetaData = CreateSourceLocationWithOneDestination(),
        };

        var consumer = new FTSConsumer(
            transferOps,
            dataAccess,
            new DictionaryAppSettings(settings),
            new TenantContext { TenantId = "tenant" },
            new EventHelper(new NoopDistributedEventStorage()),
            new NullQueryProvider(),
            new NullServiceProvider());

        var queue = new BlockingCollection<FileToDeliverModel>();
        queue.Add(new FileToDeliverModel
        {
            FileId = Guid.Empty,
            FileName = "C:\\in\\file.txt",
            SakFileTrack = 999,
            FileSize = 10,
            GroupId = Guid.Empty,
            SourceLocation = new SourceLocationModel
            {
                ConfigurationId = 1,
                SourceLocationId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                SendNotificationsIndicator = true,
                FtpServerDetail = new FtpServerModel { FtpServerIp = "ip", FtpUser = "user", FtpPassword = "pw", FtpServerName = "name", FtpTransferMode = "m" },
                SourceAddress = "C:\\src",
                SourceProtocolCode = ProtocolCodeConstant.UNC,
                FileMask = "*",
            },
        });

        Assert.True(consumer.ProcessDeliveryQueue(queue, "tenant"));

        Assert.True(transferOps.RenameSourceCalled);
        Assert.Equal(".dup", transferOps.LastRenameExtension);
        Assert.Equal(DataAccessConstants.FileStatusConstant.Completed, dataAccess.LastStatus);
    }

    [Fact]
    public void F03_030_deletes_source_only_when_every_destination_is_fully_uploaded()
    {
        var settings = new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            [AppSettingsConstants.WorkingDirectory] = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")),
            [AppSettingsConstants.EnvironmentName] = "ENV",
        };

        var transferOps = new CapturingFileTransferOperations
        {
            UploadReturnValueByDestinationId = new Dictionary<Guid, string?>
            {
                [Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")] = "ok",
                [Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb")] = string.Empty,
            },
        };

        var dataAccess = new CapturingDataAccessFactory
        {
            ConfigMetaData = CreateSourceLocationWithTwoDestinations(),
        };

        var consumer = new FTSConsumer(
            transferOps,
            dataAccess,
            new DictionaryAppSettings(settings),
            new TenantContext { TenantId = "tenant" },
            new EventHelper(new NoopDistributedEventStorage()),
            new NullQueryProvider(),
            new NullServiceProvider());

        var queue = new BlockingCollection<FileToDeliverModel>();
        queue.Add(new FileToDeliverModel
        {
            FileId = Guid.Empty,
            FileName = "C:\\in\\file.txt",
            SakFileTrack = 123,
            FileSize = 10,
            GroupId = Guid.Empty,
            SourceLocation = new SourceLocationModel
            {
                ConfigurationId = 1,
                SourceLocationId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                SendNotificationsIndicator = true,
                FtpServerDetail = new FtpServerModel { FtpServerIp = "ip", FtpUser = "user", FtpPassword = "pw", FtpServerName = "name", FtpTransferMode = "m" },
                SourceAddress = "C:\\src",
                SourceProtocolCode = ProtocolCodeConstant.UNC,
                FileMask = "*",
            },
        });

        Assert.True(consumer.ProcessDeliveryQueue(queue, "tenant"));

        Assert.False(transferOps.DeleteFileCalled);
        Assert.Equal(DataAccessConstants.FileStatusConstant.Completed, dataAccess.LastStatus);
    }

    private static SourceLocationModel CreateSourceLocationWithOneDestination()
    {
        return new SourceLocationModel
        {
            ConfigurationId = 1,
            SourceLocationId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            OwnerEmail = "owner@example.com",
            OwnerName = "Owner",
            SendNotificationsIndicator = true,
            SourceProtocolCode = ProtocolCodeConstant.UNC,
            SourceAddress = "C:\\src",
            FtpServerDetail = new FtpServerModel { FtpServerIp = "ip", FtpUser = "user", FtpPassword = "pw", FtpServerName = "name", FtpTransferMode = "m" },
            Destinations = new List<DestinationModel>
            {
                new DestinationModel
                {
                    DestinationId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    DestinationAddress = "C:\\dest",
                    DestinationProtocolCode = "UNC",
                    DestinationFileName = "C:\\dest\\file.txt",
                    EncryptionCode = string.Empty,
                    EmailId = "",
                    ContactName = "",
                },
            },
            FileMask = "*",
        };
    }

    private static SourceLocationModel CreateSourceLocationWithTwoDestinations()
    {
        var source = CreateSourceLocationWithOneDestination();
        source.Destinations.Add(new DestinationModel
        {
            DestinationId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            DestinationAddress = "C:\\dest2",
            DestinationProtocolCode = "UNC",
            DestinationFileName = "C:\\dest2\\file.txt",
            EncryptionCode = string.Empty,
            EmailId = "",
            ContactName = "",
        });
        return source;
    }

    private sealed class NoopDistributedEventStorage : IDistributedEventStorage
    {
        public void Add(EventMessage message)
        {
        }
    }

    private sealed class NullQueryProvider : IFileTransferSvcQueryServiceProvider
    {
        public FtsIntegrationFileModel? GetIntegrationFile(GetIntegrationFileQuery query) => null;
    }

    private sealed class NullServiceProvider : IFileTransferSvcServiceProvider
    {
        public IntegrationFileUpdated UpdateIntegrationFile(UpdateIntegrationFileCommand command) => new();
    }

    private sealed class CapturingFileTransferOperations : IFileTransferOperations
    {
        public bool DeleteFileCalled { get; private set; }

        public bool RenameSourceCalled { get; private set; }

        public string? LastRenameExtension { get; private set; }

        public (SourceLocationModel Source, string Subject, string Body, bool IsError, string TenantId)? LastSendNotification { get; private set; }

        public (string EventName, string SubSystem, string Body, bool Archived, string OperatorId)? LastPostEvent { get; private set; }

        public Exception? Crc32ExceptionToThrow { get; init; }

        public Exception? UploadExceptionToThrow { get; init; }

        public Dictionary<Guid, string?> UploadReturnValueByDestinationId { get; init; } = new();

        public bool DownloadFile(SourceLocationModel source, string sourceFileName, DestinationModel destination) => true;

        public bool DownloadFile(SourceLocationModel source, string sourceFileName, DestinationModel destination, string tenantId) => true;

        public string UploadFile(string fullyQualifiedSourceFileName, string fullyQualifiedDestinationFileName, DestinationModel destinationLocation)
        {
            if (UploadExceptionToThrow is not null)
            {
                throw UploadExceptionToThrow;
            }

            if (UploadReturnValueByDestinationId.TryGetValue(destinationLocation.DestinationId, out var val))
            {
                return val ?? string.Empty;
            }

            return fullyQualifiedDestinationFileName;
        }

        public bool DeleteFile(SourceLocationModel source, string fullyQualifedSourceFile)
        {
            DeleteFileCalled = true;
            return true;
        }

        public bool RenameSourceFile(SourceLocationModel source, string fullyQualifedSourceFile, string renameExtension)
        {
            RenameSourceCalled = true;
            LastRenameExtension = renameExtension;
            return true;
        }

        public List<FileBasicInfo> GetListOfFileNamesFromSource(SourceLocationModel location, int maxFilesPerCycle) => new();

        public string GetDestinationFileName(string fullyQualifiedSourceFileName, DestinationModel destinationLocation, Guid fileId, string crc32, string uploadedSFTPUserName)
            => destinationLocation.DestinationFileName;

        public string CreateZipFile(string inputFileName) => inputFileName + ".zip";

        public void SendNotification(long configId, string subject, string body, bool isErrorNotification)
        {
        }

        public void SendNotification(SourceLocationModel source, string subject, string body, bool isErrorNotification, string tenantId)
        {
            LastSendNotification = (source, subject, body, isErrorNotification, tenantId);
        }

        public void PostEvent(string eventName, string subSystem, string messageBody, bool isArchived, string operatorId)
        {
            LastPostEvent = (eventName, subSystem, messageBody, isArchived, operatorId);
        }

        public string GetCrc32(string filePath)
        {
            if (Crc32ExceptionToThrow is not null)
            {
                throw Crc32ExceptionToThrow;
            }

            return "crc";
        }
    }

    private sealed class CapturingDataAccessFactory : IDataAccessFactory
    {
        public SourceLocationModel? ConfigMetaData { get; init; }

        public string? LastStatus { get; private set; }

        public (Guid FileId, Guid DestinationId, string DestinationFileName, string OperatorId, int DestinationCount)? LastFtsDelivery { get; private set; }

        public (Guid FileId, Guid DestinationId, string DestinationFileName, string OperatorId)? LastExceptionEvent { get; private set; }

        public IDataAccess<List<PendingFileModel>> CreateGetPendingFileListDataAccess() => throw new NotSupportedException();

        public IDataAccess<List<SourceLocationModel>> CreateGetSourceLocationsDataAccess() => throw new NotSupportedException();

        public IDataAccess<FileManagementResponseModel> CreateFileManagementDataAccess(FileManagementRequestModel request)
        {
            return new ReturningDataAccess<FileManagementResponseModel>(new FileManagementResponseModel
            {
                FileId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                FileTrackId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            });
        }

        public IDataAccess<int> CreateErrorLogDataAccess(ErrorLogDetailModel errorLog) => new ReturningDataAccess<int>(1);

        public IDataAccess<SourceLocationModel>? CreateGetConfigMetaDataDataAccess(Guid fileId)
        {
            return ConfigMetaData is null ? null : new ReturningDataAccess<SourceLocationModel>(ConfigMetaData);
        }

        public IDataAccess<Guid> CreateUpdateFileManagementStatusDataAccess(Guid fileId, string fileStatus)
        {
            LastStatus = fileStatus;
            return new ReturningDataAccess<Guid>(fileId);
        }

        public IDataAccess<Guid> CreateUpdateSourceSuspendStatusDataAccess(Guid sourceLocationId) => throw new NotSupportedException();

        public IDataAccess<int> CreateFtsSuspensionDataAccess(FtsSuspensionSearchCriteria criteria) => throw new NotSupportedException();

        public IDataAccess<Guid> CreateFtsFileDeliveryDataAccess(Guid fileId, Guid destinationId, string destinationFileName, string operatorId, List<DestinationModel> destinations)
        {
            LastFtsDelivery = (fileId, destinationId, destinationFileName, operatorId, destinations.Count);
            return new ReturningDataAccess<Guid>(Guid.NewGuid());
        }

        public IDataAccess<Guid> CreateFtsFileExceptionEventDataAccess(Guid fileId, Guid destinationId, string destinationFileName, string operatorId, ErrorLogDetailModel errorLog)
        {
            LastExceptionEvent = (fileId, destinationId, destinationFileName, operatorId);
            return new ReturningDataAccess<Guid>(Guid.NewGuid());
        }

        public IDataAccess<List<FtsNotificationModel>> CreateQueryFtsNotificationDataAccess(GetNotificationSearchCriteria searchQuery) => throw new NotSupportedException();
    }

    private sealed class ReturningDataAccess<T> : IDataAccess<T>
    {
        private readonly T _value;

        public ReturningDataAccess(T value)
        {
            _value = value;
        }

        public T Execute() => _value;
    }
}


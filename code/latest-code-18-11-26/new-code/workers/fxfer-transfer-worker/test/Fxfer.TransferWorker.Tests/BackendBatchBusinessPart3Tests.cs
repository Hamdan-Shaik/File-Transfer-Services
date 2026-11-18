using Fxfer.TransferWorker.Application.Batch;
using Fxfer.TransferWorker.Application.Constants;
using Fxfer.TransferWorker.Application.DataAccess;
using Fxfer.TransferWorker.Contracts.Batch;
using Fxfer.TransferWorker.Contracts.NotificationService;
using Fxfer.TransferWorker.Domain.Batch;
using Fxfer.TransferWorker.Worker.Configuration;

using Microsoft.Extensions.Logging;

namespace Fxfer.TransferWorker.Tests;

public sealed class BackendBatchBusinessPart3Tests
{
    [Fact]
    public void FileTransferOperations_GetDestinationFileName_builds_datedir_and_renamed_filename_for_unc()
    {
        var temp = CreateTempDirectory();
        try
        {
            var input = Path.Combine(temp, "input.txt");
            File.WriteAllText(input, "abc");

            var fileId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var crc32 = "DEADBEEF";

            var destination = new DestinationModel
            {
                DestinationProtocolCode = ProtocolCodeConstant.UNC,
                DestinationAddress = Path.Combine(temp, "dest"),
                IsRenameAtDestination = true,
                RenamePattern = "<DATEDIR>2"
            };

            var baseAddress = destination.DestinationAddress;

            var ops = CreateOperations(new FakeProtocols());

            var actual = ops.GetDestinationFileName(input, destination, fileId, crc32, uploadedSFTPUserName: "");

            var expectedDir = Path.Combine(baseAddress, DateTime.Today.Year.ToString(), DateTime.Today.Month.ToString(), DateTime.Today.Day.ToString());
            Assert.StartsWith(expectedDir + "\\", actual);
            Assert.EndsWith(fileId + "_" + crc32 + "_" + Path.GetFileName(input), actual);
        }
        finally
        {
            SafeDeleteDirectory(temp);
        }
    }

    [Fact]
    public void FileTransferOperations_CreateZipFile_returns_existing_zip_without_recreating()
    {
        var temp = CreateTempDirectory();
        try
        {
            var protocols = new FakeProtocols();
            var ops = CreateOperations(protocols);

            var input = Path.Combine(temp, "input.txt");
            File.WriteAllText(input, "hello");

            var zip1 = ops.CreateZipFile(input);
            Assert.True(File.Exists(zip1));
            Assert.Equal(1, protocols.CreateZipCallCount);

            var zip2 = ops.CreateZipFile(input);
            Assert.Equal(zip1, zip2);
            Assert.Equal(1, protocols.CreateZipCallCount);

            var alreadyZip = Path.Combine(temp, "already.zip");
            File.WriteAllText(alreadyZip, "zip");
            var zip3 = ops.CreateZipFile(alreadyZip);
            Assert.Equal(alreadyZip, zip3);
            Assert.Equal(1, protocols.CreateZipCallCount);
        }
        finally
        {
            SafeDeleteDirectory(temp);
        }
    }

    [Fact]
    public void FileTransferOperations_GetCrc32_delegates_to_protocols()
    {
        var protocols = new FakeProtocols { Crc32ToReturn = "ABC" };
        var ops = CreateOperations(protocols);

        var actual = ops.GetCrc32("any.txt");

        Assert.Equal("ABC", actual);
    }

    private static FileTransferOperations CreateOperations(IFileTransferProtocols protocols)
    {
        var loggerFactory = LoggerFactory.Create(builder => { });

        return new FileTransferOperations(
            protocols,
            new StubNotificationService(),
            new StubTenantAppSettingsManager(),
            loggerFactory.CreateLogger<FileTransferOperations>());
    }

    private sealed class FakeProtocols : IFileTransferProtocols
    {
        public int CreateZipCallCount { get; private set; }

        public string Crc32ToReturn { get; set; } = string.Empty;

        public void DownloadFromUNCConnection(string sourceFileName, DestinationModel destination) => throw new NotImplementedException();

        public void DownloadFromFtpConnection(EnterpriseDT.Net.Ftp.FTPConnection remoteServer, SourceLocationModel sourceLocation, string remoteFileName, DestinationModel destinationLocation, string tenantId)
            => throw new NotImplementedException();

        public void DownloadFromSSHFtpConnection(Renci.SshNet.SftpClient remoteServer, SourceLocationModel sourceLocation, string remoteFileName, DestinationModel destinationLocation, string tenantId)
            => throw new NotImplementedException();

        public string? UploadToUnc(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation)
            => throw new NotImplementedException();

        public bool UploadToFTPServer(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation, EnterpriseDT.Net.Ftp.FTPConnection remoteServer)
            => throw new NotImplementedException();

        public void UploadToSSHFTPServer(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation, Renci.SshNet.SftpClient remoteServer)
            => throw new NotImplementedException();

        public bool DeleteFromUNC(string fullyQualifiedSourceFileName) => throw new NotImplementedException();

        public bool DeleteFromFtp(EnterpriseDT.Net.Ftp.FTPConnection remoteServer, string fileName) => throw new NotImplementedException();

        public bool DeleteFromSFtp(Renci.SshNet.SftpClient remoteServer, string fileName) => throw new NotImplementedException();

        public bool RenameFileFromUNC(string fullyQualifiedSourceFileName, string renameExtension) => throw new NotImplementedException();

        public bool RenameFtpFile(string fullyQualifiedSourceFileName, string renameExtension, EnterpriseDT.Net.Ftp.FTPConnection remoteServer)
            => throw new NotImplementedException();

        public bool RenameSSHFtpFile(string fullyQualifiedSourceFileName, string renameExtension, Renci.SshNet.SftpClient remoteServer)
            => throw new NotImplementedException();

        public List<FileBasicInfo> GetFileListFromUncSource(SourceLocationModel location) => throw new NotImplementedException();

        public List<FileBasicInfo> GetFilesFromSource(EnterpriseDT.Net.Ftp.FTPConnection remoteServer, SourceLocationModel location) => throw new NotImplementedException();

        public List<FileBasicInfo> GetFilesFromSSHSource(Renci.SshNet.SftpClient remoteServer, SourceLocationModel location) => throw new NotImplementedException();

        public void CreateZipFile(string inputFileName, string zipFileName, FileInfo fileInfo)
        {
            CreateZipCallCount++;
            using var fs = new FileStream(zipFileName, FileMode.Create, FileAccess.Write);
            using var archive = new System.IO.Compression.ZipArchive(fs, System.IO.Compression.ZipArchiveMode.Create);
            var entry = archive.CreateEntry(fileInfo.Name);
            using var input = File.OpenRead(inputFileName);
            using var output = entry.Open();
            input.CopyTo(output);
        }

        public string GetCrc32(string filePath) => Crc32ToReturn;
    }

    private sealed class StubDataAccessFactory : IDataAccessFactory
    {
        public IDataAccess<FtsSuspensionModel> CreateFtsSuspensionDataAccess(FtsSuspensionSearchCriteria searchQuery) => throw new NotImplementedException();
        public IDataAccess<int> CreateErrorLogDataAccess(ErrorLogDetailModel errorLog) => throw new NotImplementedException();
        public IDataAccess<Guid> CreateFtsFileDeliveryDataAccess(Guid fileId, Guid destinationId, string destinationFileName, string operatorId) => throw new NotImplementedException();
        public IDataAccess<Guid> CreateFtsFileExceptionEventDataAccess(Guid fileId, Guid destinationId, string destinationFileName, string operatorId, ErrorLogDetailModel errorLog) => throw new NotImplementedException();
        public IDataAccess<FileManagementResponseModel> CreateFileManagementDataAccess(FileManagementRequestModel requestModel) => throw new NotImplementedException();
        public IDataAccess<SourceLocationModel?> CreateGetConfigMetaDataDataAccess(Guid fileId) => throw new NotImplementedException();
        public IDataAccess<List<PendingFileModel>> CreateGetPendingFileListDataAccess() => throw new NotImplementedException();
        public IDataAccess<List<SourceLocationModel>> CreateGetSourceLocationsDataAccess() => throw new NotImplementedException();
        public IDataAccess<List<FtsNotificationModel>> CreateQueryFtsNotificationDataAccess(GetNotificationSearchCriteria searchQuery) => new StubDataAccess<List<FtsNotificationModel>>([]);
        public IDataAccess<FtsIntegrationFileModel?> CreateGetIntegrationFileDataAccess(string fileName) => throw new NotImplementedException();
        public IDataAccess<bool> CreateUpdateIntegrationFileDataAccess(UpdateIntegrationFileCommand command) => throw new NotImplementedException();
        public IDataAccess<Guid> CreateUpdateFileManagementStatusDataAccess(Guid fileId, string fileStatus) => throw new NotImplementedException();
        public IDataAccess<bool> CreateUpdateSourceSuspendStatusDataAccess(Guid sourceLocationId) => throw new NotImplementedException();
        public IDataAccess<SourceLocationModel?> GetSuspendedLocation(Guid sourceLocationId) => throw new NotImplementedException();
    }

    private sealed class StubDataAccess<T> : IDataAccess<T>
    {
        private readonly T _value;

        public StubDataAccess(T value) => _value = value;

        public Task<T> ExecuteAsync(CancellationToken cancellationToken) => Task.FromResult(_value);
    }

    private sealed class StubNotificationService : INotificationService
    {
        public PostEvent PostEvent(string eventName, string subSystem, string messageBody, bool isArchived, DateTime createdDateTimeUtc, Guid? lastEventId, string operatorId, string tenantId)
            => new PostEvent { EventId = Guid.NewGuid() };

        public void SendEmail(string tenantId, string subject, string emailBody, List<Address> addressList, Address fromAddress, MailPriority priority, List<Attachments> attachmentList)
        {
        }
    }

    private sealed class StubRequestContextAccessor : IRequestContextAccessor
    {
        public string? CorrelationId => null;
        public string? TenantId => "test";
    }

    private sealed class StubTenantAppSettingsManager : ITenantAppSettingsManager
    {
        public ITenantAppSettings? AppSettings => null;

        public void LoadApplicationConfiguration(string tenantId)
        {
        }
    }

    private static string CreateTempDirectory()
    {
        var directory = Path.Combine(Path.GetTempPath(), "fxfer-tests-" + Guid.NewGuid().ToString("n"));
        Directory.CreateDirectory(directory);
        return directory;
    }

    private static void SafeDeleteDirectory(string directory)
    {
        try
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, recursive: true);
            }
        }
        catch
        {
        }
    }
}


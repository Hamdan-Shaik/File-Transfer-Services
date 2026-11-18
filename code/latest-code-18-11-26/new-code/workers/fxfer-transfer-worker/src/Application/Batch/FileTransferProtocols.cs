using System.IO.Compression;
using System.Linq;

using EnterpriseDT.Net.Ftp;

using Fxfer.TransferWorker.Application.DataAccess;
using Fxfer.TransferWorker.Application.Constants;
using Fxfer.TransferWorker.Contracts.Batch;
using Fxfer.TransferWorker.Contracts.NotificationService;
using Fxfer.TransferWorker.Domain.Batch;
using Fxfer.TransferWorker.Domain.Constants;
using Fxfer.TransferWorker.Domain.Exceptions;
using Fxfer.TransferWorker.Worker.Configuration;

using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace Fxfer.TransferWorker.Application.Batch;

public sealed class FileTransferProtocols : IFileTransferProtocols
{
    private const string UploadExtension = ".ftsUpload";

    private static List<long>? badConfigs;
    private readonly bool deleteArchiveIfExists;
    private readonly IDataAccessFactory dataAcessData;
    private readonly IPatternMatcher patternMatcher;
    private readonly INotificationService notificationService;
    private readonly IRequestContextAccessor requestContextAccessor;

    public FileTransferProtocols(
        IDataAccessFactory dataAccessFactory,
        IPatternMatcher patternMatcher,
        ITenantAppSettingsManager configurationManager,
        INotificationService notificationService,
        IRequestContextAccessor requestContextAccessor)
    {
        this.dataAcessData = dataAccessFactory;
        this.patternMatcher = patternMatcher;
        this.notificationService = notificationService;
        this.requestContextAccessor = requestContextAccessor;
        this.deleteArchiveIfExists = Convert.ToBoolean(configurationManager.AppSettings?[AppSettingsConstants.DeleteArchiveIfExists]);
    }

    public static List<long> BadConfigs
    {
        get
        {
            badConfigs ??= new List<long>(0);

            return badConfigs;
        }

        set
        {
            badConfigs = value;
        }
    }

    #region Download Methods
    public void DownloadFromUNCConnection(string sourceFileName, DestinationModel destination)
    {
        FileInfo file = new FileInfo(sourceFileName);
        File.Copy(file.FullName, destination.DestinationFileName);
    }

    public void DownloadFromFtpConnection(FTPConnection remoteServer, SourceLocationModel sourceLocation, string remoteFileName, DestinationModel destinationLocation, string tenantId)
    {
        string fullyQualifiedRemoteFileName = sourceLocation.SourceAddress + "/" + remoteFileName;

        try
        {
            remoteServer.Connect();
            remoteServer.DownloadFile(destinationLocation.DestinationFileName, remoteFileName);
        }
        catch (Exception ex)
        {
            FTSConsumer.Log(ex.Message);

            if (BadConfigs != null && !BadConfigs.Contains(sourceLocation.ConfigurationId))
            {
                BadConfigs.Add(sourceLocation.ConfigurationId);
                this.SendNotificationOfBadConfig(sourceLocation, tenantId);
            }
        }

        remoteServer.Close();
    }

    public void DownloadFromSSHFtpConnection(SftpClient remoteServer, SourceLocationModel sourceLocation, string remoteFileName, DestinationModel destinationLocation, string tenantId)
    {
        string sourceFullFilePath;

        BadConfigs = BadConfigs ?? new List<long>(0);

        sourceFullFilePath = sourceLocation.SourceProtocolCode == ProtocolCodeConstant.UNC
            ? sourceLocation.SourceAddress + "\\" + remoteFileName
            : sourceLocation.SourceAddress + "/" + remoteFileName;

        FTSConsumer.Log(string.Format(FileTransferManagementConstants.Information.FullFilePath, sourceFullFilePath));

        using (SftpClient sftpClient = remoteServer)
        {
            this.AttempToDownloadFileWithSSHFtp(sourceLocation, destinationLocation, tenantId, sourceFullFilePath, sftpClient);

            sftpClient.Disconnect();
        }
    }
    #endregion

    #region Upload Methods
    public string UploadToUnc(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation)
    {
        this.IsAValidDestination(destinationLocation.DestinationAddress);
        if (!Directory.Exists(destinationLocation.DestinationAddress))
        {
            Directory.CreateDirectory(destinationLocation.DestinationAddress);
        }

        string uploadFileName = fullyQualifiedDestinationFile + UploadExtension;
        File.Copy(fullyQualifiedSourceFile, uploadFileName, true);

        if (File.Exists(fullyQualifiedDestinationFile))
        {
            if (!destinationLocation.IsOverwriteAtDestination)
            {
                if (destinationLocation.IsArchive && this.deleteArchiveIfExists == true)
                {
                    File.Delete(fullyQualifiedDestinationFile);
                }
                else
                {
                    throw new DuplicateFileNameException(FileTransferManagementConstants.ExceptionMessages.FileExixstAtDestination);
                }
            }
            else
            {
                File.Delete(fullyQualifiedDestinationFile);
            }
        }

        File.Move(uploadFileName, fullyQualifiedDestinationFile);

        return File.Exists(fullyQualifiedDestinationFile) ? fullyQualifiedDestinationFile : null;
    }

    public bool UploadToFTPServer(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation, FTPConnection remoteServer)
    {
        string uploadFileName = fullyQualifiedDestinationFile + UploadExtension;

        bool isFullyUploaded = false;
        remoteServer.Connect();

        FTSConsumer.Log(FileTransferManagementConstants.Information.SFTPConnectionEstablished);

        if (remoteServer.DirectoryExists(destinationLocation.DestinationAddress) == false)
        {
            this.CreateRemoteDirectoryIfNotExists(remoteServer, destinationLocation.DestinationAddress);
        }

        this.CheckAndHandleFileOverwrite(remoteServer, fullyQualifiedDestinationFile, destinationLocation);

        this.RemoveTemporaryUploadFile(remoteServer, uploadFileName);
        try
        {
            if (!this.TryUploadFileToSFTP(remoteServer, fullyQualifiedSourceFile, uploadFileName, fullyQualifiedDestinationFile, destinationLocation))
            {
                return false;
            }

            isFullyUploaded = this.RenameUploadedFile(remoteServer, uploadFileName, fullyQualifiedDestinationFile);
        }
        catch (Exception ex)
        {
            isFullyUploaded = false;
            FTSConsumer.Log(string.Format(FileTransferManagementConstants.ExceptionMessages.AutoCreateDirectoryFailed, fullyQualifiedDestinationFile));
        }
        finally
        {
            this.CloseRemoteServerConnection(remoteServer);
        }

        return isFullyUploaded;
    }

    public void UploadToSSHFTPServer(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation, SftpClient remoteServer)
    {
        string uploadFileName = fullyQualifiedDestinationFile + UploadExtension;

        using (SftpClient sftpClient = remoteServer)
        {
            sftpClient.Connect();
            this.TryToAutoCreateDirectoryIfDestinationLocationDoesNotExists(destinationLocation, sftpClient);
            this.HandleOverwriteAtDestination(fullyQualifiedDestinationFile, destinationLocation, sftpClient);

            if (sftpClient.Exists(uploadFileName))
            {
                sftpClient.DeleteFile(uploadFileName);
            }

            using (FileStream fileStream = new FileStream(fullyQualifiedSourceFile, FileMode.Open))
            {
                sftpClient.UploadFile(fileStream, uploadFileName);
            }

            sftpClient.RenameFile(uploadFileName, fullyQualifiedDestinationFile);
            sftpClient.Disconnect();
        }
    }
    #endregion

    #region Delete Methods
    public bool DeleteFromUNC(string fullyQualifiedSourceFileName)
    {
        if (File.Exists(fullyQualifiedSourceFileName))
        {
            File.Delete(fullyQualifiedSourceFileName);
        }

        return true;
    }

    public bool DeleteFromFtp(FTPConnection remoteServer, string fileName)
    {
        remoteServer.Connect();
        bool result = remoteServer.DeleteFile(fileName);
        remoteServer.Close();

        return result;
    }

    public bool DeleteFromSFtp(SftpClient remoteServer, string fileName)
    {
        bool result;

        using (SftpClient sftpClient = remoteServer)
        {
            sftpClient.Connect();

            try
            {
                sftpClient.DeleteFile(fileName);
                result = true;
            }
            catch (Exception ex)
            {
                FTSConsumer.Log(string.Format(FileTransferManagementConstants.ExceptionMessages.ErrorTryingToDeleteFile, ex.ToString()));
                result = false;
            }
            finally
            {
                sftpClient.Disconnect();
            }
        }

        return result;
    }
    #endregion

    #region Rename Methods
    public bool RenameFileFromUNC(string fullyQualifiedSourceFileName, string renameExtension)
    {
        int i = 1;
        string renameFile = fullyQualifiedSourceFileName + renameExtension;
        while (File.Exists(renameFile))
        {
            renameFile = fullyQualifiedSourceFileName + "." + i.ToString() + renameExtension;
            i++;
        }

        File.Move(fullyQualifiedSourceFileName, renameFile);
        return true;
    }

    public bool RenameFtpFile(string fullyQualifiedSourceFileName, string renameExtension, FTPConnection remoteServer)
    {
        remoteServer.Connect();

        // Rename the dup file if it already exists
        int i = 1;
        string renameFile = fullyQualifiedSourceFileName + renameExtension;
        while (remoteServer.Exists(renameFile))
        {
            renameFile = fullyQualifiedSourceFileName + "." + i.ToString() + renameExtension;
            i++;
        }

        // Rename the file
        bool result = remoteServer.RenameFile(fullyQualifiedSourceFileName, renameFile);
        remoteServer.Close();
        return result;
    }

    public bool RenameSSHFtpFile(string fullyQualifiedSourceFileName, string renameExtension, SftpClient remoteServer)
    {
        bool result;

        using (SftpClient sftpClient = remoteServer)
        {
            try
            {
                sftpClient.Connect();
                int fileIndex = 1;
                string renameFile = fullyQualifiedSourceFileName + renameExtension;
                while (sftpClient.Exists(renameFile))
                {
                    renameFile = fullyQualifiedSourceFileName + "." + fileIndex.ToString() + renameExtension;
                    fileIndex++;
                }

                sftpClient.RenameFile(fullyQualifiedSourceFileName, renameFile);
                sftpClient.Disconnect();
                result = true;
            }
            catch (Exception e)
            {
                FTSConsumer.Log(string.Format(FileTransferManagementConstants.ExceptionMessages.ErrorRenamingFile, e.ToString()));
                result = false;
            }
        }

        return result;
    }
    #endregion

    #region List Methods
    public List<FileBasicInfo> GetFileListFromUncSource(SourceLocationModel location)
    {
        List<FileBasicInfo> result = new List<FileBasicInfo>();
        List<string> files = Directory.GetFiles(location.SourceAddress, location.FileMask).ToList();

        foreach (string file in files)
        {
            FileInfo fileInfo = new FileInfo(file);
            result.Add(new FileBasicInfo(file, fileInfo.Extension, fileInfo.Length));
        }

        return result;
    }

    public List<FileBasicInfo> GetFilesFromSource(FTPConnection remoteServer, SourceLocationModel location)
    {
        List<FileBasicInfo> result = new List<FileBasicInfo>();
        remoteServer.Connect();
        var ftpfilesToProcess = remoteServer.GetFileInfos(location.SourceAddress);
        remoteServer.Close();

        foreach (FTPFile f in ftpfilesToProcess)
        {
            if (f.Dir == false && this.patternMatcher.StrictMatchPattern(location.FileMask, f.Name) == true)
            {
                result.Add(new FileBasicInfo(f.Name, f.Size));
            }
        }

        return result;
    }

    public List<FileBasicInfo> GetFilesFromSSHSource(SftpClient remoteServer, SourceLocationModel location)
    {
        List<FileBasicInfo> result = new List<FileBasicInfo>();

        FTSConsumer.Log(string.Format(FileTransferManagementConstants.Information.AttemptingToConnect, location.FtpServerDetail.FtpServerIp));

        using (SftpClient sftp = remoteServer)
        {
            remoteServer.Connect();
            FTSConsumer.Log(FileTransferManagementConstants.Information.ConnectionSuccessful);
            var ftpfilesToProcess = remoteServer.ListDirectory(location.SourceAddress);
            remoteServer.Disconnect();

            foreach (SftpFile file in ftpfilesToProcess)
            {
                if (!file.IsDirectory && this.patternMatcher.StrictMatchPattern(location.FileMask, file.Name))
                {
                    result.Add(new FileBasicInfo(file.Name, file.Attributes.Size));
                }
            }
        }

        return result;
    }
    #endregion

    #region File Utility Methods
    public void CreateZipFile(string inputFileName, string zipFileName, FileInfo fi)
    {
        using (FileStream fileStream = new FileStream(zipFileName, FileMode.Create, FileAccess.Write))
        {
            using (ZipArchive outputArchive = new ZipArchive(fileStream, ZipArchiveMode.Create))
            {
                outputArchive.CreateEntryFromFile(inputFileName, fi.Name);
            }

            fileStream.Close();
        }
    }

    public string GetCrc32(string filePath)
    {
        string result = string.Empty;
        if (File.Exists(filePath))
        {
            var checkSumCalculator = new ICSharpCode.SharpZipLib.Checksum.Crc32();
            int bytesRead = 1;
            int bufferSize = 1048576; //1 MB buffer
            long position = 0;
            using (FileStream stream = File.OpenRead(filePath))
            {
                while (bytesRead != 0)
                {
                    if (stream.Length < (bufferSize + position))
                    {
                        bufferSize = (int)(stream.Length - position);
                    }

                    byte[] buffer = new byte[bufferSize];
                    bytesRead = stream.Read(buffer, 0, bufferSize);

                    if (bytesRead > 0)
                    {
                        checkSumCalculator.Update(buffer);
                    }

                    position = position + bytesRead;
                }

                stream.Close();
            }

            //convert result value to HEX
            result = string.Format("{0:x2}", System.Convert.ToUInt32(checkSumCalculator.Value));
        }

        return result.ToUpper();
    }
    #endregion

    private void SendNotificationOfBadConfig(SourceLocationModel sourceLocation, string tenantId)
    {
        var subject = AppSettingsConstants.EnvironmentName + ": ATTENTION : FTS JOB - A BAD CONFIG HAS BEEN IDENTIFIED .";
        var body = " A bad config has been identified in the FTS job cycle. Please investigate and take appropriate action as needed  \n\n  ***CONFIG:" + sourceLocation.ConfigurationId.ToString() + " *** \n Please check log for more information \n";

        // Config Id indicates the Admin group that gets notified on Job lag / Job failure
        List<Address> recipients = new List<Address>();

        recipients.Add(new Address { EmailAddress = sourceLocation.OwnerEmail, DisplayName = sourceLocation.OwnerName });

        // Is there anyone to send the message to?
        if (recipients.Count != 0 && sourceLocation.SendNotificationsIndicator)
        {
            this.notificationService.SendEmail(
                tenantId,
                subject,
                body,
                recipients,
                new Address(),
                MailPriority.Normal,
                new List<Attachments>());
        }
    }

    private void AttempToDownloadFileWithSSHFtp(SourceLocationModel sourceLocation, DestinationModel destinationLocation, string tenantId, string sourceFullFilePath, SftpClient sftpClient)
    {
        try
        {
            sftpClient.Connect();

            using (Stream fileStream = File.Create(destinationLocation.DestinationFileName))
            {
                sftpClient.DownloadFile(sourceFullFilePath, fileStream);
            }
        }
        catch (Exception ex)
        {
            FTSConsumer.Log(ex.Message);

            if (BadConfigs != null && !BadConfigs.Contains(sourceLocation.ConfigurationId))
            {
                BadConfigs.Add(sourceLocation.ConfigurationId);
                this.SendNotificationOfBadConfig(sourceLocation, tenantId);
            }
        }
    }

    private void TryToAutoCreateDirectoryIfDestinationLocationDoesNotExists(DestinationModel destinationLocation, SftpClient remoteServer)
    {
        if (!remoteServer.Exists(destinationLocation.DestinationAddress))
        {
            if (destinationLocation.DestinationAddress.StartsWith("/"))
            {
                string directorySeparator = string.Empty;
                string dir = "/";
                foreach (string destinationAddress in destinationLocation.DestinationAddress.Substring(1).Split('/'))
                {
                    dir = dir + directorySeparator + destinationAddress;
                    directorySeparator = "/";
                    if (!remoteServer.Exists(dir))
                    {
                        remoteServer.CreateDirectory(dir);
                    }
                }
            }
            else
            {
                throw new Exception(FileTransferManagementConstants.ExceptionMessages.AutoCreateDirectoryFailed);
            }
        }
    }

    private void HandleOverwriteAtDestination(string fullyQualifiedDestinationFile, DestinationModel destinationLocation, SftpClient remoteServer)
    {
        if (remoteServer.Exists(fullyQualifiedDestinationFile) && !destinationLocation.IsOverwriteAtDestination)
        {
            if (!destinationLocation.IsArchive && this.deleteArchiveIfExists)
            {
                remoteServer.DeleteFile(fullyQualifiedDestinationFile);
            }
        }
        else if (remoteServer.Exists(fullyQualifiedDestinationFile))
        {
            remoteServer.DeleteFile(fullyQualifiedDestinationFile);
        }
    }

    private void IsAValidDestination(string destinationAddress)
    {
        if (!Directory.Exists(Path.GetPathRoot(destinationAddress)) || string.IsNullOrWhiteSpace(Path.GetPathRoot(destinationAddress)))
        {
            throw new Exception(string.Format(FileTransferManagementConstants.ExceptionMessages.DestinationPathIsNotValid, destinationAddress));
        }
    }

    private void CloseRemoteServerConnection(FTPConnection remoteServer)
    {
        remoteServer.Dispose();
        remoteServer.Close();

        FTSConsumer.Log(FileTransferManagementConstants.Information.SFTPConnectionClosed);
    }

    private bool IsFileFullyUploaded(FTPConnection remoteServer, string uploadFileName)
    {
        if (!remoteServer.Exists(uploadFileName))
        {
            FTSConsumer.Log(FileTransferManagementConstants.Information.FileFullyMoved);
            return true;
        }

        FTSConsumer.Log(FileTransferManagementConstants.Information.FileWasUnabletoMove);
        return false;
    }

    private bool TryUploadFileToSFTP(FTPConnection remoteServer, string fullyQualifiedSourceFile, string uploadFileName, string fullyQualifiedDestinationFile, DestinationModel destinationLocation)
    {
        if (this.FileAndTemporalFileExistsOnRemoteServer(remoteServer, uploadFileName, fullyQualifiedDestinationFile))
        {
            remoteServer.UploadFile(fullyQualifiedSourceFile, uploadFileName);
            return true;
        }
        else
        {
            destinationLocation.IsFullyUploaded = true;
            this.CloseRemoteServerConnection(remoteServer);
            return false;
        }
    }

    private bool FileAndTemporalFileExistsOnRemoteServer(FTPConnection remoteServer, string uploadFileName, string fullyQualifiedDestinationFile)
    {
        return !remoteServer.Exists(uploadFileName) && !remoteServer.Exists(fullyQualifiedDestinationFile);
    }

    private bool RenameUploadedFile(FTPConnection remoteServer, string uploadFileName, string fullyQualifiedDestinationFile)
    {
        if (remoteServer.Exists(uploadFileName))
        {
            remoteServer.RenameFile(uploadFileName, fullyQualifiedDestinationFile);
            return this.IsFileFullyUploaded(remoteServer, uploadFileName);
        }

        return false;
    }

    private void RemoveTemporaryUploadFile(FTPConnection remoteServer, string uploadFileName)
    {
        if (remoteServer.Exists(uploadFileName))
        {
            remoteServer.DeleteFile(uploadFileName);
        }
    }

    private void CheckAndHandleFileOverwrite(FTPConnection remoteServer, string fullyQualifiedDestinationFile, DestinationModel destinationLocation)
    {
        if (remoteServer.Exists(fullyQualifiedDestinationFile))
        {
            if (!destinationLocation.IsOverwriteAtDestination)
            {
                if (destinationLocation.IsArchive && this.deleteArchiveIfExists)
                {
                    remoteServer.DeleteFile(fullyQualifiedDestinationFile);
                }
            }
            else
            {
                remoteServer.DeleteFile(fullyQualifiedDestinationFile);
            }
        }
    }

    private void CreateRemoteDirectoryIfNotExists(FTPConnection remoteServer, string destinationAddress)
    {
        if (destinationAddress.StartsWith("/"))
        {
            string sep = string.Empty;
            string dir = "/";
            foreach (string p in destinationAddress.Substring(1).Split('/'))
            {
                dir = dir + sep + p;
                sep = "/";
                if (!remoteServer.DirectoryExists(dir))
                {
                    remoteServer.CreateDirectory(dir);
                }
            }
        }
        else
        {
            throw new Exception(FileTransferManagementConstants.ExceptionMessages.AutoCreateDirectoryFailed);
        }
    }
}


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Fxfer.ExecutionWorker.Application.Contracts.Domain;
using Fxfer.ExecutionWorker.Application.Helpers;
using Fxfer.ExecutionWorker.Application.NotificationService;
using Fxfer.ExecutionWorker.Batch.Business.DataAccess;
using Fxfer.ExecutionWorker.Domain.Attributes;
using Fxfer.ExecutionWorker.Domain.Batch.Business;
using Fxfer.ExecutionWorker.Domain.Constants;
using Fxfer.ExecutionWorker.Infrastructure.Protocols;
using Renci.SshNet;

namespace Fxfer.ExecutionWorker.Batch.Business;

public sealed class FileTransferOperations : IFileTransferOperations
{
    private readonly IDataAccessFactory dataAcessData;
    private readonly IFileTransferProtocols fileTransferProtocols;
    private readonly INotificationService notificationService;
    private readonly IAppSettings appSettings;
    private string tenantId = string.Empty;

    private string TenantId
    {
        get => tenantId;
        set => tenantId = value;
    }

    public FileTransferOperations()
    {
        dataAcessData = BatchServiceLocator.GetRequiredService<IDataAccessFactory>();
        fileTransferProtocols = BatchServiceLocator.GetRequiredService<IFileTransferProtocols>();
        notificationService = BatchServiceLocator.GetRequiredService<INotificationService>();
        appSettings = BatchServiceLocator.GetRequiredService<IAppSettings>();
    }

    public FileTransferOperations(IDataAccessFactory dataAccess, IFileTransferProtocols fileTransferProtocols, INotificationService notificationService)
    {
        dataAcessData = dataAccess;
        this.fileTransferProtocols = fileTransferProtocols;
        this.notificationService = notificationService;
        this.appSettings = BatchServiceLocator.GetRequiredService<IAppSettings>();
    }

    public FileTransferOperations(IDataAccessFactory dataAccess, IFileTransferProtocols fileTransferProtocols, INotificationService notificationService, IAppSettings appSettings)
    {
        dataAcessData = dataAccess;
        this.fileTransferProtocols = fileTransferProtocols;
        this.notificationService = notificationService;
        this.appSettings = appSettings;
    }

    public bool DownloadFile(SourceLocationModel source, string sourceFileName, DestinationModel destination)
    {
        return DownloadFile(source, sourceFileName, destination, TenantId);
    }

    public bool DownloadFile(SourceLocationModel source, string sourceFileName, DestinationModel destination, string tenantId)
    {
        TenantId = tenantId;
        switch (source.SourceProtocolCode)
        {
            case ProtocolCodeConstant.UNC:
                fileTransferProtocols.DownloadFromUNCConnection(sourceFileName, destination);
                break;
            case ProtocolCodeConstant.FTP:
                DownloadFileFromFtp(source, sourceFileName, destination);
                break;
            case ProtocolCodeConstant.SFTP:
                if (source.FtpServerDetail.SSHAuth)
                {
                    DownloadFileFromSftpUsingSSHKeys(source, sourceFileName, destination);
                }
                else
                {
                    DownloadFileFromSftp(source, sourceFileName, destination);
                }

                break;
            case ProtocolCodeConstant.FTPS:
                DownloadFileFromFtps(source, sourceFileName, destination);
                break;
            default:
                break;
        }

        return true;
    }

    public string UploadFile(string fullyQualifiedSourceFileName, string fullyQualifiedDestinationFileName, DestinationModel destinationLocation)
    {
        switch (destinationLocation.DestinationProtocolCode)
        {
            case ProtocolCodeConstant.UNC:
                fullyQualifiedDestinationFileName = fileTransferProtocols.UploadToUnc(fullyQualifiedSourceFileName, fullyQualifiedDestinationFileName, destinationLocation);
                break;
            case ProtocolCodeConstant.FTP:
                if (!UploadFileToFtp(fullyQualifiedSourceFileName, fullyQualifiedDestinationFileName, destinationLocation))
                {
                    return null!;
                }

                break;
            case ProtocolCodeConstant.SFTP:
                if (destinationLocation.FtpServerDetail.SSHAuth)
                {
                    UploadFiletoSSHFtp(fullyQualifiedSourceFileName, fullyQualifiedDestinationFileName, destinationLocation);
                }
                else if (!UploadFiletoSftp(fullyQualifiedSourceFileName, fullyQualifiedDestinationFileName, destinationLocation))
                {
                    return null!;
                }

                break;
            case ProtocolCodeConstant.FTPS:
                if (!UploadFiletoFtps(fullyQualifiedSourceFileName, fullyQualifiedDestinationFileName, destinationLocation))
                {
                    return null!;
                }

                break;
            default:
                break;
        }

        return fullyQualifiedDestinationFileName;
    }

    public bool DeleteFile(SourceLocationModel source, string fullyQualifedSourceFile)
    {
        switch (source.SourceProtocolCode)
        {
            case ProtocolCodeConstant.UNC:
                fileTransferProtocols.DeleteFromUNC(fullyQualifedSourceFile);
                break;
            case ProtocolCodeConstant.FTP:
                DeleteFileFromFtp(source, fullyQualifedSourceFile);
                break;
            case ProtocolCodeConstant.SFTP:
                if (source.FtpServerDetail.SSHAuth)
                {
                    DeleteFileFromSSHFtp(source, fullyQualifedSourceFile);
                }
                else
                {
                    DeleteFileFromSftp(source, fullyQualifedSourceFile);
                }

                break;
            case ProtocolCodeConstant.FTPS:
                DeleteFileFromFtps(source, fullyQualifedSourceFile);
                break;
            default:
                break;
        }

        return true;
    }

    public bool RenameSourceFile(SourceLocationModel source, string fullyQualifedSourceFile, string renameExtension)
    {
        switch (source.SourceProtocolCode)
        {
            case ProtocolCodeConstant.UNC:
                fileTransferProtocols.RenameFileFromUNC(fullyQualifedSourceFile, renameExtension);
                break;
            case ProtocolCodeConstant.FTP:
                RenameFileFromFtp(source, fullyQualifedSourceFile, renameExtension);
                break;
            case ProtocolCodeConstant.SFTP:
                if (source.FtpServerDetail.SSHAuth)
                {
                    RenameFileFromSSHFtp(source, fullyQualifedSourceFile, renameExtension);
                }
                else
                {
                    RenameFileFromSftp(source, fullyQualifedSourceFile, renameExtension);
                }

                break;
            case ProtocolCodeConstant.FTPS:
                RenameFileFromFtps(source, fullyQualifedSourceFile, renameExtension);
                break;
            default:
                break;
        }

        return true;
    }

    public List<FileBasicInfo> GetListOfFileNamesFromSource(SourceLocationModel location, int maxFilesPerCycle)
    {
        List<FileBasicInfo> result = new List<FileBasicInfo>();
        switch (location.SourceProtocolCode)
        {
            case ProtocolCodeConstant.UNC:
                result = fileTransferProtocols.GetFileListFromUncSource(location);
                break;
            case ProtocolCodeConstant.FTP:
                result = GetFileListFromFtpSource(location);
                break;
            case ProtocolCodeConstant.SFTP:
                if (location.FtpServerDetail.SSHAuth)
                {
                    result = GetFileListFromSSHFtpSource(location);
                }
                else
                {
                    result = GetFileListFromSftpSource(location);
                }

                break;
            case ProtocolCodeConstant.FTPS:
                result = GetFileListFromFtpsSource(location);
                break;
            default:
                break;
        }

        // app.config setting (default 50 files) to stop a single high volume source from blocking other source locations
        if (result.Count > maxFilesPerCycle)
        {
            result = result.Take(maxFilesPerCycle).ToList();
        }

        return result;
    }

    public string GetCrc32(string filePath)
    {
        return fileTransferProtocols.GetCrc32(filePath);
    }

    public string GetDestinationFileName(string fullyQualifiedSourceFileName, DestinationModel destinationLocation, Guid fileId, string crc32, string uploadedSFTPUserName)
    {
        if (destinationLocation.FTEMetadataId != null)
        {
            destinationLocation.DestinationAddress = appSettings[AppSettingsConstants.ExternalFTECommonFolder]!;
        }

        FileInfo fileInfo = new FileInfo(fullyQualifiedSourceFileName);
        string result = fileInfo.Name;

        if (destinationLocation.IsRenameAtDestination == true && !string.IsNullOrEmpty(destinationLocation.RenamePattern))
        {
            ////<DATEDIR> is used to create a date stamped directorystructure at the archive location (ex: 2014/6/15)
            ////this param is not use in renaming the file, so is removed when checking the rename pattern

            string constValue = string.Empty;
            string constTagOpen = @"<CONST>";
            string constTagClose = @"</CONST>";
            if (destinationLocation.RenamePattern.Contains(constTagOpen))
            {
                // get tag positions
                int openTagPos = destinationLocation.RenamePattern.IndexOf(constTagOpen) + constTagOpen.Length;
                int closetagpos = destinationLocation.RenamePattern.IndexOf(constTagClose) - openTagPos;

                // get constant value based on tag positions
                constValue = destinationLocation.RenamePattern.Substring(openTagPos, closetagpos);

                // remove from rename pattern
                string fullTag = constTagOpen + constValue + constTagClose;
                _ = fullTag; // legacy computes then does not use.
            }

            if (destinationLocation.RenamePattern.Contains("<DATEDIR>"))
            {
                if (destinationLocation.DestinationProtocolCode == ProtocolCodeConstant.UNC)
                {
                    destinationLocation.DestinationAddress += @"\\" + DateTime.Today.Year + @"\\" + DateTime.Today.Month + @"\\" + DateTime.Today.Day;
                }
                else
                {
                    destinationLocation.DestinationAddress += @"/" + DateTime.Today.Year + @"/" + DateTime.Today.Month + @"/" + DateTime.Today.Day;
                }

                destinationLocation.RenamePattern = destinationLocation.RenamePattern.Replace("<DATEDIR>", string.Empty);
            }

            switch (destinationLocation.RenamePattern)
            {
                case "1":
                    string date = DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString().PadLeft(2, '0') + DateTime.Today.Day.ToString().PadLeft(2, '0');
                    string time = DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0');
                    string filename = Path.GetFileNameWithoutExtension(fileInfo.Name);
                    string extension = Path.GetExtension(fileInfo.Name);

                    result = filename + "_" + date + "_" + time + extension;
                    break;
                case "2":
                    result = fileId.ToString() + "_" + crc32 + "_" + fileInfo.Name;
                    break;
                case "3":
                    if (string.IsNullOrEmpty(uploadedSFTPUserName))
                    {
                        result = "UserId_" + fileId.ToString() + "_" + crc32 + "_" + fileInfo.Name;
                    }
                    else
                        result = uploadedSFTPUserName + "_" + fileId.ToString() + "_" + crc32 + "_" + fileInfo.Name;
                    break;
                case "4":
                    result = fileId.ToString() + "_" + fileInfo.Name;
                    break;
                case "5":
                    result = fileId.ToString() + "_" + fileInfo.Name;
                    break;
                default:
                    break;
            }

            result = constValue + result;
        }

        if (destinationLocation.DestinationProtocolCode == ProtocolCodeConstant.UNC)
        {
            result = destinationLocation.DestinationAddress + "\\" + result;
        }
        else
        {
            result = destinationLocation.DestinationAddress + "/" + result;
        }

        LoggerManager.Logger.LogDebug(string.Format(FileTransferManagementConstants.Information.DestinationFileName, result));
        return result;
    }

    public string CreateZipFile(string inputFileName)
    {
        var fi = new FileInfo(inputFileName);
        if (fi.Extension == @".zip")
        {
            return inputFileName;
        }

        string zipFileName = inputFileName + ".zip";
        if (File.Exists(zipFileName))
        {
            return zipFileName;
        }

        fileTransferProtocols.CreateZipFile(inputFileName, zipFileName, fi);

        return zipFileName;
    }

    public void SendNotification(long configId, string subject, string body, bool isErrorNotification)
    {
        GetNotificationSearchCriteria searchQuery = new GetNotificationSearchCriteria();
        List<FtsNotificationModel> notifications = dataAcessData.CreateQueryFtsNotificationDataAccess(searchQuery).Execute();
        List<Address> recipients = new List<Address>();
        foreach (FtsNotificationModel notification in notifications)
        {
            if (isErrorNotification)
            {
                if (notification.NotifyOnErrorInd)
                {
                    recipients.Add(new Address { EmailAddress = notification.EmailAddress });
                }
            }
            else
            {
                if (notification.NotifyOnSuccessInd)
                {
                    recipients.Add(new Address { EmailAddress = notification.EmailAddress });
                }
            }
        }

        // Is there anyone to send the message to?
        if (recipients.Count == 0)
        {
            return;
        }

        // shortcut: SendNotification legacy signature does not include tenant id; use the most recent tenant id if available.
        var helper = new NotificationServiceHelper(ResolveTenantId(), notificationService);
        helper.SendEmail(subject, body, recipients, null!, MailPriority.Normal, null!);
    }

    public void SendNotification(SourceLocationModel source, string subject, string body, bool isErrorNotification, string tenantId)
    {
        TenantId = tenantId;

        List<Address> recipients = new List<Address>()
        {
            new Address { EmailAddress = source.OwnerEmail, DisplayName = source.OwnerName },
        };

        // Is there anyone to send the message to?
        if (recipients.Count != 0)
        {
            var notificationServiceHelper = new NotificationServiceHelper(TenantId, notificationService);
            notificationServiceHelper.SendEmail(subject, body, recipients, null!, MailPriority.Normal, null!);
        }
    }

    public void PostEvent(string eventName, string subSystem, string messageBody, bool isArchived, string operatorId)
    {
        var notificationServiceHelper = new NotificationServiceHelper(TenantId, notificationService);
        notificationServiceHelper.PostEvent(eventName, subSystem, messageBody, isArchived, operatorId);
    }

    private string ResolveTenantId()
    {
        if (!string.IsNullOrEmpty(tenantId))
        {
            return tenantId;
        }

        return Environment.GetEnvironmentVariable("FXFER_TENANT_ID") ?? string.Empty;
    }

    private List<FileBasicInfo> GetFileListFromFtpSource(SourceLocationModel location)
    {
        FTPConnection ftpConnection = new FTPConnection();
        ftpConnection.ServerAddress = location.FtpServerDetail.FtpServerIp;
        ftpConnection.UserName = location.FtpServerDetail.FtpUser;
        ftpConnection.Password = location.FtpServerDetail.FtpPassword;
        ftpConnection.ServerPort = location.FtpServerDetail.FtpPort;

        if (!string.IsNullOrEmpty(appSettings[AppSettingsConstants.FTPConnectMode]))
        {
            ftpConnection.ConnectMode = ParseEnum<FTPConnectMode>(appSettings[AppSettingsConstants.FTPConnectMode]!);
        }

        return fileTransferProtocols.GetFilesFromSource(ftpConnection, location);
    }

    [MaintainabilityDetails(ApprovedIndex = 57, ReviewedOnSprint = 94, ReviewerName = "Josue Chavez")]
    private List<FileBasicInfo> GetFileListFromFtpsSource(SourceLocationModel location)
    {
        SecureFTPConnection ftpConnection = new SecureFTPConnection();
        ftpConnection.ServerAddress = location.FtpServerDetail.FtpServerIp;
        ftpConnection.UserName = location.FtpServerDetail.FtpUser;
        ftpConnection.Password = location.FtpServerDetail.FtpPassword;
        if (!string.IsNullOrEmpty(appSettings[AppSettingsConstants.FtpsConnectionProtocol]))
        {
            ftpConnection.Protocol = ParseEnum<FileTransferProtocol>(appSettings[AppSettingsConstants.FtpsConnectionProtocol]!);
        }
        else
        {
            ftpConnection.Protocol = FileTransferProtocol.FTPSExplicit;
        }

        ftpConnection.ServerPort = location.FtpServerDetail.FtpPort;

        if (!string.IsNullOrEmpty(appSettings[AppSettingsConstants.FTPConnectMode]))
        {
            ftpConnection.ConnectMode = ParseEnum<FTPConnectMode>(appSettings[AppSettingsConstants.FTPConnectMode]!);
        }

        return fileTransferProtocols.GetFilesFromSource(ftpConnection, location);
    }

    private List<FileBasicInfo> GetFileListFromSftpSource(SourceLocationModel location)
    {
        SecureFTPConnection sftpConnection = new SecureFTPConnection();
        sftpConnection.Protocol = FileTransferProtocol.SFTP;
        sftpConnection.ServerAddress = location.FtpServerDetail.FtpServerIp;
        sftpConnection.ServerPort = location.FtpServerDetail.FtpPort;
        sftpConnection.UserName = location.FtpServerDetail.FtpUser;
        sftpConnection.Password = location.FtpServerDetail.FtpPassword;
        sftpConnection.ServerCompatibility = SecureFTPCompatibilityFlags.SSHDisableChmodAfterPut;

        return fileTransferProtocols.GetFilesFromSource(sftpConnection, location);
    }

    private List<FileBasicInfo> GetFileListFromSSHFtpSource(SourceLocationModel location)
    {
        return fileTransferProtocols.GetFilesFromSSHSource(BuildSftpClient(location.FtpServerDetail.FtpUser, location.FtpServerDetail.FtpPassword, location.FtpServerDetail.FtpServerIp, location.FtpServerDetail.FtpPort), location);
    }

    private T ParseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    private bool DownloadFileFromFtp(SourceLocationModel sourceLocation, string remoteFileName, DestinationModel destinationLocation)
    {
        FTPConnection remoteServer = new FTPConnection()
        {
            ServerAddress = sourceLocation.FtpServerDetail.FtpServerIp,
            ServerDirectory = sourceLocation.SourceAddress,
            UserName = sourceLocation.FtpServerDetail.FtpUser,
            Password = sourceLocation.FtpServerDetail.FtpPassword,
            ServerPort = sourceLocation.FtpServerDetail.FtpPort,
        };
        fileTransferProtocols.DownloadFromFtpConnection(remoteServer, sourceLocation, remoteFileName, destinationLocation, TenantId);

        return true;
    }

    private bool DownloadFileFromSftp(SourceLocationModel sourceLocation, string remoteFileName, DestinationModel destinationLocation)
    {
        SecureFTPConnection remoteServer = new SecureFTPConnection()
        {
            ServerAddress = sourceLocation.FtpServerDetail.FtpServerIp,
            ServerDirectory = sourceLocation.SourceAddress,
            UserName = sourceLocation.FtpServerDetail.FtpUser,
            Password = sourceLocation.FtpServerDetail.FtpPassword,
            ServerPort = sourceLocation.FtpServerDetail.FtpPort,
            Protocol = FileTransferProtocol.SFTP,
            ServerCompatibility = SecureFTPCompatibilityFlags.SSHDisableChmodAfterPut,
        };

        fileTransferProtocols.DownloadFromFtpConnection(remoteServer, sourceLocation, remoteFileName, destinationLocation, TenantId);
        return true;
    }

    private bool DownloadFileFromFtps(SourceLocationModel sourceLocation, string remoteFileName, DestinationModel destinationLocation)
    {
        SecureFTPConnection remoteServer = new SecureFTPConnection()
        {
            ServerAddress = sourceLocation.FtpServerDetail.FtpServerIp,
            ServerDirectory = sourceLocation.SourceAddress,
            UserName = sourceLocation.FtpServerDetail.FtpUser,
            Password = sourceLocation.FtpServerDetail.FtpPassword,
            ServerPort = sourceLocation.FtpServerDetail.FtpPort,
        };

        if (!string.IsNullOrEmpty(appSettings[AppSettingsConstants.FtpsConnectionProtocol]))
        {
            remoteServer.Protocol = ParseEnum<FileTransferProtocol>(appSettings[AppSettingsConstants.FtpsConnectionProtocol]!);
        }
        else
        {
            remoteServer.Protocol = FileTransferProtocol.FTPSExplicit;
        }

        fileTransferProtocols.DownloadFromFtpConnection(remoteServer, sourceLocation, remoteFileName, destinationLocation, TenantId);

        return true;
    }

    private bool UploadFileToFtp(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation)
    {
        FTPConnection remoteServer = new FTPConnection()
        {
            ServerAddress = destinationLocation.FtpServerDetail.FtpServerIp,
            ServerDirectory = destinationLocation.DestinationAddress,
            UserName = destinationLocation.FtpServerDetail.FtpUser,
            Password = destinationLocation.FtpServerDetail.FtpPassword,
            ServerPort = destinationLocation.FtpServerDetail.FtpPort,
        };

        return fileTransferProtocols.UploadToFTPServer(fullyQualifiedSourceFile, fullyQualifiedDestinationFile, destinationLocation, remoteServer);
    }

    private bool UploadFiletoSftp(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation)
    {
        SecureFTPConnection remoteServer = new SecureFTPConnection()
        {
            ServerAddress = destinationLocation.FtpServerDetail.FtpServerIp,
            ServerDirectory = destinationLocation.DestinationAddress,
            UserName = destinationLocation.FtpServerDetail.FtpUser,
            Password = destinationLocation.FtpServerDetail.FtpPassword,
            ServerPort = destinationLocation.FtpServerDetail.FtpPort,
            Protocol = FileTransferProtocol.SFTP,
            ServerCompatibility = SecureFTPCompatibilityFlags.SSHDisableChmodAfterPut,
        };

        return fileTransferProtocols.UploadToFTPServer(fullyQualifiedSourceFile, fullyQualifiedDestinationFile, destinationLocation, remoteServer);
    }

    private bool UploadFiletoFtps(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation)
    {
        SecureFTPConnection remoteServer = new SecureFTPConnection()
        {
            ServerAddress = destinationLocation.FtpServerDetail.FtpServerIp,
            ServerDirectory = destinationLocation.DestinationAddress,
            UserName = destinationLocation.FtpServerDetail.FtpUser,
            Password = destinationLocation.FtpServerDetail.FtpPassword,
            ServerPort = destinationLocation.FtpServerDetail.FtpPort,
        };

        if (!string.IsNullOrEmpty(appSettings[AppSettingsConstants.FtpsConnectionProtocol]))
        {
            remoteServer.Protocol = ParseEnum<FileTransferProtocol>(appSettings[AppSettingsConstants.FtpsConnectionProtocol]!);
        }
        else
        {
            remoteServer.Protocol = FileTransferProtocol.FTPSExplicit;
        }

        return fileTransferProtocols.UploadToFTPServer(fullyQualifiedSourceFile, fullyQualifiedDestinationFile, destinationLocation, remoteServer);
    }

    private bool DeleteFileFromFtp(SourceLocationModel source, string fullyQualifiedSourceFileName)
    {
        FTPConnection remoteServer = new FTPConnection()
        {
            ServerAddress = source.FtpServerDetail.FtpServerIp,
            ServerDirectory = source.SourceAddress,
            UserName = source.FtpServerDetail.FtpUser,
            Password = source.FtpServerDetail.FtpPassword,
            ServerPort = source.FtpServerDetail.FtpPort,
        };
        return fileTransferProtocols.DeleteFromFtp(remoteServer, fullyQualifiedSourceFileName);
    }

    private bool DeleteFileFromFtps(SourceLocationModel source, string fullyQualifiedSourceFileName)
    {
        SecureFTPConnection remoteServer = new SecureFTPConnection()
        {
            ServerAddress = source.FtpServerDetail.FtpServerIp,
            ServerDirectory = source.SourceAddress,
            UserName = source.FtpServerDetail.FtpUser,
            Password = source.FtpServerDetail.FtpPassword,
            ServerPort = source.FtpServerDetail.FtpPort,
        };

        if (!string.IsNullOrEmpty(appSettings[AppSettingsConstants.FtpsConnectionProtocol]))
        {
            remoteServer.Protocol = ParseEnum<FileTransferProtocol>(appSettings[AppSettingsConstants.FtpsConnectionProtocol]!);
        }
        else
        {
            remoteServer.Protocol = FileTransferProtocol.FTPSExplicit;
        }

        return fileTransferProtocols.DeleteFromFtp(remoteServer, fullyQualifiedSourceFileName);
    }

    private bool DeleteFileFromSftp(SourceLocationModel source, string fullyQualifiedSourceFileName)
    {
        SecureFTPConnection remoteServer = new SecureFTPConnection()
        {
            ServerAddress = source.FtpServerDetail.FtpServerIp,
            ServerDirectory = source.SourceAddress,
            UserName = source.FtpServerDetail.FtpUser,
            Password = source.FtpServerDetail.FtpPassword,
            ServerPort = source.FtpServerDetail.FtpPort,
            Protocol = FileTransferProtocol.SFTP,
            ServerCompatibility = SecureFTPCompatibilityFlags.SSHDisableChmodAfterPut,
        };
        return fileTransferProtocols.DeleteFromFtp(remoteServer, fullyQualifiedSourceFileName);
    }

    private bool RenameFileFromFtp(SourceLocationModel source, string fullyQualifiedSourceFileName, string renameExtension)
    {
        FTPConnection remoteServer = new FTPConnection()
        {
            ServerAddress = source.FtpServerDetail.FtpServerIp,
            ServerDirectory = source.SourceAddress,
            UserName = source.FtpServerDetail.FtpUser,
            Password = source.FtpServerDetail.FtpPassword,
            ServerPort = source.FtpServerDetail.FtpPort,
        };

        return fileTransferProtocols.RenameFtpFile(fullyQualifiedSourceFileName, renameExtension, remoteServer);
    }

    private bool RenameFileFromFtps(SourceLocationModel source, string fullyQualifiedSourceFileName, string renameExtension)
    {
        SecureFTPConnection remoteServer = new SecureFTPConnection()
        {
            ServerAddress = source.FtpServerDetail.FtpServerIp,
            ServerDirectory = source.SourceAddress,
            UserName = source.FtpServerDetail.FtpUser,
            Password = source.FtpServerDetail.FtpPassword,
            ServerPort = source.FtpServerDetail.FtpPort,
        };

        if (!string.IsNullOrEmpty(appSettings[AppSettingsConstants.FtpsConnectionProtocol]))
        {
            remoteServer.Protocol = ParseEnum<FileTransferProtocol>(appSettings[AppSettingsConstants.FtpsConnectionProtocol]!);
        }
        else
        {
            remoteServer.Protocol = FileTransferProtocol.FTPSExplicit;
        }

        return fileTransferProtocols.RenameFtpFile(fullyQualifiedSourceFileName, renameExtension, remoteServer);
    }

    private bool RenameFileFromSftp(SourceLocationModel source, string fullyQualifiedSourceFileName, string renameExtension)
    {
        SecureFTPConnection remoteServer = new SecureFTPConnection()
        {
            ServerAddress = source.FtpServerDetail.FtpServerIp,
            ServerDirectory = source.SourceAddress,
            UserName = source.FtpServerDetail.FtpUser,
            Password = source.FtpServerDetail.FtpPassword,
            ServerPort = source.FtpServerDetail.FtpPort,
            Protocol = FileTransferProtocol.SFTP,
            ServerCompatibility = SecureFTPCompatibilityFlags.SSHDisableChmodAfterPut,
        };

        return fileTransferProtocols.RenameFtpFile(fullyQualifiedSourceFileName, renameExtension, remoteServer);
    }

    private bool DownloadFileFromSftpUsingSSHKeys(SourceLocationModel sourceLocation, string remoteFileName, DestinationModel destinationLocation)
    {
        fileTransferProtocols.DownloadFromSSHFtpConnection(BuildSftpClient(sourceLocation.FtpServerDetail.FtpUser, sourceLocation.FtpServerDetail.FtpPassword, sourceLocation.FtpServerDetail.FtpServerIp, sourceLocation.FtpServerDetail.FtpPort), sourceLocation, remoteFileName, destinationLocation, TenantId);
        return true;
    }

    private bool UploadFiletoSSHFtp(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation)
    {
        fileTransferProtocols.UploadToSSHFTPServer(fullyQualifiedSourceFile, fullyQualifiedDestinationFile, destinationLocation, BuildSftpClient(destinationLocation.FtpServerDetail.FtpUser, destinationLocation.FtpServerDetail.FtpPassword, destinationLocation.FtpServerDetail.FtpServerIp, destinationLocation.FtpServerDetail.FtpPort));
        return true;
    }

    private bool DeleteFileFromSSHFtp(SourceLocationModel source, string fullyQualifiedSourceFileName)
    {
        return fileTransferProtocols.DeleteFromSFtp(BuildSftpClient(source.FtpServerDetail.FtpUser, source.FtpServerDetail.FtpPassword, source.FtpServerDetail.FtpServerIp, source.FtpServerDetail.FtpPort), fullyQualifiedSourceFileName);
    }

    private bool RenameFileFromSSHFtp(SourceLocationModel source, string fullyQualifiedSourceFileName, string renameExtension)
    {
        return fileTransferProtocols.RenameSSHFtpFile(fullyQualifiedSourceFileName, renameExtension, BuildSftpClient(source.FtpServerDetail.FtpUser, source.FtpServerDetail.FtpPassword, source.FtpServerDetail.FtpServerIp, source.FtpServerDetail.FtpPort));
    }

    private SftpClient BuildSftpClient(string ftpUser, string passprhase, string ftpServerIp, int ftpPort)
    {
        PrivateKeyFile pkRsa;
        pkRsa = SetPrivateKey(passprhase);

        PrivateKeyAuthenticationMethod authenticationMethodRsa = new PrivateKeyAuthenticationMethod(ftpUser, pkRsa);

        ConnectionInfo conn = new ConnectionInfo(ftpServerIp, ftpPort, ftpUser, authenticationMethodRsa);

        using (SftpClient sftpClient = new SftpClient(conn))
        {
            _ = sftpClient; // legacy uses a using scope here.
            RsaSha256Util.SetupConnection(conn);
            return new SftpClient(conn);
        }
    }

    private PrivateKeyFile SetPrivateKey(string passprhase)
    {
        PrivateKeyFile pkRsa;
        var executingAssembly = Assembly.GetExecutingAssembly();
        if (executingAssembly == null)
        {
            throw new InvalidOperationException("Could not get executing assembly.");
        }

        // shortcut: legacy used AssemblyName.CodeBase + Substring(6); Assembly.Location is equivalent on .NET.
        string pathToKeyFile = Path.GetDirectoryName(executingAssembly.Location)!;

        pathToKeyFile += FileTransferManagementConstants.Paths.PathToPrivateKeyFile;
        byte[] regularKey = File.ReadAllBytes(pathToKeyFile);
        string keyText = File.ReadAllText(pathToKeyFile);

        if (keyText.Contains(FileTransferManagementConstants.Information.Encrypted))
        {
            pkRsa = new PrivateKeyFile(new MemoryStream(regularKey), passprhase);
        }
        else
        {
            pkRsa = new PrivateKeyFile(new MemoryStream(regularKey));
        }

        RsaSha256Util.ConvertToKeyWithSha256Signature(pkRsa);
        return pkRsa;
    }
}

using EnterpriseDT.Net.Ftp;
using Fxfer.TransferWorker.Application.Constants;
using Fxfer.TransferWorker.Contracts.Batch;
using Fxfer.TransferWorker.Contracts.NotificationService;
using Fxfer.TransferWorker.Domain.Batch;
using Fxfer.TransferWorker.Domain.Constants;
using Fxfer.TransferWorker.Worker.Configuration;
using Microsoft.Extensions.Logging;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fxfer.TransferWorker.Application.Batch;

public sealed class FileTransferOperations : IFileTransferOperations
{
    private readonly IFileTransferProtocols fileTransferProtocols;
    private readonly INotificationService notificationService;
    private readonly ITenantAppSettingsManager configurationManager;
    private readonly ILogger<FileTransferOperations> logger;

    private string TenantId { get; set; } = string.Empty;

    public FileTransferOperations(
        IFileTransferProtocols fileTransferProtocols,
        INotificationService notificationService,
        ITenantAppSettingsManager configurationManager,
        ILogger<FileTransferOperations> logger)
    {
        this.fileTransferProtocols = fileTransferProtocols;
        this.notificationService = notificationService;
        this.configurationManager = configurationManager;
        this.logger = logger;
    }

        #region Public Methods
        public bool DownloadFile(SourceLocationModel source, string sourceFileName, DestinationModel destination, string tenantId)
        {
            this.TenantId = tenantId;
            switch (source.SourceProtocolCode)
            {
                case ProtocolCodeConstant.UNC:
                    this.fileTransferProtocols.DownloadFromUNCConnection(sourceFileName, destination);
                    break;
                case ProtocolCodeConstant.FTP:
                    this.DownloadFileFromFtp(source, sourceFileName, destination);
                    break;
                case ProtocolCodeConstant.SFTP:
                    if (source.FtpServerDetail.SSHAuth)
                    {
                        this.DownloadFileFromSftpUsingSSHKeys(source, sourceFileName, destination);
                    }
                    else
                    {
                        this.DownloadFileFromSftp(source, sourceFileName, destination);
                    }
                    
                    break;
                case ProtocolCodeConstant.FTPS:
                    this.DownloadFileFromFtps(source, sourceFileName, destination);
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
                    fullyQualifiedDestinationFileName = this.fileTransferProtocols.UploadToUnc(fullyQualifiedSourceFileName, fullyQualifiedDestinationFileName, destinationLocation);
                    break;
                case ProtocolCodeConstant.FTP:
                    if (!this.UploadFileToFtp(fullyQualifiedSourceFileName, fullyQualifiedDestinationFileName, destinationLocation))
                    {
                        return null;
                    }
                    break;
                case ProtocolCodeConstant.SFTP:
                    if (destinationLocation.FtpServerDetail.SSHAuth)
                    {
                        this.UploadFiletoSSHFtp(fullyQualifiedSourceFileName, fullyQualifiedDestinationFileName, destinationLocation);
                    }
                    else if (!this.UploadFiletoSftp(fullyQualifiedSourceFileName, fullyQualifiedDestinationFileName, destinationLocation))
                    {
                        return null;
                    }
                    break;
                case ProtocolCodeConstant.FTPS:
                    if (!this.UploadFiletoFtps(fullyQualifiedSourceFileName, fullyQualifiedDestinationFileName, destinationLocation))
                    {
                        return null;
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
                    this.fileTransferProtocols.DeleteFromUNC(fullyQualifedSourceFile);
                    break;
                case ProtocolCodeConstant.FTP:
                    this.DeleteFileFromFtp(source, fullyQualifedSourceFile);
                    break;
                case ProtocolCodeConstant.SFTP:
                    if (source.FtpServerDetail.SSHAuth)
                    {
                        this.DeleteFileFromSSHFtp(source, fullyQualifedSourceFile);
                    }
                    else
                    {
                        this.DeleteFileFromSftp(source, fullyQualifedSourceFile);
                    }
                    break;
                case ProtocolCodeConstant.FTPS:
                    this.DeleteFileFromFtps(source, fullyQualifedSourceFile);
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
                    this.fileTransferProtocols.RenameFileFromUNC(fullyQualifedSourceFile, renameExtension);
                    break;
                case ProtocolCodeConstant.FTP:
                    this.RenameFileFromFtp(source, fullyQualifedSourceFile, renameExtension);
                    break;
                case ProtocolCodeConstant.SFTP:
                    if (source.FtpServerDetail.SSHAuth)
                    {
                        this.RenameFileFromSSHFtp(source, fullyQualifedSourceFile, renameExtension);
                    }
                    else
                    {
                        this.RenameFileFromSftp(source, fullyQualifedSourceFile, renameExtension);
                    }
                    break;
                case ProtocolCodeConstant.FTPS:
                    this.RenameFileFromFtps(source, fullyQualifedSourceFile, renameExtension);
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
                    result = this.fileTransferProtocols.GetFileListFromUncSource(location);
                    break;
                case ProtocolCodeConstant.FTP:
                    result = this.GetFileListFromFtpSource(location);
                    break;
                case ProtocolCodeConstant.SFTP:
                    if (location.FtpServerDetail.SSHAuth)
                    {
                        result = this.GetFileListFromSSHFtpSource(location);
                    }
                    else
                    {
                        result = this.GetFileListFromSftpSource(location);
                    }
                    break;
                case ProtocolCodeConstant.FTPS:
                    result = this.GetFileListFromFtpsSource(location);
                    break;
                default:
                    break;
            }

            //app.config setting (default 50 files) to stop a single high volume source from blocking other source locations
            if (result.Count > maxFilesPerCycle)
            {
                result = result.Take(maxFilesPerCycle).ToList();
            }

            return result;
        }

        public string GetCrc32(string filePath)
        {
            return this.fileTransferProtocols.GetCrc32(filePath);
        }
        #endregion

        #region Public File Utility Methods      

        public string GetDestinationFileName(string fullyQualifiedSourceFileName, DestinationModel destinationLocation, Guid fileId, string crc32, string uploadedSFTPUserName)
        {
            if (destinationLocation.FTEMetadataId != null)
            {
                destinationLocation.DestinationAddress = this.configurationManager.AppSettings?[AppSettingsConstants.ExternalFTECommonFolder];
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
                    //get tag positions                   
                    int openTagPos = destinationLocation.RenamePattern.IndexOf(constTagOpen) + constTagOpen.Length;
                    int closetagpos = destinationLocation.RenamePattern.IndexOf(constTagClose) - openTagPos;

                    //get constant value based on tag positions
                    constValue = destinationLocation.RenamePattern.Substring(openTagPos, closetagpos);

                    //remove from rename pattern 
                    string fullTag = constTagOpen + constValue + constTagClose;
                }

                if (destinationLocation.RenamePattern.Contains("<DATEDIR>"))
                {
                    if (destinationLocation.DestinationProtocolCode == ProtocolCodeConstant.UNC)
                    {
                        destinationLocation.DestinationAddress += @"\" + DateTime.Today.Year + @"\" + DateTime.Today.Month + @"\" + DateTime.Today.Day;
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

            this.logger.LogDebug(FileTransferManagementConstants.Information.DestinationFileName, result);
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

            this.fileTransferProtocols.CreateZipFile(inputFileName, zipFileName, fi);

            return zipFileName;
        }

        public void SendNotification(SourceLocationModel sourceLocation, string subject, string body, bool isErrorNotification, string tenantId)
        {
            this.TenantId = tenantId;

            List<Address> recipients = new List<Address>()
            {
                new Address { EmailAddress = sourceLocation.OwnerEmail, DisplayName = sourceLocation.OwnerName }
            };

            // Is there anyone to send the message to?
            if (recipients.Count != 0)
            {
                this.notificationService.SendEmail(
                    this.TenantId,
                    subject,
                    body,
                    recipients,
                    new Address(),
                    MailPriority.Normal,
                    new List<Attachments>());
            }
        }

        public void PostEvent(string eventName, string subSystem, string messageBody, bool isArchived, string operatorId)
        {
            _ = this.notificationService.PostEvent(
                eventName,
                subSystem,
                messageBody,
                isArchived,
                DateTime.UtcNow,
                null,
                operatorId,
                this.TenantId);
        }
        #endregion

        #region Private List Methods
        private List<FileBasicInfo> GetFileListFromFtpSource(SourceLocationModel location)
        {
            FTPConnection ftpConnection = new FTPConnection();
            ftpConnection.ServerAddress = location.FtpServerDetail.FtpServerIp;
            ftpConnection.UserName = location.FtpServerDetail.FtpUser;
            ftpConnection.Password = location.FtpServerDetail.FtpPassword;

            return this.fileTransferProtocols.GetFilesFromSource(ftpConnection, location);
        }

        [MaintainabilityDetails(ApprovedIndex = 57, ReviewedOnSprint = 94, ReviewerName = "Josue Chavez")]
        private List<FileBasicInfo> GetFileListFromFtpsSource(SourceLocationModel location)
        {
            SecureFTPConnection ftpConnection = new SecureFTPConnection();
            ftpConnection.ServerAddress = location.FtpServerDetail.FtpServerIp;
            ftpConnection.UserName = location.FtpServerDetail.FtpUser;
            ftpConnection.Password = location.FtpServerDetail.FtpPassword;

            string? ftpsConnectionProtocol = this.configurationManager.AppSettings[AppSettingsConstants.FtpsConnectionProtocol];
            if (!string.IsNullOrEmpty(ftpsConnectionProtocol))
            {
                ftpConnection.Protocol = this.ParseEnum<FileTransferProtocol>(ftpsConnectionProtocol);
            }
            else
            {
                ftpConnection.Protocol = FileTransferProtocol.FTPSExplicit;
            }

            ftpConnection.ServerPort = location.FtpServerDetail.FtpPort;

            string? ftpConnectMode = this.configurationManager.AppSettings[AppSettingsConstants.FTPConnectMode];
            if (!string.IsNullOrEmpty(ftpConnectMode))
            {
                ftpConnection.ConnectMode = this.ParseEnum<FTPConnectMode>(ftpConnectMode);
            }

            return this.fileTransferProtocols.GetFilesFromSource(ftpConnection, location);
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

            return this.fileTransferProtocols.GetFilesFromSource(sftpConnection, location);
        }

        private T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
        #endregion

        #region Private Download Methods
        private bool DownloadFileFromFtp(SourceLocationModel sourceLocation, string remoteFileName, DestinationModel destinationLocation)
        {
            FTPConnection remoteServer = new FTPConnection()
            {
                ServerAddress = sourceLocation.FtpServerDetail.FtpServerIp,
                ServerDirectory = sourceLocation.SourceAddress,
                UserName = sourceLocation.FtpServerDetail.FtpUser,
                Password = sourceLocation.FtpServerDetail.FtpPassword,
                ServerPort = sourceLocation.FtpServerDetail.FtpPort
            };
            this.fileTransferProtocols.DownloadFromFtpConnection(remoteServer, sourceLocation, remoteFileName, destinationLocation, this.TenantId);

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
                ServerCompatibility = SecureFTPCompatibilityFlags.SSHDisableChmodAfterPut
            };

            this.fileTransferProtocols.DownloadFromFtpConnection(remoteServer, sourceLocation, remoteFileName, destinationLocation, this.TenantId);
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
                ServerPort = sourceLocation.FtpServerDetail.FtpPort
            };

            if (!string.IsNullOrEmpty(this.configurationManager.AppSettings?[AppSettingsConstants.FtpsConnectionProtocol]))
            {
                remoteServer.Protocol = this.ParseEnum<FileTransferProtocol>(this.configurationManager.AppSettings?[AppSettingsConstants.FtpsConnectionProtocol]);
            }
            else
            {
                remoteServer.Protocol = FileTransferProtocol.FTPSExplicit;
            }

            this.fileTransferProtocols.DownloadFromFtpConnection(remoteServer, sourceLocation, remoteFileName, destinationLocation, this.TenantId);

            return true;
        }
        #endregion

        #region Private Upload Methods
        private bool UploadFileToFtp(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation)
        {
            FTPConnection remoteServer = new FTPConnection()
            {
                ServerAddress = destinationLocation.FtpServerDetail.FtpServerIp,
                ServerDirectory = destinationLocation.DestinationAddress,
                UserName = destinationLocation.FtpServerDetail.FtpUser,
                Password = destinationLocation.FtpServerDetail.FtpPassword,
                ServerPort = destinationLocation.FtpServerDetail.FtpPort
            };

            return this.fileTransferProtocols.UploadToFTPServer(fullyQualifiedSourceFile, fullyQualifiedDestinationFile, destinationLocation, remoteServer);
        }

        private bool UploadFiletoSftp(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation)
        {
            // Legacy used `using (remoteServer = new SecureFTPConnection())`; this target `SecureFTPConnection` is not IDisposable.
            SecureFTPConnection remoteServer = new SecureFTPConnection();
            remoteServer.ServerAddress = destinationLocation.FtpServerDetail.FtpServerIp;
            remoteServer.ServerDirectory = destinationLocation.DestinationAddress;
            remoteServer.UserName = destinationLocation.FtpServerDetail.FtpUser;
            remoteServer.Password = destinationLocation.FtpServerDetail.FtpPassword;
            remoteServer.ServerPort = destinationLocation.FtpServerDetail.FtpPort;
            remoteServer.Protocol = FileTransferProtocol.SFTP;
            remoteServer.ServerCompatibility = SecureFTPCompatibilityFlags.SSHDisableChmodAfterPut;

            return this.fileTransferProtocols.UploadToFTPServer(fullyQualifiedSourceFile, fullyQualifiedDestinationFile, destinationLocation, remoteServer);
        }

        private bool UploadFiletoFtps(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation)
        {
            SecureFTPConnection remoteServer = new SecureFTPConnection()
            {
                ServerAddress = destinationLocation.FtpServerDetail.FtpServerIp,
                ServerDirectory = destinationLocation.DestinationAddress,
                UserName = destinationLocation.FtpServerDetail.FtpUser,
                Password = destinationLocation.FtpServerDetail.FtpPassword,
                ServerPort = destinationLocation.FtpServerDetail.FtpPort
            };

            string? ftpsConnectionProtocol = this.configurationManager.AppSettings?[AppSettingsConstants.FtpsConnectionProtocol];

            if (!string.IsNullOrEmpty(ftpsConnectionProtocol))
            {
                remoteServer.Protocol = this.ParseEnum<FileTransferProtocol>(ftpsConnectionProtocol);
            }
            else
            {
                remoteServer.Protocol = FileTransferProtocol.FTPSExplicit;
            }

            return this.fileTransferProtocols.UploadToFTPServer(fullyQualifiedSourceFile, fullyQualifiedDestinationFile, destinationLocation, remoteServer);
        }

        #endregion

        #region Private Delete Methods

        private bool DeleteFileFromFtp(SourceLocationModel source, string fullyQualifiedSourceFileName)
        {
            bool result;

            FTPConnection remoteServer = new FTPConnection()
            {
                ServerAddress = source.FtpServerDetail.FtpServerIp,
                ServerDirectory = source.SourceAddress,
                UserName = source.FtpServerDetail.FtpUser,
                Password = source.FtpServerDetail.FtpPassword,
                ServerPort = source.FtpServerDetail.FtpPort
            };
            result = this.fileTransferProtocols.DeleteFromFtp(remoteServer, fullyQualifiedSourceFileName);

            return result;
        }

        private bool DeleteFileFromFtps(SourceLocationModel source, string fullyQualifiedSourceFileName)
        {
            bool result;

            SecureFTPConnection remoteServer = new SecureFTPConnection()
            {
                ServerAddress = source.FtpServerDetail.FtpServerIp,
                ServerDirectory = source.SourceAddress,
                UserName = source.FtpServerDetail.FtpUser,
                Password = source.FtpServerDetail.FtpPassword,
                ServerPort = source.FtpServerDetail.FtpPort
            };

            if (!string.IsNullOrEmpty(this.configurationManager.AppSettings?[AppSettingsConstants.FtpsConnectionProtocol]))
            {
                remoteServer.Protocol = this.ParseEnum<FileTransferProtocol>(this.configurationManager.AppSettings?[AppSettingsConstants.FtpsConnectionProtocol]);
            }
            else
            {
                remoteServer.Protocol = FileTransferProtocol.FTPSExplicit;
            }

            result = this.fileTransferProtocols.DeleteFromFtp(remoteServer, fullyQualifiedSourceFileName);

            return result;
        }

        private bool DeleteFileFromSftp(SourceLocationModel source, string fullyQualifiedSourceFileName)
        {
            bool result;

            SecureFTPConnection remoteServer = new SecureFTPConnection()
            {
                ServerAddress = source.FtpServerDetail.FtpServerIp,
                ServerDirectory = source.SourceAddress,
                UserName = source.FtpServerDetail.FtpUser,
                Password = source.FtpServerDetail.FtpPassword,
                ServerPort = source.FtpServerDetail.FtpPort,
                Protocol = FileTransferProtocol.SFTP,
                ServerCompatibility = SecureFTPCompatibilityFlags.SSHDisableChmodAfterPut
            };
            result = this.fileTransferProtocols.DeleteFromFtp(remoteServer, fullyQualifiedSourceFileName);

            return result;
        }
        #endregion

        #region Private Rename Methods
        private bool RenameFileFromFtp(SourceLocationModel source, string fullyQualifiedSourceFileName, string renameExtension)
        {
            bool result;

            FTPConnection remoteServer = new FTPConnection()
            {
                ServerAddress = source.FtpServerDetail.FtpServerIp,
                ServerDirectory = source.SourceAddress,
                UserName = source.FtpServerDetail.FtpUser,
                Password = source.FtpServerDetail.FtpPassword,
                ServerPort = source.FtpServerDetail.FtpPort
            };

            result = this.fileTransferProtocols.RenameFtpFile(fullyQualifiedSourceFileName, renameExtension, remoteServer);

            return result;
        }

        private bool RenameFileFromFtps(SourceLocationModel source, string fullyQualifiedSourceFileName, string renameExtension)
        {
            bool result;

            SecureFTPConnection remoteServer = new SecureFTPConnection()
            {
                ServerAddress = source.FtpServerDetail.FtpServerIp,
                ServerDirectory = source.SourceAddress,
                UserName = source.FtpServerDetail.FtpUser,
                Password = source.FtpServerDetail.FtpPassword,
                ServerPort = source.FtpServerDetail.FtpPort,
            };

            if (!string.IsNullOrEmpty(this.configurationManager.AppSettings?[AppSettingsConstants.FtpsConnectionProtocol]))
            {
                remoteServer.Protocol = this.ParseEnum<FileTransferProtocol>(this.configurationManager.AppSettings?[AppSettingsConstants.FtpsConnectionProtocol]);
            }
            else
            {
                remoteServer.Protocol = FileTransferProtocol.FTPSExplicit;
            }

            result = this.fileTransferProtocols.RenameFtpFile(fullyQualifiedSourceFileName, renameExtension, remoteServer);
            return result;
        }

        private bool RenameFileFromSftp(SourceLocationModel source, string fullyQualifiedSourceFileName, string renameExtension)
        {
            bool result;

            SecureFTPConnection remoteServer = new SecureFTPConnection()
            {
                ServerAddress = source.FtpServerDetail.FtpServerIp,
                ServerDirectory = source.SourceAddress,
                UserName = source.FtpServerDetail.FtpUser,
                Password = source.FtpServerDetail.FtpPassword,
                ServerPort = source.FtpServerDetail.FtpPort,
                Protocol = FileTransferProtocol.SFTP,
                ServerCompatibility = SecureFTPCompatibilityFlags.SSHDisableChmodAfterPut
            };

            result = this.fileTransferProtocols.RenameFtpFile(fullyQualifiedSourceFileName, renameExtension, remoteServer);

            return result;
        }
        #endregion

        #region SSH FTP Private Methods
        private bool DownloadFileFromSftpUsingSSHKeys(SourceLocationModel sourceLocation, string remoteFileName, DestinationModel destinationLocation)
        {
            this.fileTransferProtocols.DownloadFromSSHFtpConnection(this.BuildSftpClient(sourceLocation.FtpServerDetail.FtpUser, sourceLocation.FtpServerDetail.FtpPassword, sourceLocation.FtpServerDetail.FtpServerIp, sourceLocation.FtpServerDetail.FtpPort), sourceLocation, remoteFileName, destinationLocation, this.TenantId);
            return true;
        }

        private bool UploadFiletoSSHFtp(string fullyQualifiedSourceFile, string fullyQualifiedDestinationFile, DestinationModel destinationLocation)
        {
            this.fileTransferProtocols.UploadToSSHFTPServer(fullyQualifiedSourceFile, fullyQualifiedDestinationFile, destinationLocation, this.BuildSftpClient(destinationLocation.FtpServerDetail.FtpUser, destinationLocation.FtpServerDetail.FtpPassword, destinationLocation.FtpServerDetail.FtpServerIp, destinationLocation.FtpServerDetail.FtpPort));
            return true;
        }

        private bool DeleteFileFromSSHFtp(SourceLocationModel source, string fullyQualifiedSourceFileName)
        {
            return this.fileTransferProtocols.DeleteFromSFtp(this.BuildSftpClient(source.FtpServerDetail.FtpUser, source.FtpServerDetail.FtpPassword, source.FtpServerDetail.FtpServerIp, source.FtpServerDetail.FtpPort), fullyQualifiedSourceFileName);
        }

        private bool RenameFileFromSSHFtp(SourceLocationModel source, string fullyQualifiedSourceFileName, string renameExtension)
        {
            bool result;

            result = this.fileTransferProtocols.RenameSSHFtpFile(fullyQualifiedSourceFileName, renameExtension, this.BuildSftpClient(source.FtpServerDetail.FtpUser, source.FtpServerDetail.FtpPassword, source.FtpServerDetail.FtpServerIp, source.FtpServerDetail.FtpPort));
            return result;
        }

        private List<FileBasicInfo> GetFileListFromSSHFtpSource(SourceLocationModel location)
        {
            return this.fileTransferProtocols.GetFilesFromSSHSource(this.BuildSftpClient(location.FtpServerDetail.FtpUser, location.FtpServerDetail.FtpPassword, location.FtpServerDetail.FtpServerIp, location.FtpServerDetail.FtpPort), location);
        }

        private SftpClient BuildSftpClient(string ftpUser, string passprhase, string ftpServerIp, int ftpPort)
        {
            PrivateKeyFile pkRsa;
            pkRsa = this.SetPrivateKey(passprhase);

            PrivateKeyAuthenticationMethod authenticationMethodRsa = new PrivateKeyAuthenticationMethod(ftpUser, pkRsa);

            Renci.SshNet.ConnectionInfo conn = new Renci.SshNet.ConnectionInfo(ftpServerIp, ftpPort, ftpUser, authenticationMethodRsa);

            using (SftpClient sftpClient = new SftpClient(conn))
            {
                RsaSha256Util.SetupConnection(conn);
                return new SftpClient(conn);
            }
        }

        private PrivateKeyFile SetPrivateKey(string passprhase)
        {
            PrivateKeyFile pkRsa;
            var executingAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            if (executingAssembly == null)
            {
                throw new InvalidOperationException("Could not get executing assembly.");
            }
            string pathToKeyFile = AppContext.BaseDirectory;

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
        #endregion
    }

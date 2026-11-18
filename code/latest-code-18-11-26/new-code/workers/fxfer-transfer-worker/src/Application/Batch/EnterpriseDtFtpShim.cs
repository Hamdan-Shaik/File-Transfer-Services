using System.Net;

using FluentFTP;

using Renci.SshNet;

namespace EnterpriseDT.Net.Ftp;

public enum FTPConnectMode
{
    PASV,
    PORT
}

public enum FileTransferProtocol
{
    FTP,
    FTPSExplicit,
    FTPSImplicit,
    SFTP
}

[Flags]
public enum SecureFTPCompatibilityFlags
{
    SSHDisableChmodAfterPut = 1
}

public sealed class FTPFile
{
    public bool Dir { get; init; }

    public string Name { get; init; } = string.Empty;

    public long Size { get; init; }
}

public class FTPConnection : IDisposable
{
    private FtpClient? _ftpClient;
    private SftpClient? _sftpClient;

    public string? ServerAddress { get; set; }

    public string? ServerDirectory { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public int ServerPort { get; set; } = 21;

    public FTPConnectMode ConnectMode { get; set; } = FTPConnectMode.PASV;

    public FileTransferProtocol Protocol { get; set; } = FileTransferProtocol.FTP;

    public SecureFTPCompatibilityFlags ServerCompatibility { get; set; }

    public void Connect()
    {
        if (_ftpClient != null || _sftpClient != null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(ServerAddress))
        {
            throw new InvalidOperationException("ServerAddress must be specified before Connect().");
        }

        if (Protocol == FileTransferProtocol.SFTP)
        {
            _sftpClient = new SftpClient(ServerAddress, ServerPort, UserName ?? string.Empty, Password ?? string.Empty);
            _sftpClient.Connect();
            return;
        }

        _ftpClient = new FtpClient();
        _ftpClient.Host = ServerAddress;
        _ftpClient.Port = ServerPort;
        _ftpClient.Credentials = new NetworkCredential(UserName ?? string.Empty, Password ?? string.Empty);
        _ftpClient.Config.DataConnectionType = ConnectMode == FTPConnectMode.PASV
            ? FtpDataConnectionType.PASV
            : FtpDataConnectionType.AutoActive;

        _ftpClient.Config.EncryptionMode = Protocol switch
        {
            FileTransferProtocol.FTPSImplicit => FtpEncryptionMode.Implicit,
            FileTransferProtocol.FTPSExplicit => FtpEncryptionMode.Explicit,
            _ => FtpEncryptionMode.None,
        };

        _ftpClient.Connect();

        if (!string.IsNullOrWhiteSpace(ServerDirectory))
        {
            _ftpClient.SetWorkingDirectory(NormalizeRemotePath(ServerDirectory));
        }
    }

    public void Close()
    {
        try
        {
            _ftpClient?.Disconnect();
        }
        finally
        {
            _ftpClient?.Dispose();
            _ftpClient = null;
        }

        try
        {
            _sftpClient?.Disconnect();
        }
        finally
        {
            _sftpClient?.Dispose();
            _sftpClient = null;
        }
    }

    public void Dispose()
    {
        Close();
    }

    public void DownloadFile(string localPath, string remoteFileName)
    {
        if (_ftpClient != null)
        {
            _ftpClient.DownloadFile(localPath, ResolveRemotePath(remoteFileName));
            return;
        }

        if (_sftpClient != null)
        {
            using var output = File.Open(localPath, FileMode.Create, FileAccess.Write);
            _sftpClient.DownloadFile(ResolveRemotePath(remoteFileName), output);
            return;
        }

        throw new InvalidOperationException("Connect() must be called before DownloadFile().");
    }

    public void UploadFile(string localPath, string remotePath)
    {
        if (_ftpClient != null)
        {
            _ftpClient.UploadFile(localPath, ResolveRemotePath(remotePath));
            return;
        }

        if (_sftpClient != null)
        {
            using var input = File.OpenRead(localPath);
            _sftpClient.UploadFile(input, ResolveRemotePath(remotePath), canOverride: true);
            return;
        }

        throw new InvalidOperationException("Connect() must be called before UploadFile().");
    }

    public bool DeleteFile(string remotePath)
    {
        if (_ftpClient != null)
        {
            _ftpClient.DeleteFile(ResolveRemotePath(remotePath));
            return true;
        }

        if (_sftpClient != null)
        {
            var resolved = ResolveRemotePath(remotePath);
            if (_sftpClient.Exists(resolved))
            {
                _sftpClient.DeleteFile(resolved);
            }

            return true;
        }

        throw new InvalidOperationException("Connect() must be called before DeleteFile().");
    }

    public bool RenameFile(string remoteSourcePath, string remoteTargetPath)
    {
        if (_ftpClient != null)
        {
            _ftpClient.Rename(ResolveRemotePath(remoteSourcePath), ResolveRemotePath(remoteTargetPath));
            return true;
        }

        if (_sftpClient != null)
        {
            _sftpClient.RenameFile(ResolveRemotePath(remoteSourcePath), ResolveRemotePath(remoteTargetPath));
            return true;
        }

        throw new InvalidOperationException("Connect() must be called before RenameFile().");
    }

    public bool Exists(string remotePath)
    {
        if (_ftpClient != null)
        {
            return _ftpClient.FileExists(ResolveRemotePath(remotePath));
        }

        if (_sftpClient != null)
        {
            return _sftpClient.Exists(ResolveRemotePath(remotePath));
        }

        throw new InvalidOperationException("Connect() must be called before Exists().");
    }

    public bool DirectoryExists(string remoteDirectory)
    {
        if (_ftpClient != null)
        {
            return _ftpClient.DirectoryExists(ResolveRemotePath(remoteDirectory));
        }

        if (_sftpClient != null)
        {
            var resolved = ResolveRemotePath(remoteDirectory);
            if (!_sftpClient.Exists(resolved))
            {
                return false;
            }

            var attrs = _sftpClient.GetAttributes(resolved);
            return attrs.IsDirectory;
        }

        throw new InvalidOperationException("Connect() must be called before DirectoryExists().");
    }

    public void CreateDirectory(string remoteDirectory)
    {
        if (_ftpClient != null)
        {
            _ftpClient.CreateDirectory(ResolveRemotePath(remoteDirectory));
            return;
        }

        if (_sftpClient != null)
        {
            _sftpClient.CreateDirectory(ResolveRemotePath(remoteDirectory));
            return;
        }

        throw new InvalidOperationException("Connect() must be called before CreateDirectory().");
    }

    public FTPFile[] GetFileInfos(string remoteDirectory)
    {
        if (_ftpClient != null)
        {
            var listing = _ftpClient.GetListing(ResolveRemotePath(remoteDirectory));
            return listing.Select(item => new FTPFile
            {
                Dir = item.Type == FtpObjectType.Directory,
                Name = item.Name,
                Size = item.Size,
            }).ToArray();
        }

        if (_sftpClient != null)
        {
            var listing = _sftpClient.ListDirectory(ResolveRemotePath(remoteDirectory))
                .Where(item => item.Name != "." && item.Name != "..");
            return listing.Select(item => new FTPFile
            {
                Dir = item.IsDirectory,
                Name = item.Name,
                Size = item.Attributes.Size,
            }).ToArray();
        }

        throw new InvalidOperationException("Connect() must be called before GetFileInfos().");
    }

    private string ResolveRemotePath(string remotePath)
    {
        var normalized = NormalizeRemotePath(remotePath);

        if (string.IsNullOrWhiteSpace(ServerDirectory))
        {
            return normalized;
        }

        if (normalized.StartsWith("/", StringComparison.Ordinal))
        {
            return normalized;
        }

        if (normalized.Contains('/'))
        {
            return normalized;
        }

        return NormalizeRemotePath(ServerDirectory) + "/" + normalized;
    }

    private static string NormalizeRemotePath(string path)
    {
        return path.Replace('\\', '/');
    }
}

public sealed class SecureFTPConnection : FTPConnection
{
}


using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;

namespace Fxfer.ExecutionWorker.Infrastructure.Protocols;

public class FTPConnection : IDisposable
{
    public string ServerAddress { get; set; } = string.Empty;

    public string ServerDirectory { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public int ServerPort { get; set; }

    public FTPConnectMode ConnectMode { get; set; }

    public virtual void Connect()
    {
        // shortcut: we use per-request FTP connections (FtpWebRequest).
    }

    public virtual void Close()
    {
        // shortcut: we use per-request FTP connections (FtpWebRequest).
    }

    public void Dispose()
    {
        Close();
        GC.SuppressFinalize(this);
    }

    public virtual bool Exists(string remotePath)
    {
        try
        {
            using var response = (FtpWebResponse)CreateRequest(remotePath, WebRequestMethods.Ftp.GetFileSize).GetResponse();
            return response.StatusCode is FtpStatusCode.FileStatus or FtpStatusCode.CommandOK;
        }
        catch (WebException ex) when (IsNotFound(ex))
        {
            return false;
        }
    }

    public virtual bool DirectoryExists(string remotePath)
    {
        try
        {
            using var response = (FtpWebResponse)CreateRequest(remotePath, WebRequestMethods.Ftp.ListDirectory).GetResponse();
            return response.StatusCode is FtpStatusCode.OpeningData or FtpStatusCode.DataAlreadyOpen;
        }
        catch (WebException ex) when (IsNotFound(ex))
        {
            return false;
        }
    }

    public virtual void CreateDirectory(string remotePath)
    {
        using var response = (FtpWebResponse)CreateRequest(remotePath, WebRequestMethods.Ftp.MakeDirectory).GetResponse();
        _ = response.StatusDescription;
    }

    public virtual bool DeleteFile(string remotePath)
    {
        try
        {
            using var response = (FtpWebResponse)CreateRequest(remotePath, WebRequestMethods.Ftp.DeleteFile).GetResponse();
            return response.StatusCode is FtpStatusCode.FileActionOK or FtpStatusCode.CommandOK;
        }
        catch (WebException ex) when (IsNotFound(ex))
        {
            return false;
        }
    }

    public virtual bool RenameFile(string remoteSourcePath, string remoteDestinationPath)
    {
        var request = CreateRequest(remoteSourcePath, WebRequestMethods.Ftp.Rename);

        // Many servers accept an absolute path; others accept a simple name. Keep legacy semantics.
        request.RenameTo = remoteDestinationPath;

        using var response = (FtpWebResponse)request.GetResponse();
        return response.StatusCode is FtpStatusCode.FileActionOK or FtpStatusCode.CommandOK;
    }

    public virtual void DownloadFile(string localFilePath, string remoteFileNameOrPath)
    {
        var request = CreateRequest(remoteFileNameOrPath, WebRequestMethods.Ftp.DownloadFile);
        using var response = (FtpWebResponse)request.GetResponse();
        using var responseStream = response.GetResponseStream();

        if (responseStream is null)
        {
            throw new IOException("FTP server returned an empty response stream.");
        }

        Directory.CreateDirectory(Path.GetDirectoryName(localFilePath) ?? ".");
        using var fileStream = File.Create(localFilePath);
        responseStream.CopyTo(fileStream);
    }

    public virtual void UploadFile(string localFilePath, string remoteFileNameOrPath)
    {
        var request = CreateRequest(remoteFileNameOrPath, WebRequestMethods.Ftp.UploadFile);

        using (var requestStream = request.GetRequestStream())
        using (var fileStream = File.OpenRead(localFilePath))
        {
            fileStream.CopyTo(requestStream);
        }

        using var response = (FtpWebResponse)request.GetResponse();
        _ = response.StatusDescription;
    }

    public virtual IReadOnlyList<FTPFile> GetFileInfos(string remoteDirectory)
    {
        var request = CreateRequest(remoteDirectory, WebRequestMethods.Ftp.ListDirectoryDetails);
        using var response = (FtpWebResponse)request.GetResponse();
        using var stream = response.GetResponseStream();

        if (stream is null)
        {
            return Array.Empty<FTPFile>();
        }

        using var reader = new StreamReader(stream);
        var files = new List<FTPFile>();

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (TryParseUnixListLine(line, out var unixFile))
            {
                files.Add(unixFile);
                continue;
            }

            if (TryParseWindowsListLine(line, out var windowsFile))
            {
                files.Add(windowsFile);
                continue;
            }

            // Fallback: treat as a file name.
            files.Add(new FTPFile(line.Trim(), dir: false, size: 0));
        }

        return files;
    }

    protected virtual bool UseSsl
    {
        get
        {
            if (this is not SecureFTPConnection secure)
            {
                return false;
            }

            return secure.Protocol is FileTransferProtocol.FTPSExplicit or FileTransferProtocol.FTPSImplicit;
        }
    }

    private FtpWebRequest CreateRequest(string remotePathOrName, string method)
    {
        var uri = BuildUri(remotePathOrName);

#pragma warning disable SYSLIB0014 // WebRequest is obsolete; FTP is a legacy protocol and we keep this minimal.
        var request = (FtpWebRequest)WebRequest.Create(uri);
#pragma warning restore SYSLIB0014

        request.Method = method;
        request.Credentials = new NetworkCredential(UserName, Password);
        request.UseBinary = true;
        request.KeepAlive = false;
        request.EnableSsl = UseSsl;

        // Legacy default is passive.
        request.UsePassive = true;

        return request;
    }

    private Uri BuildUri(string remotePathOrName)
    {
        var port = ServerPort > 0 ? ServerPort : 21;

        var path = remotePathOrName;
        if (!path.StartsWith("/", StringComparison.Ordinal) && !string.IsNullOrEmpty(ServerDirectory))
        {
            path = ServerDirectory.TrimEnd('/') + "/" + path;
        }

        if (!path.StartsWith("/", StringComparison.Ordinal))
        {
            path = "/" + path;
        }

        var builder = new UriBuilder("ftp", ServerAddress, port, path);
        return builder.Uri;
    }

    private static bool IsNotFound(WebException ex)
    {
        if (ex.Response is not FtpWebResponse ftp)
        {
            return false;
        }

        return ftp.StatusCode is FtpStatusCode.ActionNotTakenFileUnavailable or FtpStatusCode.ActionNotTakenFilenameNotAllowed;
    }

    private static bool TryParseUnixListLine(string line, out FTPFile file)
    {
        // Example: -rw-r--r-- 1 user group 1234 Jan 01 12:00 name.txt
        file = null!;
        if (line.Length < 10)
        {
            return false;
        }

        var first = line[0];
        if (first is not ('d' or '-')
            || (line.Length > 1 && line[1] is not ('r' or 'w' or 'x' or '-')))
        {
            return false;
        }

        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 9)
        {
            return false;
        }

        var isDir = parts[0].StartsWith('d');
        if (!long.TryParse(parts[4], NumberStyles.Integer, CultureInfo.InvariantCulture, out var size))
        {
            size = 0;
        }

        var name = parts[^1];
        file = new FTPFile(name, dir: isDir, size: size);
        return true;
    }

    private static bool TryParseWindowsListLine(string line, out FTPFile file)
    {
        // Example: 11-08-26  02:15PM       <DIR>          folder
        file = null!;
        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 4)
        {
            return false;
        }

        if (!DateTime.TryParseExact(parts[0], new[] { "MM-dd-yy", "MM-dd-yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            return false;
        }

        var isDir = string.Equals(parts[2], "<DIR>", StringComparison.OrdinalIgnoreCase);
        var name = parts[^1];

        long size = 0;
        if (!isDir)
        {
            _ = long.TryParse(parts[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out size);
        }

        file = new FTPFile(name, dir: isDir, size: size);
        return true;
    }
}
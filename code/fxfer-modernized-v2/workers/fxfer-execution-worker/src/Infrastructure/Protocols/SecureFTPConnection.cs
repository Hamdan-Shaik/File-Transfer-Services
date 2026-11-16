namespace Fxfer.ExecutionWorker.Infrastructure.Protocols;

public class SecureFTPConnection : FTPConnection
{
    public FileTransferProtocol Protocol { get; set; }

    public SecureFTPCompatibilityFlags ServerCompatibility { get; set; }
}

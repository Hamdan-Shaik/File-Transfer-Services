namespace Fxfer.TransferWorker.Application.Batch;

public static class RsaSha256Util
{
    // shortcut: legacy called into an external helper; SSH.NET 2024 already supports RSA-SHA2.
    public static void SetupConnection(Renci.SshNet.ConnectionInfo connectionInfo)
    {
    }

    public static void ConvertToKeyWithSha256Signature(Renci.SshNet.PrivateKeyFile privateKeyFile)
    {
    }
}


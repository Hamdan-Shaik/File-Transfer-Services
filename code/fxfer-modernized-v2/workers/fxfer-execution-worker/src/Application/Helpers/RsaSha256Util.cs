using Renci.SshNet;

namespace Fxfer.ExecutionWorker.Application.Helpers;

internal static class RsaSha256Util
{
    // shortcut: legacy used a helper to force RSA-SHA256 algorithms; SSH.NET 2024 supports them natively.
    public static void SetupConnection(ConnectionInfo connection)
    {
    }

    public static void ConvertToKeyWithSha256Signature(PrivateKeyFile key)
    {
    }
}

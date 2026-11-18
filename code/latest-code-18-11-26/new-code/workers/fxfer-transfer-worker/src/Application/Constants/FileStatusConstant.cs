namespace Fxfer.TransferWorker.Application.Constants;

public static class FileStatusConstant
{
    public const string Completed = "1";
    public const string Error = "2";
    public const string Transfering = "3";
    // F05-025: Legacy movement result split uses UNCProtocolCD to separate UNC vs non-UNC cases.
    public const string UNCProtocolCD = "3";
}


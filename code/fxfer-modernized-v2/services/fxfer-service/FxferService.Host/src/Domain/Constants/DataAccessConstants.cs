namespace FxferService.Domain.Constants;

public static class DataAccessConstants
{
    public const string DefaultConnection = "DefaultConnection";
    public const string DefaultConnectionProvider = "DefaultConnectionProvider";
    public const string DefaultConnectionString = "DefaultConnectionString";

    public static class FileStatusConstant
    {
        public const string Completed = "1";
        public const string Error = "2";
        public const string Transfering = "3";
        public const string UNCProtocolCD = "3";
    }
}

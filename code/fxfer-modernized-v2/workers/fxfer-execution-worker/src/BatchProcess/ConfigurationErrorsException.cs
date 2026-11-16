namespace Fxfer.ExecutionWorker.BatchProcess;

public sealed class ConfigurationErrorsException : Exception
{
    public ConfigurationErrorsException(string message)
        : base(message)
    {
    }
}

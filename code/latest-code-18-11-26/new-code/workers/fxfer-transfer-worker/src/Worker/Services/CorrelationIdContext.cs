namespace Fxfer.TransferWorker.Worker.Services;

public sealed class CorrelationIdContext
{
    private static readonly AsyncLocal<string?> AmbientCorrelationId = new();

    public string? Get()
    {
        return AmbientCorrelationId.Value;
    }

    public void Set(string correlationId)
    {
        AmbientCorrelationId.Value = correlationId;
    }
}


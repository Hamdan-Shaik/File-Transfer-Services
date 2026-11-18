using System.Threading;

namespace Fxfer.ProjectionWorker.Worker.Processing;

public sealed class ProcessingContextAccessor
{
    private static readonly AsyncLocal<ProcessingContext?> CurrentContext = new();

    public ProcessingContext? Current
    {
        get => CurrentContext.Value;
        set => CurrentContext.Value = value;
    }
}


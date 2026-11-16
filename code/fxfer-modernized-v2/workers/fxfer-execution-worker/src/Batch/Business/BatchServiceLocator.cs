using System;

namespace Fxfer.ExecutionWorker.Batch.Business;

public static class BatchServiceLocator
{
    public static IServiceProvider? ServiceProvider { get; set; }

    public static T GetRequiredService<T>() where T : notnull
    {
        if (ServiceProvider is null)
        {
            throw new InvalidOperationException($"{nameof(BatchServiceLocator)}.{nameof(ServiceProvider)} is not configured.");
        }

        var service = ServiceProvider.GetService(typeof(T));
        if (service is not T typed)
        {
            throw new InvalidOperationException($"Required service is not available: {typeof(T).FullName}");
        }

        return typed;
    }
}
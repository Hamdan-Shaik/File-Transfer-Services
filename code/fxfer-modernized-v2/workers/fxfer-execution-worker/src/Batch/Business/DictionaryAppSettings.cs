using System;
using System.Collections.Generic;

namespace Fxfer.ExecutionWorker.Batch.Business;

public sealed class DictionaryAppSettings : IAppSettings
{
    private readonly IReadOnlyDictionary<string, string?> _settings;

    public DictionaryAppSettings(IReadOnlyDictionary<string, string?> settings)
    {
        _settings = settings;
    }

    public string? this[string key]
    {
        get
        {
            return _settings.TryGetValue(key, out var value) ? value : null;
        }
    }
}
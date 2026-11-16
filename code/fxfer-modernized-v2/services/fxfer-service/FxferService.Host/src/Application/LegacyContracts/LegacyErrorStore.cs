namespace FxferService.Application.LegacyContracts;

internal static class LegacyErrorStore
{
    private static readonly AsyncLocal<Exception?> _lastError = new();

    public static Exception? LastError
    {
        get => _lastError.Value;
        set => _lastError.Value = value;
    }
}


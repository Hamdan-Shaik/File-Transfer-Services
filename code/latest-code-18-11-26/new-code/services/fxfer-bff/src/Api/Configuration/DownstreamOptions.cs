namespace Fxfer.Bff.Api.Configuration;

public sealed class DownstreamOptions
{
    public string? FxferDomainApiBaseUrl { get; init; }
    public int TimeoutSeconds { get; init; } = 30;
    public int MaxRetries { get; init; } = 2;
}


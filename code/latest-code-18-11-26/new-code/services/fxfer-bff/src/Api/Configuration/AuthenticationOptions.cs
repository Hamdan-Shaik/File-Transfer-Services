namespace Fxfer.Bff.Api.Configuration;

public sealed class AuthenticationOptions
{
    public string? Authority { get; init; }
    public string? Audience { get; init; }
    public bool RequireHttpsMetadata { get; init; } = true;
}


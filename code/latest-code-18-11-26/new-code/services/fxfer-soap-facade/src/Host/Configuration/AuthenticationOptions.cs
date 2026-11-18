namespace Fxfer.SoapFacade.Host.Configuration;

public sealed class AuthenticationOptions
{
    public string? Authority { get; set; }
    public string? Audience { get; set; }
    public bool RequireHttpsMetadata { get; set; } = true;
}
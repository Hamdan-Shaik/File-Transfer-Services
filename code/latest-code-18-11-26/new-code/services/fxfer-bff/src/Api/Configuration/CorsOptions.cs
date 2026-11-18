namespace Fxfer.Bff.Api.Configuration;

public sealed class CorsOptions
{
    public string AllowedOriginsCsv { get; init; } = "";

    public static string[] ParseCsv(string? csv)
    {
        if (string.IsNullOrWhiteSpace(csv))
        {
            return Array.Empty<string>();
        }

        return csv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }
}


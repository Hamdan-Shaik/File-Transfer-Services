namespace Fxfer.Bff.Host.Middleware;

public sealed record ErrorDetail(string field, string message, string code);

public sealed record ErrorEnvelope(
    string code,
    string message,
    ErrorDetail[] details,
    DateTimeOffset timestamp,
    string trace_id);

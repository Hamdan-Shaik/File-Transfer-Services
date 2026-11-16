namespace FxferService.Host.Composition;

public sealed record ErrorEnvelope(
    string code,
    string message,
    ErrorDetail[] details,
    DateTimeOffset timestamp,
    string trace_id);


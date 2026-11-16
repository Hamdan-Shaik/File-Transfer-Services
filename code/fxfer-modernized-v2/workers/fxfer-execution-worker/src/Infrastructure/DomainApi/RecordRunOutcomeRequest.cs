using System.Text.Json.Serialization;

namespace Fxfer.ExecutionWorker.Infrastructure.DomainApi;

public sealed record RecordRunOutcomeRequest(
    [property: JsonPropertyName("run_id")] Guid RunId,
    [property: JsonPropertyName("outcome_code")] string OutcomeCode,
    [property: JsonPropertyName("occurred_at")] DateTimeOffset OccurredAt);

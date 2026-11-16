using System;
using Xunit;

namespace Fxfer.ExecutionWorker.UnitTests;

public sealed class IdempotencyKeyTests
{
    [Fact]
    public void DeriveIdempotencyKey_IsDeterministic()
    {
        var tenantId = "t-1";
        var runId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        var a = TransferRunProcessor.DeriveIdempotencyKey(tenantId, runId, attemptId: null, stepId: null);
        var b = TransferRunProcessor.DeriveIdempotencyKey(tenantId, runId, attemptId: null, stepId: null);

        Assert.Equal(a, b);
        Assert.NotEmpty(a);
    }
}

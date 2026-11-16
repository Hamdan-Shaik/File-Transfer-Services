namespace Fxfer.ExecutionWorker.Infrastructure.Staging;

public sealed class StagingWorkspace
{
    public string CreateTenantScopedPath(string tenantId)
    {
        // shortcut: local filesystem temp staging; upgrade to encrypted-at-rest managed staging per target-state.
        var root = Path.Combine(Path.GetTempPath(), "fxfer", "staging", tenantId);
        Directory.CreateDirectory(root);
        return root;
    }
}

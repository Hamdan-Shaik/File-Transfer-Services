using Fxfer.Bff.Contracts.Common;

namespace Fxfer.Bff.Application.Providers;

public sealed class DataListsClient
{
    public Task<IReadOnlyList<DataListItem>> GetItemsAsync(string contentId, string? startsWith, string? localeId, CancellationToken cancellationToken)
    {
        _ = (contentId, startsWith, localeId, cancellationToken);
        return Task.FromResult<IReadOnlyList<DataListItem>>(Array.Empty<DataListItem>());
    }
}


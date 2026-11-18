using Fxfer.DomainApi.Contracts.Utilities;

namespace Fxfer.DomainApi.Contracts.Queries.Parameters;

public sealed class FtpServerQueryParms
{
    public bool? IsValid { get; set; }

    public Guid FtpServerId { get; set; }

    public string? FtpServerName { get; set; }

    public string? FtpDescription { get; set; }

    public string? FtpUser { get; set; }

    public List<SearchModeOption>? SearchOptions { get; set; }
}

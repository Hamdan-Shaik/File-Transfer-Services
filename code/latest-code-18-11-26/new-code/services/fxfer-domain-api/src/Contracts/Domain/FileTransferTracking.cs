namespace Fxfer.DomainApi.Contracts.Domain;

/// <summary>
/// <para>Object containing properties of a File Tracking record.</para>
/// </summary>
public sealed record FileTransferTracking
{
    /// <summary>
    /// System generated unique identifier for the file tracking record.
    /// </summary>
    public required Guid FileId { get; init; }
    public Guid FileSourceLocationId { get; init; }

    /// <summary>
    /// Name of the file.
    /// </summary>
    public required string FileName { get; init; }
    public required string FileStatus { get; init; }
    public required string FileDirection { get; init; }
    public required string FileTransportType { get; init; }
    public DateTimeOffset CreatedDate { get; init; }
    public required string TradingPartner { get; init; }
    public required string CheckSumCode { get; init; }
    public decimal FileSize { get; init; }

    /// <summary>
    /// Address of the source location.
    /// </summary>
    public required string SourceLocAddress { get; init; }
    public required string SourceLocConfigName { get; init; }
    public required string OriginatingServerName { get; init; }
    public required IReadOnlyList<FileEvent> Events { get; init; }
    public required IReadOnlyList<FileException> Exceptions { get; init; }
}


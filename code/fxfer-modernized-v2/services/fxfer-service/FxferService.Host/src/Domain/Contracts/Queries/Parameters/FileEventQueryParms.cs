using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Queries.Parameters;

/// <summary>
/// <para>Query parameters for File Event</para>
/// </summary>
[DataContract]
public class FileEventQueryParms : SerializableBase
{
    public string FileId { get; set; } = null!;
}


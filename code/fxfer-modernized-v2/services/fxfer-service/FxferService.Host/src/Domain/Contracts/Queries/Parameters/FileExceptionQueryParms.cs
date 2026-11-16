using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Queries.Parameters;

/// <summary>
/// <para>Query parameters for File Exception</para>
/// </summary>
[DataContract]
public class FileExceptionQueryParms : SerializableBase
{
    public string FileId { get; set; } = null!;
}


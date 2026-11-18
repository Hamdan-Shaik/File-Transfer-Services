using System.IO;

namespace Fxfer.TransferWorker.Domain.Batch;

/// <summary>
/// This class is responsible for getting the basic information for the files to be processed.
/// </summary>
public sealed class FileBasicInfo
{
    public FileBasicInfo(string name, long size)
    {
        Name = name;
        Extension = Path.GetExtension(name);
        Size = size;
    }

    public FileBasicInfo(string name, string extension, long size)
        : this(name, size)
    {
        Extension = extension;
    }

    public string Name { get; private set; }

    public string Extension { get; private set; }

    public long Size { get; private set; }
}

namespace Fxfer.ExecutionWorker.Infrastructure.Protocols;

public sealed class FTPFile
{
    public FTPFile(string name, bool dir, long size)
    {
        Name = name;
        Dir = dir;
        Size = size;
    }

    public string Name { get; }

    public bool Dir { get; }

    public long Size { get; }
}


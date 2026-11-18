using Fxfer.Bff.Contracts.Tracking;

namespace Fxfer.Bff.Tests;

public sealed class FileTransferTrackingContractTests
{
    [Fact]
    public void FileTransferTracking_DefaultsEventsAndExceptionsToEmptyLists()
    {
        var dto = new FileTransferTracking
        {
            FileName = "file.txt",
            FileStatus = "status",
            FileDirection = "direction",
            FileTransportType = "transport",
            SourceLocAddress = "address",
            SourceLocConfigName = "config",
            OriginatingServerName = "server",
        };

        Assert.NotNull(dto.Events);
        Assert.Empty(dto.Events);
        Assert.NotNull(dto.Exceptions);
        Assert.Empty(dto.Exceptions);
    }
}


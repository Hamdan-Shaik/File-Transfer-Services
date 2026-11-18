using Fxfer.TransferWorker.Application.Constants;

namespace Fxfer.TransferWorker.Tests;

public sealed class F05_025_FileStatusConstantTests
{
    [Fact]
    public void F05_025_file_status_constants_match_legacy()
    {
        Assert.Equal("1", FileStatusConstant.Completed);
        Assert.Equal("2", FileStatusConstant.Error);
        Assert.Equal("3", FileStatusConstant.Transfering);
        Assert.Equal("3", FileStatusConstant.UNCProtocolCD);
    }
}

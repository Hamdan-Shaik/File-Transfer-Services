using System.Text;
using Xunit;
using System;
using System.IO;

namespace Fxfer.ExecutionWorker.ContractTests;

public sealed class ContractSmokeTests
{
    [Fact]
    public void AsyncApi_FileExistsAndLooksValid()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "fxfer-execution-worker.asyncapi.yaml");
        Assert.True(File.Exists(path));

        var text = File.ReadAllText(path, Encoding.UTF8);
        Assert.Contains("asyncapi", text);
        Assert.Contains("TransferRunRequested", text);
    }
}

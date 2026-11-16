using System.Text;
using Xunit;

namespace Fxfer.SchedulerWorker.ContractTests;

public sealed class ContractSmokeTests
{
    [Fact]
    public void AsyncApi_FileExistsAndLooksValid()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "fxfer-scheduler-worker.asyncapi.yaml");
        Assert.True(File.Exists(path));

        var text = File.ReadAllText(path, Encoding.UTF8);
        Assert.Contains("asyncapi", text);
        Assert.Contains("TransferRunRequested", text);
    }
}

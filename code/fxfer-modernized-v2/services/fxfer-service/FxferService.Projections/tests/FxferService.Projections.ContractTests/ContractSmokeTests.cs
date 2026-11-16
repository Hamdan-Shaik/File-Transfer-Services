using System.Text;
using Xunit;

namespace FxferService.Projections.ContractTests;

public sealed class ContractSmokeTests
{
    [Fact]
    public void AsyncApi_FileExistsAndLooksValid()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "fxfer-projections.asyncapi.yaml");
        Assert.True(File.Exists(path), $"Missing AsyncAPI contract file at {path}");

        var text = File.ReadAllText(path, Encoding.UTF8);
        Assert.Contains("asyncapi: 3.0.0", text);
        Assert.Contains("Fxfer.FtpServerAdded", text);
    }
}

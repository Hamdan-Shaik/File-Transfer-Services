using FxferService.Infrastructure.Persistence;
using FxferService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Xunit;

namespace FxferService.Tests;

public sealed class F01_004_FtpServerTableMappingTests
{
    [Fact]
    public void F01_004_reads_from_ftp_server_table()
    {
        Environment.SetEnvironmentVariable("Persistence__Schema", null);

        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseNpgsql("Host=localhost;Database=fxfer;Username=fxfer;Password=fxfer")
            .Options;

        using var db = new FxferDbContext(options);

        IEntityType? entityType = db.Model.FindEntityType(typeof(FtpServer));
        Assert.NotNull(entityType);
        Assert.Equal("FTP_SERVER", entityType!.GetTableName());
        Assert.Equal("public", entityType.GetSchema());
    }
}


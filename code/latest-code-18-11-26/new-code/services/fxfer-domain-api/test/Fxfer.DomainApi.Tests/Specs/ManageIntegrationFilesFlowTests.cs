using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.DataAccess.Entities;
using Fxfer.DomainApi.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fxfer.DomainApi.Tests.Specs;

public sealed class ManageIntegrationFilesFlowTests
{
    private static readonly Guid A1 = Guid.Parse("00000000-0000-0000-0000-0000000000a1");
    private static readonly Guid B1 = Guid.Parse("00000000-0000-0000-0000-0000000000b1");
    private static readonly Guid C1 = Guid.Parse("00000000-0000-0000-0000-0000000000c1");

    [Theory]
    [InlineData("/api/fxfer-domain/v1/integration-files", new[] { "Daily export file", "Weekly summary file" })]
    [InlineData("/api/fxfer-domain/v1/integration-files?fileName=Daily", new[] { "Daily export file" })]
    [InlineData("/api/fxfer-domain/v1/integration-files?integrationFileId=00000000-0000-0000-0000-0000000000a1", new[] { "Daily export file" })]
    public async Task Looking_up_active_integration_file_definitions_returns_only_active_matches(string requestUri, string[] expectedNames)
    {
        await using var factory = new FxferDomainApiFactory();
        using var client = factory.CreateClient();

        await SeedAsync(factory,
            BuildIntegrationFile(A1, "Daily export file", isValid: true),
            BuildIntegrationFile(B1, "Weekly summary file", isValid: true),
            BuildIntegrationFile(C1, "Old monthly report file", isValid: false));

        using var response = await client.GetAsync(requestUri);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var document = await ReadJsonAsync(response);
        var names = document.RootElement.EnumerateArray()
            .Select(x => x.GetProperty("fileName").GetString())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToArray();

        Assert.Equal(expectedNames.Length, names.Length);
        Assert.All(expectedNames, expected => Assert.Contains(expected, names));
        Assert.DoesNotContain("Old monthly report file", names);
    }

    [Fact]
    public async Task The_lookup_does_not_promise_ordering_or_time_details()
    {
        await using var factory = new FxferDomainApiFactory();
        using var client = factory.CreateClient();

        await SeedAsync(factory,
            BuildIntegrationFile(A1, "First active", isValid: true),
            BuildIntegrationFile(B1, "Second active", isValid: true),
            BuildIntegrationFile(C1, "Third active", isValid: true));

        using var response = await client.GetAsync("/api/fxfer-domain/v1/integration-files");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var document = await ReadJsonAsync(response);
        foreach (var element in document.RootElement.EnumerateArray())
        {
            Assert.False(element.TryGetProperty("creationDate", out _));
            Assert.False(element.TryGetProperty("lastModifiedDate", out _));
        }
    }

    [Fact]
    public async Task An_update_requires_a_configuration_number()
    {
        await using var factory = new FxferDomainApiFactory();
        using var client = factory.CreateClient();

        await SeedAsync(factory, BuildIntegrationFile(A1, "Daily export file", isValid: true));

        using var response = await client.PatchAsJsonAsync(
            $"/api/fxfer-domain/v1/integration-files/{A1}",
            new { isValid = true });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        using var document = await ReadJsonAsync(response);
        var errors = document.RootElement.GetProperty("errors").EnumerateArray().ToArray();
        Assert.Contains(errors, x => string.Equals(x.GetProperty("message").GetString(), "Missing ConfigId", StringComparison.Ordinal));
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public async Task Recording_processing_marks_a_definition_processed_and_controls_visibility(bool markActive, bool expectVisible)
    {
        await using var factory = new FxferDomainApiFactory();
        using var client = factory.CreateClient();

        await SeedAsync(factory, BuildIntegrationFile(A1, "Daily export file", isValid: true));

        using var patch = await client.PatchAsJsonAsync(
            $"/api/fxfer-domain/v1/integration-files/{A1}",
            new { configId = 123, isValid = markActive });

        Assert.Equal(HttpStatusCode.OK, patch.StatusCode);

        await AssertDbAsync(factory, async db =>
        {
            var row = await db.FtsIntegrationFile.SingleAsync(x => x.FtsIntegrationFileId == A1);
            Assert.True(row.IsProcessed);
            Assert.Equal(123, row.ConfigId);
            Assert.Equal(markActive, row.IsValid);
            Assert.Equal("test-user", row.OperatorId);
        });

        using var lookup = await client.GetAsync("/api/fxfer-domain/v1/integration-files");
        lookup.EnsureSuccessStatusCode();

        using var document = await ReadJsonAsync(lookup);
        var ids = document.RootElement.EnumerateArray().Select(x => x.GetProperty("ftsIntegrationFileId").GetGuid()).ToHashSet();

        if (expectVisible)
        {
            Assert.Contains(A1, ids);
        }
        else
        {
            Assert.DoesNotContain(A1, ids);
        }
    }

    [Fact]
    public async Task Changing_the_name_in_an_update_does_not_change_the_stored_name_and_sets_last_modified_time()
    {
        await using var factory = new FxferDomainApiFactory();
        using var client = factory.CreateClient();

        var seeded = BuildIntegrationFile(A1, "Daily export file", isValid: true) with
        {
            LastModifiedDate = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero)
        };

        await SeedAsync(factory, seeded);

        var startedAt = DateTimeOffset.UtcNow;

        using var patch = await client.PatchAsJsonAsync(
            $"/api/fxfer-domain/v1/integration-files/{A1}",
            new { configId = 123, isValid = true, fileName = "Daily export file renamed" });

        patch.EnsureSuccessStatusCode();

        await AssertDbAsync(factory, async db =>
        {
            var row = await db.FtsIntegrationFile.SingleAsync(x => x.FtsIntegrationFileId == A1);
            Assert.Equal("Daily export file", row.FileName);
            Assert.True(row.LastModifiedDate >= startedAt);
        });
    }

    private static FtsIntegrationFile BuildIntegrationFile(Guid id, string name, bool isValid)
    {
        var now = DateTimeOffset.UtcNow;

        return new FtsIntegrationFile
        {
            FtsIntegrationFileId = id,
            GroupId = Guid.NewGuid(),
            ConfigId = null,
            FileName = name,
            ETFOutboundMetadataDefnId = Guid.NewGuid(),
            IsProcessed = false,
            OperatorId = "seed",
            IsValid = isValid,
            CreationDate = now,
            LastModifiedDate = now
        };
    }

    private static async Task SeedAsync(FxferDomainApiFactory factory, params FtsIntegrationFile[] rows)
    {
        await AssertDbAsync(factory, async db =>
        {
            db.FtsIntegrationFile.AddRange(rows);
            await db.SaveChangesAsync();
        });
    }

    private static async Task AssertDbAsync(FxferDomainApiFactory factory, Func<FxferDbContext, Task> assertion)
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<FxferDbContext>();
        await assertion(dbContext);
    }

    private static async Task<JsonDocument> ReadJsonAsync(HttpResponseMessage response)
    {
        var stream = await response.Content.ReadAsStreamAsync();
        return await JsonDocument.ParseAsync(stream);
    }
}

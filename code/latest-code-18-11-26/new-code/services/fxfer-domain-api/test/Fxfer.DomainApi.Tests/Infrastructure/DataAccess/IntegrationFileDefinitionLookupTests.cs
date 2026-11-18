using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.DataAccess.Entities;

namespace Fxfer.DomainApi.Tests.Infrastructure.DataAccess;

public sealed class IntegrationFileDefinitionLookupTests
{
    [Fact]
    public void F03_009_lookup_maps_only_subset_fields_and_leaves_others_default()
    {
        var now = DateTimeOffset.UtcNow;

        var entity = new FtsIntegrationFile
        {
            FtsIntegrationFileId = Guid.NewGuid(),
            GroupId = Guid.NewGuid(),
            ConfigId = 123,
            FileName = "file.txt",
            ETFOutboundMetadataDefnId = Guid.NewGuid(),
            IsProcessed = true,
            OperatorId = "unit-test",
            IsValid = true,
            CreationDate = now,
            LastModifiedDate = now
        };

        var result = entity.ToIntegrationFileDefinitionLookup();

        Assert.Equal(entity.FtsIntegrationFileId, result.FtsIntegrationFileId);
        Assert.Equal(entity.GroupId, result.GroupId);
        Assert.Equal(entity.ConfigId, result.ConfigId);
        Assert.Equal(entity.FileName, result.FileName);
        Assert.Equal(entity.ETFOutboundMetadataDefnId, result.ETFOutboundMetadataDefnId);
        Assert.Equal(entity.IsProcessed, result.IsProcessed);

        Assert.Null(result.IsValid);
        Assert.Null(result.CreationDate);
        Assert.Null(result.LastModifiedDate);
    }
}


using Fxfer.DomainApi.Contracts.Domain;
using Fxfer.DomainApi.Infrastructure.DataAccess.Entities;

namespace Fxfer.DomainApi.Infrastructure.DataAccess;

public static class IntegrationFileDefinitionLookup
{
    public static IntegrationFileDefinition ToIntegrationFileDefinitionLookup(this FtsIntegrationFile source)
    {
        // F03-009: Lookup returns only a subset of fields; the remaining fields are left at their default values.
        return new IntegrationFileDefinition
        {
            FtsIntegrationFileId = source.FtsIntegrationFileId,
            GroupId = source.GroupId,
            ConfigId = source.ConfigId,
            FileName = source.FileName,
            ETFOutboundMetadataDefnId = source.ETFOutboundMetadataDefnId,
            IsProcessed = source.IsProcessed
        };
    }
}


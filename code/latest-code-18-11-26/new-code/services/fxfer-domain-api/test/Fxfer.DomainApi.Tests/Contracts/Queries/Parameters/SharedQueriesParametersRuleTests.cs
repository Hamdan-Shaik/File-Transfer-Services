using Fxfer.DomainApi.Contracts.Queries.Parameters;

namespace Fxfer.DomainApi.Tests.Contracts.Queries.Parameters;

public sealed class SharedQueriesParametersRuleTests
{
    [Fact]
    public void F01_095_owner_module_guid_parses_owner_subsystem_as_a_guid()
    {
        var parms = new SourceLocationQueryParms
        {
            OwnerSubsystem = "c56a4180-65aa-42ec-a945-5fd21dec0538",
        };

        Assert.Equal(Guid.Parse("c56a4180-65aa-42ec-a945-5fd21dec0538"), parms.OwnerModuleGuid);
    }

    [Fact]
    public void F01_095_owner_module_guid_throws_when_owner_subsystem_is_not_a_guid()
    {
        var parms = new SourceLocationQueryParms
        {
            OwnerSubsystem = "not-a-guid",
        };

        Assert.Throws<FormatException>(() => parms.OwnerModuleGuid);
    }

    [Fact]
    public void F01_101_sourece_location_guid_parses_source_location_id_as_a_guid()
    {
        var parms = new GetSourceLocationQueryParms
        {
            SourceLocationId = "c56a4180-65aa-42ec-a945-5fd21dec0538",
        };

        Assert.Equal(Guid.Parse("c56a4180-65aa-42ec-a945-5fd21dec0538"), parms.SoureceLocationGuid);
    }

    [Fact]
    public void F01_101_sourece_location_guid_throws_when_source_location_id_is_not_a_guid()
    {
        var parms = new GetSourceLocationQueryParms
        {
            SourceLocationId = "not-a-guid",
        };

        Assert.Throws<FormatException>(() => parms.SoureceLocationGuid);
    }

    [Fact]
    public void F01_104_is_active_does_not_affect_source_location_id_parsing()
    {
        var active = new GetSourceLocationQueryParms
        {
            SourceLocationId = "c56a4180-65aa-42ec-a945-5fd21dec0538",
            IsActive = true,
        };

        var inactive = new GetSourceLocationQueryParms
        {
            SourceLocationId = "c56a4180-65aa-42ec-a945-5fd21dec0538",
            IsActive = false,
        };

        Assert.Equal(active.SoureceLocationGuid, inactive.SoureceLocationGuid);
    }

    [Fact]
    public void F03_006_integration_file_query_parms_defaults_integration_file_id_to_guid_empty()
    {
        var parms = new IntegrationFileQueryParms();

        Assert.Equal(Guid.Empty, parms.FTSIntegrationFileId);
    }

    [Fact]
    public void F05_031_inbound_target_query_parms_defaults_metadata_id_to_guid_empty()
    {
        var parms = new InboundTargetQueryParms();

        Assert.Equal(Guid.Empty, parms.FTEMetadataId);
    }
}
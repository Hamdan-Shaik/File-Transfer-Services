using AutoMapper;
using Fxfer.DomainApi.Application.Profiles;
using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Events;

namespace Fxfer.DomainApi.Tests.Application.Profiles;

public sealed class InboundTargetProfileTests
{
    private static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<InboundTargetProfile>();
        });

        return config.CreateMapper();
    }

    [Fact]
    public void FTEMetadataUpdated_Maps_To_AddFtsInboundTargetCommand()
    {
        var mapper = CreateMapper();

        var source = new FTEMetadataUpdated
        {
            UserID = "User",
            ModuleName = "Module",
            ImportJobName = "Import",
            FTEMetadataId = Guid.NewGuid()
        };

        var mapped = mapper.Map<AddFtsInboundTargetCommand>(source);

        Assert.NotNull(mapped.Requestor);
        Assert.Equal("User", mapped.Requestor!.IdentifierId);
        Assert.Equal("Module", mapped.ContractName);
        Assert.Equal("Import", mapped.ImportJobName);
        Assert.Equal(source.FTEMetadataId, mapped.FTEMetadataId);
    }

    [Fact]
    public void FTEMetadataInvalidated_Maps_To_InvalidateFtsInboundTargetCommand()
    {
        var mapper = CreateMapper();

        var source = new FTEMetadataInvalidated
        {
            UserID = "User",
            FTEMetadataId = Guid.NewGuid()
        };

        var mapped = mapper.Map<InvalidateFtsInboundTargetCommand>(source);

        Assert.NotNull(mapped.Requestor);
        Assert.Equal("User", mapped.Requestor!.IdentifierId);
        Assert.Equal(source.FTEMetadataId, mapped.FTEMetadataId);
    }

    [Fact]
    public void FTEMetadataUpdated_Does_Not_Overwrite_Destination_With_Null_Source_Members()
    {
        var mapper = CreateMapper();

        var source = new FTEMetadataUpdated
        {
            UserID = "User",
            ModuleName = null,
            ImportJobName = null,
            FTEMetadataId = Guid.NewGuid()
        };

        var destination = new AddFtsInboundTargetCommand
        {
            ContractName = "ExistingContract",
            ImportJobName = "ExistingImport"
        };

        mapper.Map(source, destination);

        Assert.Equal("ExistingContract", destination.ContractName);
        Assert.Equal("ExistingImport", destination.ImportJobName);
    }
}

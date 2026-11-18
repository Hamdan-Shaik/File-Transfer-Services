using System;
using System.Collections.Generic;
using AutoMapper;
using Fxfer.DomainApi.Application.Profiles;
using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Domain;
using Fxfer.DomainApi.Contracts.Events;

namespace Fxfer.DomainApi.Tests.Application.Profiles;

public sealed class IntegrationFileProfileTests
{
    private static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<IntegrationFileProfile>();
        });

        return config.CreateMapper();
    }

    [Fact]
    public void ExtractFileGenerated_Maps_To_AddIntegrationFileCommand_And_Copies_GroupInfo()
    {
        var mapper = CreateMapper();

        var group = new GroupInfo { GroupId = Guid.NewGuid(), NumberOfRecords = 123 };

        var source = new ExtractFileGenerated
        {
            UserID = "User",
            MetadataId = Guid.NewGuid(),
            FileDirectory = "FileDirectory",
            GroupInfo = new List<GroupInfo> { group }
        };

        var mapped = mapper.Map<AddIntegrationFileCommand>(source);

        Assert.NotNull(mapped.Requestor);
        Assert.Equal("User", mapped.Requestor!.IdentifierId);
        Assert.Equal(source.MetadataId, mapped.ETFOutboundMetadataDefnId);
        Assert.Equal("FileDirectory", mapped.FileName);
        Assert.NotNull(mapped.GroupInfo);
        Assert.Single(mapped.GroupInfo!);
        Assert.NotSame(group, mapped.GroupInfo![0]);
        Assert.Equal(group.GroupId, mapped.GroupInfo[0].GroupId);
        Assert.Equal(group.NumberOfRecords, mapped.GroupInfo[0].NumberOfRecords);
    }

    [Fact]
    public void ExtractFileGenerated_With_Null_GroupInfo_Throws()
    {
        var mapper = CreateMapper();

        var source = new ExtractFileGenerated
        {
            UserID = "User",
            MetadataId = Guid.NewGuid(),
            FileDirectory = "FileDirectory",
            GroupInfo = null
        };

        Assert.Throws<NullReferenceException>(() => mapper.Map<AddIntegrationFileCommand>(source));
    }

    [Fact]
    public void SummaryReportGenerated_Maps_To_AddIntegrationFileCommand_And_Builds_Single_GroupInfo()
    {
        var mapper = CreateMapper();

        var source = new SummaryReportGenerated
        {
            UserID = "User",
            FTEMetadataId = Guid.NewGuid(),
            FileName = "FileName",
            GroupId = Guid.NewGuid(),
            NumberOfRecords = 7
        };

        var mapped = mapper.Map<AddIntegrationFileCommand>(source);

        Assert.NotNull(mapped.Requestor);
        Assert.Equal("User", mapped.Requestor!.IdentifierId);
        Assert.Equal(source.FTEMetadataId, mapped.ETFOutboundMetadataDefnId);
        Assert.Equal("FileName", mapped.FileName);
        Assert.NotNull(mapped.GroupInfo);
        Assert.Single(mapped.GroupInfo!);
        Assert.Equal(source.GroupId, mapped.GroupInfo![0].GroupId);
        Assert.Equal(7, mapped.GroupInfo[0].NumberOfRecords);
    }
}

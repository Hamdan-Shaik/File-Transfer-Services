using System.Collections.Generic;
using AutoMapper;
using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Domain;
using Fxfer.DomainApi.Contracts.Events;

namespace Fxfer.DomainApi.Application.Profiles;

public sealed class IntegrationFileProfile : Profile
{
    public IntegrationFileProfile()
    {
        var extractMap = CreateMap<ExtractFileGenerated, AddIntegrationFileCommand>()
            .ForMember(dest => dest.Requestor,
                opt => opt.MapFrom(src => new HPP.Core.BAS.CQRS.UserMeta.RequestorModel { IdentifierId = src.UserID }))
            .ForMember(dest => dest.ETFOutboundMetadataDefnId, opt => opt.MapFrom(src => src.MetadataId))
            .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileDirectory));

        extractMap.ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember is not null));
        extractMap.AfterMap((src, dest) => dest.GroupInfo = SetGroupInfo(src));

        CreateMap<ExtractFileGenerated, Command>().As<AddIntegrationFileCommand>();

        CreateMap<SummaryReportGenerated, AddIntegrationFileCommand>()
            .ForMember(dest => dest.Requestor,
                opt => opt.MapFrom(src => new HPP.Core.BAS.CQRS.UserMeta.RequestorModel { IdentifierId = src.UserID }))
            .ForMember(dest => dest.ETFOutboundMetadataDefnId, opt => opt.MapFrom(src => src.FTEMetadataId))
            .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
            .ForMember(dest => dest.GroupInfo, opt => opt.MapFrom(src => SetGroupInfo(src)))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember is not null));

        CreateMap<SummaryReportGenerated, Command>().As<AddIntegrationFileCommand>();
    }

    private static List<GroupInfo> SetGroupInfo(ExtractFileGenerated extractFileGenerated)
    {
        List<GroupInfo> groupInfos = new List<GroupInfo>();

        foreach (var grpInfo in extractFileGenerated.GroupInfo)
        {
            GroupInfo groupInfo = new GroupInfo()
            {
                GroupId = grpInfo.GroupId,
                NumberOfRecords = grpInfo.NumberOfRecords
            };

            groupInfos.Add(groupInfo);
        }

        return groupInfos;
    }

    private static List<GroupInfo> SetGroupInfo(SummaryReportGenerated summaryReportGenerated)
    {
        return new List<GroupInfo>
        {
            new GroupInfo
            {
                GroupId = summaryReportGenerated.GroupId,
                NumberOfRecords = summaryReportGenerated.NumberOfRecords
            }
        };
    }
}

using System.Diagnostics.CodeAnalysis;
using FxferService.Application.LegacyContracts;
using FxferService.Application.LegacyContracts.ExternalEvents.EventToFile.Contracts.Events;
using FxferService.Application.LegacyContracts.ExternalEvents.EventsDistributor.Contracts.Events;
using FxferService.Domain.Contracts.Domain;
using ETFEvents = FxferService.Application.LegacyContracts.ExternalEvents.EventToFile.Contracts.Events;
using FTEEvents = FxferService.Application.LegacyContracts.ExternalEvents.EventsDistributor.Contracts.Events;

namespace FxferService.Application.Profiles;

[ExcludeFromCodeCoverage]
public class IntegrationFileProfile : Profile
{
    #region Base Methods
    protected override void Configure()
    {
        this.MapExtractFileGeneratedEvent();
        this.MapSummaryReportGeneratedEvent();
    }
    #endregion

    #region Private Methods
    private void MapExtractFileGeneratedEvent()
    {
        this.CreateMap<ETFEvents.ExtractFileGenerated, AddIntegrationFileCommand>()
                        .ForMember(dest => dest.Requestor, opt => opt.MapFrom(src => new RequestorModel { IdentifierId = src.UserID }))
                        .ForMember(dest => dest.ETFOutboundMetadataDefnId, opt => opt.MapFrom(src => src.MetadataId))
                        .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileDirectory))
                        .ForMember(dest => dest.GroupInfo, opt => opt.MapFrom(src => this.SetGroupInfo(src)))
                        .ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));

        this.CreateMap<ETFEvents.ExtractFileGenerated, Command>().As<AddIntegrationFileCommand>();
    }

    private void MapSummaryReportGeneratedEvent()
    {
        this.CreateMap<FTEEvents.SummaryReportGenerated, AddIntegrationFileCommand>()
                        .ForMember(dest => dest.Requestor, opt => opt.MapFrom(src => new RequestorModel { IdentifierId = src.UserID }))
                        .ForMember(dest => dest.ETFOutboundMetadataDefnId, opt => opt.MapFrom(src => src.FTEMetadataId))
                        .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
                        .ForMember(dest => dest.GroupInfo, opt => opt.MapFrom(src => this.SetGroupInfo(src)))
                        .ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));

        this.CreateMap<FTEEvents.SummaryReportGenerated, Command>().As<AddIntegrationFileCommand>();
    }

    private List<GroupInfo> SetGroupInfo(ExtractFileGenerated extractFileGenerated)
    {
        List<GroupInfo> groupInfos = new List<GroupInfo>();

        foreach (var grpInfo in extractFileGenerated.GroupInfo)
        {
            GroupInfo groupInfo = new GroupInfo()
            {
                GroupId = grpInfo.GroupId,
                    NumberOfRecords = grpInfo.NumberOfRecords,
            };

            groupInfos.Add(groupInfo);
        }

        return groupInfos;
    }

    private List<GroupInfo> SetGroupInfo(SummaryReportGenerated summaryReportGenerated)
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
    #endregion
}



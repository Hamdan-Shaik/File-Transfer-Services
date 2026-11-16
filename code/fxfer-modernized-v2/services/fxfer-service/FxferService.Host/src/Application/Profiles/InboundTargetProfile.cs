using System.Diagnostics.CodeAnalysis;
using FxferService.Application.LegacyContracts;
using FTEEvents = FxferService.Application.LegacyContracts.ExternalEvents.EventsDistributor.Contracts.Events;

namespace FxferService.Application.Profiles;

[ExcludeFromCodeCoverage]
public class InboundTargetProfile : Profile
{
    #region Base Methods
    protected override void Configure()
    {
        this.MapFTEMetadataUpdatedEvent();
        this.MapFTEMetadataInvalidatedEvent();
    }
    #endregion

    #region Private Methods
    private void MapFTEMetadataUpdatedEvent()
    {
        this.CreateMap<FTEEvents.FTEMetadataUpdated, AddFtsInboundTargetCommand>()
                        .ForMember(dest => dest.Requestor, opt => opt.MapFrom(src => new RequestorModel { IdentifierId = src.UserID }))
                        .ForMember(dest => dest.ContractName, opt => opt.MapFrom(src => src.ModuleName))
                        .ForMember(dest => dest.ImportJobName, opt => opt.MapFrom(src => src.ImportJobName))
                        .ForMember(dest => dest.FTEMetadataId, opt => opt.MapFrom(src => src.FTEMetadataId))
                        .ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));

        this.CreateMap<FTEEvents.FTEMetadataUpdated, Command>().As<AddFtsInboundTargetCommand>();
    }

    private void MapFTEMetadataInvalidatedEvent()
    {
        this.CreateMap<FTEEvents.FTEMetadataInvalidated, InvalidateFtsInboundTargetCommand>()
                        .ForMember(dest => dest.Requestor, opt => opt.MapFrom(src => new RequestorModel { IdentifierId = src.UserID }))
                        .ForMember(dest => dest.FTEMetadataId, opt => opt.MapFrom(src => src.FTEMetadataId))
                        .ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));

        this.CreateMap<FTEEvents.FTEMetadataInvalidated, Command>().As<InvalidateFtsInboundTargetCommand>();
    }
    #endregion
}



using AutoMapper;
using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Events;

namespace Fxfer.DomainApi.Application.Profiles;

public sealed class InboundTargetProfile : Profile
{
    public InboundTargetProfile()
    {
        CreateMap<FTEMetadataUpdated, AddFtsInboundTargetCommand>()
            .ForMember(dest => dest.Requestor,
                opt => opt.MapFrom(src => new HPP.Core.BAS.CQRS.UserMeta.RequestorModel { IdentifierId = src.UserID }))
            .ForMember(dest => dest.ContractName, opt => opt.MapFrom(src => src.ModuleName))
            .ForMember(dest => dest.ImportJobName, opt => opt.MapFrom(src => src.ImportJobName))
            .ForMember(dest => dest.FTEMetadataId, opt => opt.MapFrom(src => src.FTEMetadataId))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember is not null));

        CreateMap<FTEMetadataUpdated, Command>().As<AddFtsInboundTargetCommand>();

        CreateMap<FTEMetadataInvalidated, InvalidateFtsInboundTargetCommand>()
            .ForMember(dest => dest.Requestor,
                opt => opt.MapFrom(src => new HPP.Core.BAS.CQRS.UserMeta.RequestorModel { IdentifierId = src.UserID }))
            .ForMember(dest => dest.FTEMetadataId, opt => opt.MapFrom(src => src.FTEMetadataId))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember is not null));

        CreateMap<FTEMetadataInvalidated, Command>().As<InvalidateFtsInboundTargetCommand>();
    }
}

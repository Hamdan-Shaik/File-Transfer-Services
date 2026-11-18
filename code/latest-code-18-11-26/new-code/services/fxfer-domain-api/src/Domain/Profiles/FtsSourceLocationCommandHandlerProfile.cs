using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Events;

namespace Fxfer.DomainApi.Domain.Profiles;

[ExcludeFromCodeCoverage]
public sealed class FtsSourceLocationCommandHandlerProfile : Profile
{
    public FtsSourceLocationCommandHandlerProfile()
    {
        CreateMap<FtsSourceLocationAddCommand, FtsSourceLocationAdded>()
            .ForMember(d => d.FtsSourceLocationsId, opt => opt.MapFrom(s => s.FtsSourceLocationId))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember is not null));

        CreateMap<FtsSourceLocationUpdateCommand, FtsSourceLocationUpdated>()
            .ForMember(d => d.FtsSourceLocationsId, opt => opt.MapFrom(s => s.FtsSourceLocationId))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember is not null));
    }
}

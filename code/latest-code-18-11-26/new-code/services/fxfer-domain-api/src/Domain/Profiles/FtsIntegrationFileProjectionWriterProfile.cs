using AutoMapper;
using Fxfer.DomainApi.Contracts.Events;
using Fxfer.DomainApi.Infrastructure.DataAccess.Entities;

namespace Fxfer.DomainApi.Domain.Profiles;

public sealed class FtsIntegrationFileProjectionWriterProfile : Profile
{
    public FtsIntegrationFileProjectionWriterProfile()
    {
        CreateMap<IntegrationFileAdded, FtsIntegrationFile>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember is not null));
    }
}


using AutoMapper;
using Fxfer.DomainApi.Contracts.Domain;
using Fxfer.DomainApi.Contracts.Events;

namespace Fxfer.DomainApi.Domain.Profiles;

public sealed class FtpServerProjectionWriterProfile : Profile
{
    public FtpServerProjectionWriterProfile()
    {
        CreateMap<FtpServerAdded, FtpServer>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember is not null));
    }
}


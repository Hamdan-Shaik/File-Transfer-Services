using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Events;

namespace Fxfer.DomainApi.Domain.Profiles;

[ExcludeFromCodeCoverage]
public sealed class FtpServerCommandHandlerProfile : Profile
{
    public FtpServerCommandHandlerProfile()
    {
        CreateMap<AddFtpServerCommand, FtpServerAdded>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember is not null));

        CreateMap<UpdateFtpServerCommand, FtpServerUpdated>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember is not null));

        CreateMap<InvalidateFtpServerCommand, FtpServerInvalidated>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember is not null));
    }
}


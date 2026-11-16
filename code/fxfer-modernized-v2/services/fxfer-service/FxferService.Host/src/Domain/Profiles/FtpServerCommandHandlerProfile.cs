using System.Diagnostics.CodeAnalysis;
using FxferService.Application.LegacyContracts;
using FxferService.Domain.Contracts.Events;

namespace FxferService.Domain.Profiles;

[ExcludeFromCodeCoverage]
public class FtpServerCommandHandlerProfile : Profile
{
    #region Base Methods
    protected override void Configure()
    {
        this.CreateMap<AddFtpServerCommand, FtpServerAdded>()
                         .ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));

        this.CreateMap<UpdateFtpServerCommand, FtpServerUpdated>()
                         .ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));
    }
    #endregion
}



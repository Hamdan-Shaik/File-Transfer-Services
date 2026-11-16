using FxferService.Application.LegacyContracts;
using FxferService.Domain.Contracts.Events;
using FxferService.Infrastructure.Persistence.Entities;

namespace FxferService.Domain.Profiles;

public class FtpServerProjectionWriterProfile : Profile
{
    #region Base Methods
    protected override void Configure()
    {
        this.CreateMap<FtpServerAdded, FtpServer>()
                         .ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));
    }
    #endregion
}



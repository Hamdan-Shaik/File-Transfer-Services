using FxferService.Application.LegacyContracts;
using FxferService.Domain.Contracts.Events;
using FtsInboundTarget = FxferService.Infrastructure.Persistence.Entities.FtsInboundTargetEntity;

namespace FxferService.Domain.Profiles;

public class FtsInboundTargetProjectionWriterProfile : Profile
{
    #region Base Methods
    protected override void Configure()
    {
        this.CreateMap<FtsInboundTargetAdded, FtsInboundTarget>()
                         .ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));
    }
    #endregion
}



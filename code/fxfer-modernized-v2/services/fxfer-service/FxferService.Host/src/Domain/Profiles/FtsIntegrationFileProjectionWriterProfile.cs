using FxferService.Application.LegacyContracts;
using FxferService.Domain.Contracts.Events;
using FtsIntegrationFile = FxferService.Infrastructure.Persistence.Entities.FtsIntegrationFileEntity;

namespace FxferService.Domain.Profiles;

public class FtsIntegrationFileProjectionWriterProfile : Profile
{
    #region Base Methods
    protected override void Configure()
    {
        this.CreateMap<IntegrationFileAdded, FtsIntegrationFile>()
                         .ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));
    }
    #endregion
}


using Navicon.Mdm.ExternalServices.Extensions.BusinessRules;
using Navicon.Mdm.ExternalServices.Extensions.Entities;
using Navicon.Mdm.ExternalServices.InfrastructureContracts;
using Navicon.Mdm.ExternalServices.ServiceContracts;
using Navicon.Mdm.MasterData.BusinessRules.Dto;

namespace Navicon.Mdm.ExternalServices.Service;

public class EntityValidationService : IEntityValidationService
{
    private readonly IEntityValidationInfrastructure _entityValidationInfrastructure;

    public EntityValidationService(IEntityValidationInfrastructure entityValidationInfrastructure)
    {
        _entityValidationInfrastructure = entityValidationInfrastructure ?? throw new ArgumentNullException(nameof(entityValidationInfrastructure));
    }

    public async Task<NotificationDto> ValidateEntityAsync(EntityDto entityDto)
    {
        var entity = entityDto?.ToEntity();

        var notification = await _entityValidationInfrastructure.ValidateEntityAsync(entity);

        return notification.ToNotificationDto();
    }
}

using Navicon.Mdm.MasterData.BusinessRules.Dto;

namespace Navicon.Mdm.ExternalServices.ServiceContracts;

public interface IEntityValidationService
{
    Task<NotificationDto> ValidateEntityAsync(EntityDto entityDto);
}

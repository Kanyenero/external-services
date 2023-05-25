using Navicon.Mdm.ExternalServices.Model.BusinessRules;
using Navicon.Mdm.ExternalServices.Model.Entities;

namespace Navicon.Mdm.ExternalServices.InfrastructureContracts;

public interface IEntityValidationInfrastructure
{
    Task<Notification> ValidateEntityAsync(Entity entity);
}

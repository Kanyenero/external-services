using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Enumerations;

namespace Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.Data;

public class EntityData
{
    public IDictionary<AttributeAssignment, string> AttributeNames { get; set; }

    public IDictionary<EntityAssignment, string> EntityNames { get; set; }
}

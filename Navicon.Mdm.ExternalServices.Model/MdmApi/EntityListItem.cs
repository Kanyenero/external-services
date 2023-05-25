using Navicon.Mdm.ExternalServices.Model.Entities;
using Attribute = Navicon.Mdm.ExternalServices.Model.Entities.Attribute;

namespace Navicon.Mdm.ExternalServices.Model.MdmApi;

public class EntityListItem
{
    public long? EntityId { get; set; }

    public long EntityInfoId { get; set; }

    public string Code { get; set; }

    public string Title { get; set; }

    public IEnumerable<Attribute> AttributeList { get; set; }

    public IEnumerable<LinkAttribute> LinkAttributeList { get; set; }
}

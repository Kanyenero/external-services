using Navicon.Mdm.ExternalServices.Model.Entities;
using Attribute = Navicon.Mdm.ExternalServices.Model.Entities.Attribute;

namespace Navicon.Mdm.ExternalServices.Extensions.Entities;

public static class PrimitiveEntityExtensions
{
    public static bool TryGetAttribute(this PrimitiveEntity primitive, string attributeName, out Attribute attribute)
    {
        if (primitive is null || attributeName is null)
        {
            attribute = null;

            return false;
        }

        attribute = primitive.Attributes?.FirstOrDefault(attribute => attribute.Name == attributeName);

        return attribute is not null;
    }

    public static bool TryGetLinkAttribute(this PrimitiveEntity primitive, string linkAttributeName, out LinkAttribute linkAttribute)
    {
        if (primitive is null || linkAttributeName is null)
        {
            linkAttribute = null;

            return false;
        }

        linkAttribute = primitive.LinkAttributes?.FirstOrDefault(linkAttribute => linkAttribute.Name == linkAttributeName);

        return linkAttribute is not null;
    }
}

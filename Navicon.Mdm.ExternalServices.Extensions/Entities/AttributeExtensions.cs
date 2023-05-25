using Attribute = Navicon.Mdm.ExternalServices.Model.Entities.Attribute;

namespace Navicon.Mdm.ExternalServices.Extensions.Entities;

public static class AttributeExtensions
{
    public static bool TrySetValue(this Attribute attribute, string value)
    {
        if (attribute is null || string.Equals(value, attribute.Value, StringComparison.Ordinal))
        {
            return false;
        }

        attribute.Value = value;

        return true;
    }
}

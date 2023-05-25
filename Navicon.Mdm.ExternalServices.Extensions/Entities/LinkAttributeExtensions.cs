using Navicon.Mdm.ExternalServices.Model.Entities;

namespace Navicon.Mdm.ExternalServices.Extensions.Entities;

public static class LinkAttributeExtensions
{
    public static bool TrySetValue(this LinkAttribute linkAttribute, string newValue, long? newEntityId)
    {
        if (linkAttribute is null || string.IsNullOrWhiteSpace(newValue) || !newEntityId.HasValue || newEntityId == linkAttribute.LinkedEntityId)
        {
            return false;
        }

        linkAttribute.Value = newValue;
        linkAttribute.LinkedEntityId = newEntityId;

        return true;
    }
}

using Navicon.Mdm.ExternalServices.Model.Entities;
using Attribute = Navicon.Mdm.ExternalServices.Model.Entities.Attribute;

namespace Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.AttributeDescriptors;

internal class LinkAttributeDescriptor : AttributeDescriptorBase
{
    public LinkAttribute LinkAttribute { get; set; }

    public override string Name => LinkAttribute?.Name;

    public override string Value => LinkAttribute?.Value;

    public override bool HasValue => LinkAttribute is not null && LinkAttribute.HasValue;

    public Attribute AffectedAttribute { get; set; }

    public bool HasAffectedAttribute => AffectedAttribute is not null;

    public override string ToString() => string.Format("{0}, {1} = {2}", base.ToString(), nameof(HasAffectedAttribute), HasAffectedAttribute);
}

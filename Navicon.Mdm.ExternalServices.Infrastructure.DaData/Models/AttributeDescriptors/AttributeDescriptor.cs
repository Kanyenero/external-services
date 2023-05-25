using Attribute = Navicon.Mdm.ExternalServices.Model.Entities.Attribute;

namespace Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.AttributeDescriptors;

internal class AttributeDescriptor : AttributeDescriptorBase
{
    public Attribute Attribute { get; set; }

    public override string Name => Attribute?.Name;

    public override string Value => Attribute?.Value;

    public override bool HasValue => Attribute is not null && Attribute.HasValue;

    public override string ToString() => string.Format("{0}", base.ToString());
}

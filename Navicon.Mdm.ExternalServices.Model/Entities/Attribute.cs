namespace Navicon.Mdm.ExternalServices.Model.Entities;

public class Attribute : AttributeBase
{
    public int? AttributeInfoId { get; set; }

    public AttributeType AttributeType { get; set; }

    public override string ToString() => string.Format("{0}, {1} = {2}", base.ToString(), nameof(AttributeType), AttributeType);
}

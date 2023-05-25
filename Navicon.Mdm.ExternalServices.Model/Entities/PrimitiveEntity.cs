namespace Navicon.Mdm.ExternalServices.Model.Entities;

public class PrimitiveEntity
{
    public string PrimitiveEntityName { get; set; }

    public string MdmCode { get; set; }

    public ICollection<Attribute> Attributes { get; set; }

    public ICollection<LinkAttribute> LinkAttributes { get; set; }

    public override string ToString() => string.Format("{0} = {1}, {2} = {3}, {4} = {5}, {6} = {7}", nameof(PrimitiveEntityName), PrimitiveEntityName, nameof(MdmCode), MdmCode, nameof(Attributes), Attributes?.Count ?? 0, nameof(LinkAttributes), LinkAttributes?.Count ?? 0);
}

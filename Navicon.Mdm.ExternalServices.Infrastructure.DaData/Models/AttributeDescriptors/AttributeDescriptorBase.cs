namespace Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.AttributeDescriptors;

internal abstract class AttributeDescriptorBase
{
    public string PrimitiveEntityName { get; set; }

    public string MdmCode { get; set; }

    public abstract string Name { get; }

    public abstract string Value { get; }

    public abstract bool HasValue { get; }

    public override string ToString() => string.Format("{0} = {1}, {2} = {3}", nameof(Name), Name, nameof(Value), Value);
}

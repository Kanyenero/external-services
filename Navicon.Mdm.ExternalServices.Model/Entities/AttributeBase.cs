namespace Navicon.Mdm.ExternalServices.Model.Entities;

public abstract class AttributeBase
{
    public string Name { get; set; }

    public string Value { get; set; }

    public bool HasValue => !string.IsNullOrWhiteSpace(Value);

    public override string ToString() => string.Format("{0} = {1}, {2} = {3}", nameof(Name), Name, nameof(Value), Value);
}

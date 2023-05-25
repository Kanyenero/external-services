namespace Navicon.Mdm.ExternalServices.Model.BusinessRules;

public class ValueChangeInfo
{
    public string PrimitiveEntityName { get; set; }

    public string MdmCode { get; set; }

    public string AttributeName { get; set; }

    public string Value { get; set; }

    public override string ToString() => string.Format("{0} = {1}, {2} = {3}", nameof(AttributeName), AttributeName, nameof(Value), Value);
}

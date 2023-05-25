namespace Navicon.Mdm.ExternalServices.Model.BusinessRules;

public class ValidationAlert
{
    public string PrimitiveEntityName { get; set; }

    public string MdmCode { get; set; }

    public string AttributeName { get; set; }

    public SeverityType SeverityType { get; set; }

    public string Message { get; set; }

    public override string ToString() => string.Format("{0} = {1}, {2} = {3}", nameof(AttributeName), AttributeName, nameof(SeverityType), SeverityType);
}

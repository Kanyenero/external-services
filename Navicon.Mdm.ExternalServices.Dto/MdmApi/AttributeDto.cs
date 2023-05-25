namespace Navicon.Mdm.ExternalServices.Dto.MdmApi;

public class AttributeDto
{
    public long? EntityId { get; set; }
    public int? AttributeInfoId { get; set; }
    public string AttributeInfoName { get; set; }
    public string StringValue { get; set; }
    public string VirtualValue { get; set; }
    public bool? BoolValue { get; set; }
    public DateTime? DateTimeValue { get; set; }
    public decimal? NumberValue { get; set; }
    public long? LinkedEntityId { get; set; }
    public string LinkedEntityCode { get; set; }
    public int? LinkedEntityInfoId { get; set; }
    public string LinkedEntityTitle { get; set; }
    public string LinkedEntityInfoName { get; set; }
    public int? FileId { get; set; }
    public string FileName { get; set; }
    public bool IsTitle { get; set; }
    public bool IsVirtual { get; set; }
    public bool IsMultiLink { get; set; }
    public DateTime? DateValue { get; set; }
    public string MultilineValue { get; set; }
}

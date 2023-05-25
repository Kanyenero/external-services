namespace Navicon.Mdm.ExternalServices.Model.Entities;

public class LinkAttribute : AttributeBase
{
    public long? LinkedEntityId { get; set; }

    public long LinkedEntityInfoId { get; set; }

    public bool IsMultiLink { get; set; }

    public override string ToString() => string.Format("{0}, {1} = {2}, {3} = {4}", base.ToString(), nameof(LinkedEntityInfoId), LinkedEntityInfoId, nameof(LinkedEntityId), LinkedEntityId);
}

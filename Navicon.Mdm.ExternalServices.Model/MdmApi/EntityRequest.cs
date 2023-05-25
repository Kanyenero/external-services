namespace Navicon.Mdm.ExternalServices.Model.MdmApi;

public class EntityRequest
{
    public string RequestUri { get; set; }

    public long EntityInfoId { get; set; }

    public EntityFilter EntityFilter { get; set; }
}

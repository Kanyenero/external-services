namespace Navicon.Mdm.ExternalServices.Model.MdmApi;

public class EntityList
{
    public IEnumerable<EntityListItem> Entities { get; set; }

    public long Total { get; set; }

    public string ScrollKey { get; set; }
}

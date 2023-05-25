namespace Navicon.Mdm.ExternalServices.Dto.MdmApi;

public class EntityListDto
{
    public IEnumerable<EntityListItemDto> Entities { get; set; }
    public long Total { get; set; }
    public string ScrollKey { get; set; }
}

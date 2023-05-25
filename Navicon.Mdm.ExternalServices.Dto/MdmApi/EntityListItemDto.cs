namespace Navicon.Mdm.ExternalServices.Dto.MdmApi;

public class EntityListItemDto
{
    public long? EntityId { get; set; }
    public string Code { get; set; }
    public long EntityInfoId { get; set; }
    public IEnumerable<AttributeDto> AttributeList { get; set; }
    public string Title { get; set; }
    public PrimitiveEntityDataStateEnum EntityDataState { get; set; }
    public EntityWithNameDto CreationUser { get; set; }
    public EntityWithNameDto LastChangeUser { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? LastChangeDate { get; set; }
    public bool HasDuplicates { get; set; }
    public long ItemVersionId { get; set; }
    public byte State { get; set; }
    public bool IsValid { get; set; }
    public double Score { get; set; }
    public int SourceId { get; set; }
    public int OwnerSystemId { get; set; }
}

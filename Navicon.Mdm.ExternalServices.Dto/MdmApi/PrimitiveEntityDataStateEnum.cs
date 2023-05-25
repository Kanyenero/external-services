namespace Navicon.Mdm.ExternalServices.Dto.MdmApi;

/// <summary>
/// Значения статуса мастер-данных
/// </summary>
public enum PrimitiveEntityDataStateEnum : byte
{
    Actual = 1,
    Archival = 2,
    Deleted = 3,
    Merged = 4,
    New = 5
}

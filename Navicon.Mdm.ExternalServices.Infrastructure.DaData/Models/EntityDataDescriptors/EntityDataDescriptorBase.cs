namespace Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.EntityDataDescriptors;

internal abstract class EntityDataDescriptorBase
{
    protected internal abstract string PrimitiveEntityName { get; set; }

    protected internal abstract string MdmCode { get; set; }
}

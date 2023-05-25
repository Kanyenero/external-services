using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.AttributeDescriptors;

namespace Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.EntityDataDescriptors;

internal class AddressEntityDataDescriptor : EntityDataDescriptorBase
{
    public LinkAttributeDescriptor Country { get; } = new();

    public LinkAttributeDescriptor City { get; } = new();

    public AttributeDescriptor Address { get; } = new();

    public AttributeDescriptor Fias { get; } = new();

    private string _primitiveEntityName;

    protected internal override string PrimitiveEntityName
    {
        get => _primitiveEntityName;
        set
        {
            _primitiveEntityName = value;

            Country.PrimitiveEntityName = value;
            City.PrimitiveEntityName = value;
            Address.PrimitiveEntityName = value;
            Fias.PrimitiveEntityName = value;
        }
    }

    private string _mdmCode;

    protected internal override string MdmCode
    {
        get => _mdmCode;
        set
        {
            _mdmCode = value;

            Country.MdmCode = value;
            City.MdmCode = value;
            Address.MdmCode = value;
            Fias.MdmCode = value;
        }
    }
}

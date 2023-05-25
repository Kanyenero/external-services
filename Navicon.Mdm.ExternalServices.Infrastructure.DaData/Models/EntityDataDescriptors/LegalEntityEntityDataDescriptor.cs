using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.AttributeDescriptors;

namespace Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.EntityDataDescriptors;

internal class LegalEntityEntityDataDescriptor : EntityDataDescriptorBase
{
    public AttributeDescriptor Inn { get; } = new();

    public AttributeDescriptor Ogrn { get; } = new();

    public AttributeDescriptor Kpp { get; } = new();

    public AttributeDescriptor Name { get; } = new();

    public AttributeDescriptor RegistrationDate { get; } = new();

    public AttributeDescriptor Okved { get; } = new();

    public LinkAttributeDescriptor Opf { get; } = new();

    private string _primitiveEntityName;

    protected internal override string PrimitiveEntityName
    {
        get => _primitiveEntityName;
        set
        {
            _primitiveEntityName = value;

            Inn.PrimitiveEntityName = value;
            Kpp.PrimitiveEntityName = value;
            Opf.PrimitiveEntityName = value;
            Ogrn.PrimitiveEntityName = value;
            Okved.PrimitiveEntityName = value;
            Name.PrimitiveEntityName = value;
            RegistrationDate.PrimitiveEntityName = value;
        }
    }

    private string _mdmCode;

    protected internal override string MdmCode
    {
        get => _mdmCode;
        set
        {
            _mdmCode = value;

            Inn.MdmCode = value;
            Kpp.MdmCode = value;
            Opf.MdmCode = value;
            Ogrn.MdmCode = value;
            Okved.MdmCode = value;
            Name.MdmCode = value;
            RegistrationDate.MdmCode = value;
        }
    }
}

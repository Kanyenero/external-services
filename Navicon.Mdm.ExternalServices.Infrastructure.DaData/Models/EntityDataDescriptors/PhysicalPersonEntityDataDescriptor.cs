using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.AttributeDescriptors;

namespace Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.EntityDataDescriptors;

internal class PhysicalPersonEntityDataDescriptor : EntityDataDescriptorBase
{
    public AttributeDescriptor Fullname { get; } = new();

    public AttributeDescriptor Surname { get; } = new();

    public AttributeDescriptor Name { get; } = new();

    public AttributeDescriptor Patronymic { get; } = new();

    public AttributeDescriptor Gender { get; } = new();

    private string _primitiveEntityName;

    protected internal override string PrimitiveEntityName
    {
        get => _primitiveEntityName;
        set
        {
            _primitiveEntityName = value;

            Fullname.PrimitiveEntityName = value;
            Surname.PrimitiveEntityName = value;
            Name.PrimitiveEntityName = value;
            Patronymic.PrimitiveEntityName = value;
            Gender.PrimitiveEntityName = value;
        }
    }

    private string _mdmCode;

    protected internal override string MdmCode
    {
        get => _mdmCode;
        set
        {
            _mdmCode = value;

            Fullname.MdmCode = value;
            Surname.MdmCode = value;
            Name.MdmCode = value;
            Patronymic.MdmCode = value;
            Gender.MdmCode = value;
        }
    }
}

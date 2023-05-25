using Navicon.Mdm.ExternalServices.Model.BusinessRules;
using Navicon.Mdm.MasterData.BusinessRules.Dto;

namespace Navicon.Mdm.ExternalServices.Extensions.BusinessRules;

public static class ValueChangeInfoExtensions
{
    public static ValueChangeInfoDto ToValueChangeInfoDto(this ValueChangeInfo source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        return new()
        {
            PrimitiveEntityName = source.PrimitiveEntityName,
            MdmCode = source.MdmCode,
            AttributeName = source.AttributeName,
            Value = source.Value
        };
    }

    public static ValueChangeInfo ToValueChangeInfo(this ValueChangeInfoDto source) 
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        return new()
        {
            PrimitiveEntityName = source.PrimitiveEntityName,
            MdmCode = source.MdmCode,
            AttributeName = source.AttributeName,
            Value = source.Value
        };
    }
}

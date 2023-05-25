using Navicon.Mdm.ExternalServices.Model.BusinessRules;
using Navicon.Mdm.MasterData.BusinessRules.Dto;

namespace Navicon.Mdm.ExternalServices.Extensions.BusinessRules;

public static class ValidationAlertExtensions
{
    public static ValidationAlertDto ToValidationAlertDto(this ValidationAlert source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        return new()
        {
            PrimitiveEntityName = source.PrimitiveEntityName,
            MdmCode = source.MdmCode,
            AttributeName = source.AttributeName,
            SeverityType = source.SeverityType switch
            {
                SeverityType.Info => SeverityTypeDto.Info,
                SeverityType.Error => SeverityTypeDto.Error,
                _ => throw new ArgumentException($"Unexpected [{nameof(SeverityType)}]: '{Enum.GetName(source.SeverityType)}'.")
            },
            Message = source.Message
        };
    }

    public static ValidationAlert ToValidationAlert(this ValidationAlertDto source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        return new()
        {
            PrimitiveEntityName = source.PrimitiveEntityName,
            MdmCode = source.MdmCode,
            AttributeName = source.AttributeName,
            SeverityType = source.SeverityType switch
            {
                SeverityTypeDto.Info => SeverityType.Info,
                SeverityTypeDto.Error => SeverityType.Error,
                _ => throw new ArgumentException($"Unexpected [{nameof(SeverityTypeDto)}]: '{Enum.GetName(source.SeverityType)}'.")
            },
            Message = source.Message
        };
    }
}

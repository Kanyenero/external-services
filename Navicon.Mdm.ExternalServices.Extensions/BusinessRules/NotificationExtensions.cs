using Navicon.Mdm.ExternalServices.Extensions.Data;
using Navicon.Mdm.ExternalServices.Model.BusinessRules;
using Navicon.Mdm.MasterData.BusinessRules.Dto;

namespace Navicon.Mdm.ExternalServices.Extensions.BusinessRules;

public static class NotificationExtensions
{
    public static NotificationDto ToNotificationDto(this Notification source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        return new()
        {
            Alerts = source.Alerts?.Select(validationAlert => validationAlert.ToValidationAlertDto()).AsList(),
            Changes = source.Changes?.Select(valueChangeInfo => valueChangeInfo.ToValueChangeInfoDto()).AsList()
        };
    }

    public static void AddValueChangeInfo(this Notification notification, string primitiveEntityName, string mdmCode, string attributeName, string value)
    {
        ArgumentNullException.ThrowIfNull(notification, nameof(notification));
        ArgumentNullException.ThrowIfNull(primitiveEntityName, nameof(primitiveEntityName));
        ArgumentNullException.ThrowIfNull(mdmCode, nameof(mdmCode));
        ArgumentNullException.ThrowIfNull(attributeName, nameof(attributeName));

        notification.Changes ??= new List<ValueChangeInfo>();
        notification.Changes.Add(new ValueChangeInfo
        {
            PrimitiveEntityName = primitiveEntityName,
            MdmCode = mdmCode,
            AttributeName = attributeName,
            Value = value
        });
    }

    public static void AddValidationAlert(this Notification notification, string primitiveEntityName, string mdmCode, string attributeName, SeverityType severityType, string message)
    {
        ArgumentNullException.ThrowIfNull(notification, nameof(notification));
        ArgumentNullException.ThrowIfNull(primitiveEntityName, nameof(primitiveEntityName));
        ArgumentNullException.ThrowIfNull(mdmCode, nameof(mdmCode));
        ArgumentNullException.ThrowIfNull(attributeName, nameof(attributeName));

        notification.Alerts ??= new List<ValidationAlert>();
        notification.Alerts.Add(new ValidationAlert
        {
            PrimitiveEntityName = primitiveEntityName,
            MdmCode = mdmCode,
            AttributeName = attributeName,
            SeverityType = severityType,
            Message = message
        });
    }
}

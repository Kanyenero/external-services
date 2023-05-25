using Navicon.Mdm.ExternalServices.Model.Entities;

namespace Navicon.Mdm.ExternalServices.Model.BusinessRules;

public class Notification
{
    public ICollection<ValidationAlert> Alerts { get; set; }

    public ICollection<ValueChangeInfo> Changes { get; set; }

    public override string ToString() => string.Format("{0} = {1}, {2} = {3}", nameof(Alerts), Alerts?.Count ?? 0, nameof(Changes), Changes?.Count ?? 0);
}

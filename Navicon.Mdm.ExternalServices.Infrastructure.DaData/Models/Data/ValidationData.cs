using System.Net;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Enumerations;

namespace Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.Data;

public class ValidationData
{
    public IDictionary<AttributeAssignment, IDictionary<ValidationResultType, string>> ValidationMessages { get; set; }

    public IDictionary<EnumerationAssignment, IDictionary<EnumerationItemAssignment, string>> EnumerationBindings { get; set; }

    public IDictionary<HttpStatusCode, string> HttpRequestMessages { get; set; }
}

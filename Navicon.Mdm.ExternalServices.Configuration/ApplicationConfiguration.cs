using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Options;

namespace Navicon.Mdm.ExternalServices.Configuration;

public class ApplicationConfiguration
{
    public ApplicationOptions ApplicationOptions { get; set; }

    public EntityValidationInfrastructureOptions EntityValidationServiceOptions { get; set; }
}

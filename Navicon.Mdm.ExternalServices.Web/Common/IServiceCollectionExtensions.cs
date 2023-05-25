using Navicon.Mdm.ExternalServices.Infrastructure.DaData;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Caching;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Http;
using Navicon.Mdm.ExternalServices.Infrastructure.Http;
using Navicon.Mdm.ExternalServices.InfrastructureContracts;
using Navicon.Mdm.ExternalServices.Service;
using Navicon.Mdm.ExternalServices.ServiceContracts;

namespace Navicon.Mdm.ExternalServices.Web.Common;

public static class IServiceCollectionExtensions
{
    public static void AddDependences(this IServiceCollection services)
    {
        services.AddServices();
        services.AddInfrastructure();
        services.AddDaDataEntityValidationInfrastructure();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IEntityValidationService, EntityValidationService>();
    }

    private static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpClient<IMdmApiHttpClient, MdmApiHttpClient>().ConfigurePrimaryHttpMessageHandler(sp => new HttpClientHandler { UseDefaultCredentials = true });
    }

    private static void AddDaDataEntityValidationInfrastructure(this IServiceCollection services)
    {
        services.AddHttpClient<IEntityValidationHttpClient, EntityValidationHttpClient>();
        services.AddSingleton<IEntityListCacheManager, EntityListCacheManager>();
        services.AddTransient<IEntityValidationInfrastructure, EntityValidationInfrastructure>();
    }
}

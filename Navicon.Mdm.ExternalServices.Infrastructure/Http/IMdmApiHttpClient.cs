using Navicon.Mdm.ExternalServices.Model.MdmApi;

namespace Navicon.Mdm.ExternalServices.Infrastructure.Http;

public interface IMdmApiHttpClient
{
    Task<EntityResponse> PerformEntityRequest(EntityRequest entityRequest);
}

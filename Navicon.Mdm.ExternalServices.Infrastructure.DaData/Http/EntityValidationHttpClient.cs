namespace Navicon.Mdm.ExternalServices.Infrastructure.DaData.Http;

public class EntityValidationHttpClient : IEntityValidationHttpClient
{
    private readonly HttpClient _httpClient;

    public EntityValidationHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <inheritdoc/>
    public HttpClient Instance => _httpClient;
}

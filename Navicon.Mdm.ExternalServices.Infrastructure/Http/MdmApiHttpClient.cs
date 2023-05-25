using System.Text;
using Microsoft.Extensions.Logging;
using Navicon.Mdm.ExternalServices.Dto.MdmApi;
using Navicon.Mdm.ExternalServices.Extensions.MdmApi;
using Navicon.Mdm.ExternalServices.Model.MdmApi;
using Newtonsoft.Json;

namespace Navicon.Mdm.ExternalServices.Infrastructure.Http;

public class MdmApiHttpClient : IMdmApiHttpClient
{
    private readonly ILogger _logger;
    private readonly HttpClient _httpClient;

    public MdmApiHttpClient(ILogger<MdmApiHttpClient> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<EntityResponse> PerformEntityRequest(EntityRequest request)
    {
        var requestUri = request.RequestUri;
        var entityFilter = request.EntityFilter;

        if (string.IsNullOrWhiteSpace(requestUri))
        {
            _logger.LogError("Cannot perform MDM API request [uri: {requestUri}].", nameof(requestUri));
            return new EntityResponse();
        }

        var entityFilterDto = entityFilter.ToEntityFilterDto();

        var jsonRequestContent = JsonConvert.SerializeObject(entityFilterDto);
        var httpRequestContent = new StringContent(jsonRequestContent, Encoding.UTF8, "application/json");

        var httpResponseMessage = await _httpClient.PostAsync(requestUri, httpRequestContent);

        EntityList entityList = null;

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var httpResponseContent = httpResponseMessage.Content;
            var jsonResponseContent = await httpResponseContent.ReadAsStringAsync();

            var entityListDto = JsonConvert.DeserializeObject<EntityListDto>(jsonResponseContent);
            entityList = entityListDto.ToEntityList();

            _logger.LogDebug("MDM API request succeeded [entityInfoId: {EntityInfoId}, uri: {RequestUri}].", request.EntityInfoId, request.RequestUri);
        }
        else
        {
            _logger.LogDebug("MDM API request failed [entityInfoId: {EntityInfoId}, uri: {RequestUri}].", request.EntityInfoId, request.RequestUri);
        }

        return new EntityResponse { EntityList = entityList };
    }
}

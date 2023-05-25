using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Navicon.Mdm.ExternalServices.Extensions.MdmApi;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Options;
using Navicon.Mdm.ExternalServices.Infrastructure.Http;
using Navicon.Mdm.ExternalServices.Model.MdmApi;
using Attribute = Navicon.Mdm.ExternalServices.Model.Entities.Attribute;

namespace Navicon.Mdm.ExternalServices.Infrastructure.DaData.Caching;

/// <summary>
/// Представляет менеджер кэша объектов <see cref="EntityList"/>.
/// </summary>
public class EntityListCacheManager : IEntityListCacheManager
{
    private readonly ILogger _logger;
    private readonly IMemoryCache _cache;
    private readonly IMdmApiHttpClient _httpClient;
    private readonly EntityValidationInfrastructureOptions _options;

    /// <summary>
    /// Позволяет создать объект класса <see cref="EntityListCacheManager"/>.
    /// </summary>
    /// <param name="logger">Объект журналирования.</param>
    /// <param name="options">Объект настроек менеджера.</param>
    /// <param name="cache">Объект in-memory кэша.</param>
    /// <param name="httpClient">HTTP-клиант для запросов к MDM API.</param>
    public EntityListCacheManager(
        ILogger<EntityListCacheManager> logger,
        IOptions<EntityValidationInfrastructureOptions> options,
        IMemoryCache cache,
        IMdmApiHttpClient httpClient)
    {
        _logger = logger;
        _cache = cache;
        _httpClient = httpClient;
        _options = options.Value;
    }

    /// <inheritdoc/>
    public bool CacheIsEmpty => _cache.GetCurrentStatistics()?.CurrentEntryCount == 0;

    /// <inheritdoc/>
    public ICacheEntry CreateEntityListEntry(long entityInfoId)
    {
        return _cache.CreateEntry(entityInfoId);
    }

    /// <inheritdoc/>
    public EntityList SetEntityList(long entityInfoId, EntityList value)
    {
        return _cache.Set(entityInfoId, value);
    }

    /// <inheritdoc/>
    public bool TryGetEntityList(long entityInfoId, out EntityList value)
    {
        return _cache.TryGetValue(entityInfoId, out value);
    }

    /// <inheritdoc/>
    public bool TryGetCachedAttribute(long entityInfoId, long? entityId, out Attribute result)
    {
        var entityListExists = TryGetEntityList(entityInfoId, out var entityList);
        if (!entityListExists)
        {
            _logger.LogError("[{Class}:{Method}] EntityList doesn't exists [entityInfoId: {Value}].", nameof(EntityListCacheManager), nameof(TryGetCachedAttribute), entityInfoId);
            result = null;

            return false;
        }

        var entityListItemExists = entityList.TryGetEntityListItem(entityId, out var entityListItem);
        if (!entityListItemExists)
        {
            _logger.LogWarning("[{Class}:{Method}] EntityList exists, but EntityListItem wasn't found [entityInfoId: {Value1}, entityId: {Value2}].", nameof(EntityListCacheManager), nameof(TryGetCachedAttribute), entityInfoId, entityId);
            result = null;

            return false;
        }

        var loadOption = _options.Caching.EntityListItemLoadOptions?.FirstOrDefault(option => option.EntityInfoId == entityInfoId);
        if (loadOption is null)
        {
            result = entityListItem.AttributeList.FirstOrDefault();
        }
        else
        {
            result = entityListItem.AttributeList.FirstOrDefault(attribute => attribute.AttributeInfoId == loadOption.RequiredAttributeInfoId);
        }

        return result is not null;
    }

    /// <inheritdoc/>
    public bool TryGetCachedAttribute(long entityInfoId, string attributeValue, out Attribute result, out long? entityId)
    {
        var entityListExists = TryGetEntityList(entityInfoId, out var entityList);
        if (!entityListExists)
        {
            _logger.LogError("[{Class}:{Method}] EntityList doesn't exists [entityInfoId: {Value}].", nameof(EntityListCacheManager), nameof(TryGetCachedAttribute), entityInfoId);
            result = null;
            entityId = null;

            return false;
        }

        var attributeExists = entityList.TryGetEntityListItemAttribute(attributeValue, out result, out entityId);
        if (!attributeExists)
        {
            _logger.LogError("[EntityListCacheManager:TryGetCachedAttribute] EntityList exists, but EntityListItem wasn't found [entityInfoId: {entityInfoId}, attributeValue: {attributeValue}].", entityInfoId, attributeValue);

            return false;
        }

        return result is not null;
    }

    /// <inheritdoc/>
    public void RequestAndCacheEntityLists()
    {
        var tasks = _options.Caching.MdmApiEntityRequests.Select(cacheEntityAsync).ToArray();
        Task.WaitAll(tasks);

        async Task cacheEntityAsync(EntityRequest request)
        {
            if (request is null)
            {
                return;
            }

            var entityListExists = TryGetEntityList(request.EntityInfoId, out var entityList);
            if (entityListExists)
            {
                return;
            }

            var response = await _httpClient.PerformEntityRequest(request);
            if (response?.EntityList is not null)
            {
                SetEntityList(request.EntityInfoId, response.EntityList);
            }
        }
    }

    /// <inheritdoc/>
    public void RemoveEntityList(long entityInfoId)
    {
        _cache.Remove(entityInfoId);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _cache.Dispose();
    }
}

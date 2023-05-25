using System.Diagnostics;
using Dadata;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Caching;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Enumerations;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Http;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Options;
using Navicon.Mdm.ExternalServices.InfrastructureContracts;
using Navicon.Mdm.ExternalServices.Model.BusinessRules;
using Navicon.Mdm.ExternalServices.Model.Entities;

namespace Navicon.Mdm.ExternalServices.Infrastructure.DaData;

public partial class EntityValidationInfrastructure : IEntityValidationInfrastructure
{
    private readonly ILogger _logger;
    private readonly EntityValidationInfrastructureOptions _options;
    private readonly IEntityListCacheManager _cacheManager;
    private readonly HttpClient _httpClient;
    private readonly IDictionary<string, ApiType> _validationScenarios;
    private readonly EntityValidationHandler _handler;

    public EntityValidationInfrastructure(
        ILogger<EntityValidationInfrastructure> logger,
        IOptions<EntityValidationInfrastructureOptions> options,
        IEntityListCacheManager cacheManager,
        IEntityValidationHttpClient httpClient)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options.Value ?? throw new ArgumentException($"{nameof(options.Value)} was null.", nameof(options));
        _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
        _httpClient = httpClient.Instance ?? throw new ArgumentException($"{nameof(httpClient.Instance)} was null.", nameof(httpClient));

        _validationScenarios = _options.ValidationScenarios ?? throw new ArgumentException($"{nameof(_options.ValidationScenarios)} was null.", nameof(options));

        var authData = _options.AuthorizationData ?? throw new ArgumentException($"{nameof(_options.AuthorizationData)} was null.", nameof(options));
        var publicToken = authData.PublicToken ?? throw new ArgumentException($"{nameof(authData.PublicToken)} was null.", nameof(options));
        var secretToken = authData.SecretToken ?? throw new ArgumentException($"{nameof(authData.SecretToken)} was null.", nameof(options));

        var validationData = _options.ValidationData ?? throw new ArgumentException($"{nameof(_options.ValidationData)} was null.", nameof(options));
        var entityData = _options.EntityData ?? throw new ArgumentException($"{nameof(_options.EntityData)} was null.", nameof(options));

        var apiClient = new ApiClient(publicToken, secretToken, _httpClient);

        _handler = new EntityValidationHandler(_logger, _cacheManager, apiClient, validationData, entityData)
        {
            LogDailyStats = _options.LogDailyStats,
            PartySuggestionFuzzySearchThreshold = _options.PartySuggestionFuzzySearchThreshold
        };
    }

    public async Task<Notification> ValidateEntityAsync(Entity entity)
    {
        _logger.LogDebug("# Start. Entity [{Value}].", entity?.EntityData?.PrimitiveEntityName);

        var stopwatch = Stopwatch.StartNew();

        if (_cacheManager.CacheIsEmpty)
        {
            _cacheManager.RequestAndCacheEntityLists();
        }

        Notification notification;
        try
        {
            notification = await ValidateEntityInternalAsync(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured while entity validation.");

            return new Notification();
        }
        finally
        {
            stopwatch.Stop();

            _logger.LogDebug("# Completion. Total elapsed time [{Value} ms].", stopwatch.ElapsedMilliseconds);
        }

        return notification;
    }

    private async Task<Notification> ValidateEntityInternalAsync(Entity entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        var primitiveEntityName = entity.EntityData?.PrimitiveEntityName ?? throw new ArgumentException($"{nameof(entity.EntityData.PrimitiveEntityName)} was null.", nameof(entity));
        var apiType = getApiType(primitiveEntityName);
        var notification = new Notification();

        await _handler.Handle(apiType, entity, notification);

        return notification;

        ApiType getApiType(string primitiveEntityName)
        {
            try
            {
                return _validationScenarios[primitiveEntityName];
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException($"There is no validation scenario for primitive '{primitiveEntityName}'.", ex);
            }
        }
    }
}

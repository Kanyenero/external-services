using Microsoft.Extensions.Logging;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Caching;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Enumerations;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Handlers;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.ApiDescriptors;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.Data;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.EntityDescriptors;
using Navicon.Mdm.ExternalServices.Model.BusinessRules;
using Navicon.Mdm.ExternalServices.Model.Entities;

namespace Navicon.Mdm.ExternalServices.Infrastructure.DaData;

internal class EntityValidationHandler
{
    private readonly ILogger _logger;
    private readonly IEntityListCacheManager _cacheManager;
    private readonly ApiClient _apiClient;
    private readonly ValidationData _validationData;
    private readonly EntityData _entityData;

    private EntityDescriptorBase _entityDescriptor;
    private ApiDescriptorBase _apiDescriptor;
    private ApiHandlerBase _apiHandler;

    public EntityValidationHandler(ILogger logger, IEntityListCacheManager cacheManager, ApiClient apiClient, ValidationData validationData, EntityData entityData)
    {
        _logger = logger;
        _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        _validationData = validationData ?? throw new ArgumentNullException(nameof(validationData));
        _entityData = entityData ?? throw new ArgumentNullException(nameof(entityData));
    }

    public bool LogDailyStats { get; set; }

    public int PartySuggestionFuzzySearchThreshold { get; set; }

    public async Task Handle(ApiType api, Entity entity, Notification notification)
    {
        Initialize(api);

        _entityDescriptor.Collect(entity);
        _apiDescriptor.Collect(_entityDescriptor);

        _apiHandler.LogDailyStats = LogDailyStats;
        await _apiHandler.Handle(_apiDescriptor, notification);
    }

    private void Initialize(ApiType api)
    {
        switch (api)
        {
            case ApiType.CompositePersonRecordStandardization:
                _entityDescriptor = new PhysicalPersonEntityDescriptor(_entityData, _cacheManager);
                _apiDescriptor = new CompositePersonRecordApiDescriptor();
                _apiHandler = new CompositePersonRecordApiHandler(_logger, _cacheManager, _apiClient, _validationData);
                break;

            case ApiType.PartySuggestion:
                _entityDescriptor = new LegalEntityEntityDescriptor(_entityData, _cacheManager);
                _apiDescriptor = new PartyApiDescriptor();
                _apiHandler = new PartyApiHandler(_logger, _cacheManager, _apiClient, _validationData)
                {
                    PartySuggestionFuzzySearchThreshold = PartySuggestionFuzzySearchThreshold
                };
                break;

            default:
                throw new InvalidOperationException($"Unexpected {nameof(ApiType)}: '{Enum.GetName(api)}'.");
        }
    }
}

using Microsoft.Extensions.Logging;
using Navicon.Mdm.ExternalServices.Extensions.BusinessRules;
using Navicon.Mdm.ExternalServices.Extensions.Entities;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Caching;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Enumerations;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.ApiDescriptors;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.AttributeDescriptors;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.Data;
using Navicon.Mdm.ExternalServices.Model.BusinessRules;

namespace Navicon.Mdm.ExternalServices.Infrastructure.DaData.Handlers;

internal abstract class ApiHandlerBase
{
    protected readonly ILogger Logger;
    protected readonly IEntityListCacheManager CacheManager;
    protected readonly ApiClient ApiClient;
    protected readonly ValidationData ValidationData;

    protected ApiHandlerBase(ILogger logger, IEntityListCacheManager cacheManager, ApiClient apiClient, ValidationData validationData)
    {
        Logger = logger;
        CacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
        ApiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        ValidationData = validationData ?? throw new ArgumentNullException(nameof(validationData));
    }

    public bool LogDailyStats { get; set; }

    public virtual async Task Handle()
    {
        if (LogDailyStats)
        {
            await GetAndLogDailyStats();
        }
    }

    public abstract Task Handle(ApiDescriptorBase apiDescriptor, Notification notification);

    protected static void ApplyValidationResult(AttributeDescriptor descriptor, string resultMessage, string resultValue, in Notification notification)
    {
        var primitiveEntityName = descriptor.PrimitiveEntityName;
        var mdmCode = descriptor.MdmCode;
        var name = descriptor.Name;
        var attribute = descriptor.Attribute;

        if (attribute is not null)
        {
            var valueSet = attribute.TrySetValue(resultValue);
            if (valueSet)
            {
                notification.AddValueChangeInfo(primitiveEntityName, mdmCode, name, resultValue);
                notification.AddValidationAlert(primitiveEntityName, mdmCode, name, SeverityType.Info, resultMessage);
            }
        }
    }

    protected void ApplyValidationResult(LinkAttributeDescriptor descriptor, string resultMessage, string resultValue, in Notification notification)
    {
        var primitiveEntityName = descriptor.PrimitiveEntityName;
        var mdmCode = descriptor.MdmCode;
        var name = descriptor.Name;
        var attribute = descriptor.LinkAttribute;

        var cachedAttributeExists = CacheManager.TryGetCachedAttribute(attribute.LinkedEntityInfoId, resultValue, out var cachedAttribute, out var entityId);
        if (cachedAttributeExists)
        {
            var valueSet = attribute.TrySetValue(cachedAttribute.Value, entityId);
            if (valueSet)
            {
                notification.AddValueChangeInfo(primitiveEntityName, mdmCode, name, entityId.ToString());
                notification.AddValidationAlert(primitiveEntityName, mdmCode, name, SeverityType.Info, resultMessage);
            }
        }
    }

    protected string GetHttpRequestExceptionMessage(HttpRequestException exception)
    {
        if (!exception.StatusCode.HasValue)
        {
            return "Сервис автоматической проверки не смог обработать запрос. Причина не установлена.";
        }

        var httpStatusCode = exception.StatusCode.Value;

        try
        {
            return ValidationData.HttpRequestMessages[httpStatusCode];
        }
        catch (KeyNotFoundException ex)
        {
            throw new KeyNotFoundException($"{nameof(ValidationData.HttpRequestMessages)} doesn't contain such key [{Enum.GetName(httpStatusCode)}].", ex);
        }
    }

    protected string GetValidationMessage(AttributeAssignment attributeAssignment, ValidationResultType validationResultType)
    {
        try
        {
            return ValidationData.ValidationMessages[attributeAssignment][validationResultType];
        }
        catch (KeyNotFoundException ex)
        {
            throw new KeyNotFoundException($"{nameof(ValidationData.ValidationMessages)} doesn't contain such key [{Enum.GetName(attributeAssignment)}, {Enum.GetName(validationResultType)}].", ex);
        }
    }

    protected string GetEnumerationBinding(EnumerationAssignment enumerationAssignment, EnumerationItemAssignment enumerationItemAssignment)
    {
        try
        {
            return ValidationData.EnumerationBindings[enumerationAssignment][enumerationItemAssignment];
        }
        catch (KeyNotFoundException ex)
        {
            throw new KeyNotFoundException($"{nameof(ValidationData.EnumerationBindings)} doesn't contain such key [{Enum.GetName(enumerationAssignment)}, {Enum.GetName(enumerationItemAssignment)}].", ex);
        }
    }

    protected async Task GetAndLogDailyStats()
    {
        var dailyStatsResponse = await ApiClient.ProfileClient.GetDailyStats();
        var balanceResponse = await ApiClient.ProfileClient.GetBalance();

        var dailyStats = dailyStatsResponse?.services;
        var balance = balanceResponse?.balance;

        Logger.LogInformation("Cleanse [{Value1}], Suggestions [{Value2}], Merging [{Value3}], Balance [{Value4}]", dailyStats.clean, dailyStats.suggestions, dailyStats.merging, balance);
    }
}

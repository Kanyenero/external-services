using Dadata.Model;
using FuzzySharp;
using Microsoft.Extensions.Logging;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Caching;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Enumerations;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Extensions;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.ApiDescriptors;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.Data;
using Navicon.Mdm.ExternalServices.Model.BusinessRules;
using static Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.ApiDescriptors.PartyApiDescriptor;

namespace Navicon.Mdm.ExternalServices.Infrastructure.DaData.Handlers;

internal class PartyApiHandler : ApiHandlerBase
{
    private PartyApiDescriptor _apiDescriptor;

    public PartyApiHandler(ILogger logger, IEntityListCacheManager cacheManager, ApiClient apiClient, ValidationData validationData)
        : base(logger, cacheManager, apiClient, validationData)
    {
    }

    public int PartySuggestionFuzzySearchThreshold { get; set; }

    public override async Task Handle(ApiDescriptorBase apiDescriptor, Notification notification)
    {
        _apiDescriptor = (apiDescriptor as PartyApiDescriptor) ?? throw new InvalidCastException($"Unexpected api descriptor type: '{apiDescriptor.GetType()}'. Expected type is '{nameof(PartyApiDescriptor)}'.");
        if (!_apiDescriptor.HasAnyDescriptor)
        {
            return;
        }

        var requestPrepared = TryPrepareRequest(out var findPartyRequest);
        if (!requestPrepared)
        {
            return;
        }

        var party = await TrySendRequest(findPartyRequest);
        if (party is null)
        {
            return;
        }

        await base.Handle();

        ApplyResults(party, notification);
    }

    private bool TryPrepareRequest(out FindPartyRequest request)
    {
        if (!_apiDescriptor.PartyName.PartyName.HasValue)
        {
            request = null;

            return false;
        }

        var inn = _apiDescriptor.Inn.Inn;
        var ogrn = _apiDescriptor.Ogrn.Ogrn;
        var kpp = _apiDescriptor.Kpp.Kpp;

        var query = inn.HasValue ? inn.Value : ogrn.HasValue ? ogrn.Value : null;

        if (query is null)
        {
            request = null;

            return false;
        }

        request = new FindPartyRequest(query: query, kpp: kpp.Value) { type = PartyType.LEGAL };

        return true;
    }

    private async Task<Party> TrySendRequest(FindPartyRequest request)
    {
        SuggestResponse<Party> response;
        try
        {
            response = await ApiClient.SuggestClient.FindParty(request);
        }
        catch (HttpRequestException ex)
        {
            var httpRequestExceptionMessage = GetHttpRequestExceptionMessage(ex);

            throw new HttpRequestException(httpRequestExceptionMessage, ex);
        }

        var partySuggestions = response?.suggestions;
        if (partySuggestions is not null && partySuggestions.Count > 0)
        {
            // Постановка по fuzzy search.
            // 1. В тот момент, когда ты получаешь от дадаты данные ЮЛ (не важно, по которому критерию, ИНН, КПП, ОГРН) нужна следующая доработка.
            // 1.1. Если от дадаты получена одна запись, то нужно из наименования дадаты вырезать ОПФ и сравнить нечетким поиском полученное название с названием из МДМ (название из МДМ будет уже без ОПФ). 
            //      Если названия будут похожи на >= 80% - то выполнять обогащение из дадаты. Если меньше 80% - не выполнять.
            // 1.2. Если из дадаты получено несколько записей, то их все надо действовать в два этапа:
            // 1.2.1. Вырезать ОПФ из названия дадаты и сравнить с названием из МДМ.
            //        Если найдется одна запись, у которой название = названию в МДМ, обогатить ее данными. В противном случае - не обогащать.
            // 1.2.2. Вырезать ОПФ из названия дадаты и сравнить нечетким поиском с названием из МДМ.
            //        Обогащать только в том случае, если есть только одна запись, похожая на >= 80% .В противном случае - не обогащать.
            // Т.е.при любом обогащении должны быть похожи наименования и должна быть однозначность.

            var partyName = _apiDescriptor.PartyName.PartyName.Value;
            var partySuggestion = PartySuggestionFuzzySearch(partySuggestions, partyName, PartySuggestionFuzzySearchThreshold);

            return partySuggestion;
        }

        return null;
    }

    private static Party PartySuggestionFuzzySearch(IList<Suggestion<Party>> partySuggestions, string partyName, int ratioThreshold = 80)
    {
        var matches = new List<Party>();
        var partyNameLowerCase = partyName.ToLowerInvariant();

        foreach (var partySuggestion in partySuggestions)
        {
            var party = partySuggestion.data;
            var partyNameSuggestedLowerCase = (party.name.full ?? party.name.@short).ToLowerInvariant();

            var ratio = Fuzz.Ratio(partyNameLowerCase, partyNameSuggestedLowerCase);
            if (ratio > ratioThreshold)
            {
                matches.Add(party);
            }
        }

        return matches.Count == 1 ? matches[0] : null;
    }

    private void ApplyResults(Party party, in Notification notification)
    {
        ApplyValidationResult(_apiDescriptor.Inn, party, notification);
        ApplyValidationResult(_apiDescriptor.Kpp, party, notification);
        ApplyValidationResult(_apiDescriptor.Opf, party, notification);
        ApplyValidationResult(_apiDescriptor.Ogrn, party, notification);
        ApplyValidationResult(_apiDescriptor.Okved, party, notification);
        ApplyValidationResult(_apiDescriptor.PartyName, party, notification);
        ApplyValidationResult(_apiDescriptor.PartyRegistrationDate, party, notification);

        if (_apiDescriptor.Address is not null)
        {
            ApplyValidationResult(_apiDescriptor.Address, party.address.data, notification);
        }
    }

    private void ApplyValidationResult(InnDescriptor descriptor, Party result, in Notification notification)
    {
        var resultValue = result.inn;
        var resultType = resultValue is null ? ValidationResultType.Failure : ValidationResultType.Success;
        var resultMessage = GetValidationMessage(AttributeAssignment.LegalEntityPartInn, resultType);

        ApplyValidationResult(descriptor.Inn, resultMessage, resultValue, notification);
    }

    private void ApplyValidationResult(OgrnDescriptor descriptor, Party result, in Notification notification)
    {
        var resultValue = result.ogrn;
        var resultType = resultValue is null ? ValidationResultType.Failure : ValidationResultType.Success;
        var resultMessage = GetValidationMessage(AttributeAssignment.LegalEntityPartOgrn, resultType);

        ApplyValidationResult(descriptor.Ogrn, resultMessage, resultValue, notification);
    }

    private void ApplyValidationResult(KppDescriptor descriptor, Party result, in Notification notification)
    {
        var resultValue = result.kpp;
        var resultType = resultValue is null ? ValidationResultType.Failure : ValidationResultType.Success;
        var resultMessage = GetValidationMessage(AttributeAssignment.LegalEntityPartKpp, resultType);

        ApplyValidationResult(descriptor.Kpp, resultMessage, resultValue, notification);
    }

    private void ApplyValidationResult(PartyNameDescriptor descriptor, Party result, in Notification notification)
    {
        var resultValue = result.name.full ?? result.name.@short;
        var resultType = resultValue is null ? ValidationResultType.Failure : ValidationResultType.Success;
        var resultMessage = GetValidationMessage(AttributeAssignment.LegalEntityPartName, resultType);

        ApplyValidationResult(descriptor.PartyName, resultMessage, resultValue, notification);
    }

    private void ApplyValidationResult(PartyRegistrationDateDescriptor descriptor, Party result, in Notification notification)
    {
        //var resultValue = result.state.registration_date?.ToString("o"); // utc
        var resultValue = !result.state.registration_date.HasValue || result.state.registration_date == DateTime.MinValue ? null : result.state.registration_date.Value.ToString("o");
        var resultType = resultValue is null ? ValidationResultType.Failure : ValidationResultType.Success;
        var resultMessage = GetValidationMessage(AttributeAssignment.LegalEntityPartRegistrationDate, resultType);

        ApplyValidationResult(descriptor.PartyRegistrationDate, resultMessage, resultValue, notification);
    }

    private void ApplyValidationResult(OkvedDescriptor descriptor, Party result, in Notification notification)
    {
        var resultValue = result.okved;
        var resultType = resultValue is null ? ValidationResultType.Failure : ValidationResultType.Success;
        var resultMessage = GetValidationMessage(AttributeAssignment.LegalEntityPartOkved, resultType);

        ApplyValidationResult(descriptor.Okved, resultMessage, resultValue, notification);
    }

    private void ApplyValidationResult(OpfDescriptor descriptor, Party result, in Notification notification)
    {
        var resultValue = result.opf.@short ?? result.opf.full;
        var resultType = resultValue is null ? ValidationResultType.Failure : ValidationResultType.Success;
        var resultMessage = GetValidationMessage(AttributeAssignment.LegalEntityPartOpf, resultType);

        ApplyValidationResult(descriptor.Opf, resultMessage, resultValue, notification);
    }

    private void ApplyValidationResult(AddressDescriptor descriptor, Address result, in Notification notification)
    {
        var countryResultValue = result.country;
        var countryResultType = countryResultValue is null ? ValidationResultType.Failure : ValidationResultType.Success;
        var countryResultMessage = GetValidationMessage(AttributeAssignment.AddressPartCountry, countryResultType);

        ApplyValidationResult(descriptor.Country, countryResultMessage, countryResultValue, notification);

        var cityResultValue = result.city;
        var cityResultType = cityResultValue is null ? ValidationResultType.Failure : ValidationResultType.Success;
        var cityResultMessage = GetValidationMessage(AttributeAssignment.AddressPartCity, cityResultType);

        ApplyValidationResult(descriptor.City, cityResultMessage, cityResultValue, notification);

        var addressResultValue = result.result ?? result.source;
        var addressResultType = result.DetailValidationResultCode();
        var addressResultMessage = GetValidationMessage(AttributeAssignment.Address, addressResultType);

        ApplyValidationResult(descriptor.Address, addressResultMessage, addressResultValue, notification);

        var fiasResultValue = result.fias_id;
        var fiasResultType = fiasResultValue is null ? ValidationResultType.Failure : ValidationResultType.Success;
        var fiasResultMessage = GetValidationMessage(AttributeAssignment.AddressPartFias, fiasResultType);

        ApplyValidationResult(descriptor.Fias, fiasResultMessage, fiasResultValue, notification);
    }
}

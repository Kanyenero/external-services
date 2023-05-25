using Dadata.Model;
using Microsoft.Extensions.Logging;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Caching;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Enumerations;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Extensions;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.ApiDescriptors;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.Data;
using Navicon.Mdm.ExternalServices.Model.BusinessRules;
using static Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.ApiDescriptors.CompositePersonRecordApiDescriptor;
using AddressDescriptor = Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.ApiDescriptors.CompositePersonRecordApiDescriptor.AddressDescriptor;

namespace Navicon.Mdm.ExternalServices.Infrastructure.DaData.Handlers;

internal class CompositePersonRecordApiHandler : ApiHandlerBase
{
    private CompositePersonRecordApiDescriptor _apiDescriptor;

    public CompositePersonRecordApiHandler(ILogger logger, IEntityListCacheManager cacheManager, ApiClient apiClient, ValidationData validationData)
        : base(logger, cacheManager, apiClient, validationData)
    {
    }

    public override async Task Handle(ApiDescriptorBase apiDescriptor, Notification notification)
    {
        _apiDescriptor = (apiDescriptor as CompositePersonRecordApiDescriptor) ?? throw new InvalidCastException($"Unexpected api descriptor type: '{apiDescriptor.GetType()}'. Expected type is '{nameof(CompositePersonRecordApiDescriptor)}'.");
        if (!_apiDescriptor.HasAnyDescriptor)
        {
            return;
        }

        var requestPrepared = TryPrepareRequest(out var structure, out var data);
        if (!requestPrepared)
        {
            return;
        }

        var response = await TrySendRequest(structure, data);
        if (response is null)
        {
            return;
        }

        await base.Handle();

        ApplyResults(response, notification);
    }

    private bool TryPrepareRequest(out List<StructureType> structure, out List<string> data)
    {
        structure = new List<StructureType>();
        data = new List<string>();

        if (_apiDescriptor.Fullname is not null)
        {
            structure.Add(StructureType.NAME);
            data.Add(_apiDescriptor.Fullname.ToString());
        }
        if (_apiDescriptor.Addresses is not null)
        {
            foreach (var address in _apiDescriptor.Addresses)
            {
                structure.Add(StructureType.ADDRESS);
                data.Add(address.ToString());
            }
        }

        if (structure.Count == 0 || data.Count == 0)
        {
            return false;
        }

        return true;
    }

    private async Task<IList<IDadataEntity>> TrySendRequest(List<StructureType> structure, List<string> data)
    {
        IList<IDadataEntity> response;
        try
        {
            response = await ApiClient.CleanClient.Clean(structure, data);
        }
        catch (HttpRequestException ex)
        {
            var httpRequestExceptionMessage = GetHttpRequestExceptionMessage(ex);

            throw new HttpRequestException(httpRequestExceptionMessage, ex);
        }

        if (response is not null && response.Count > 0)
        {
            return response;
        }

        return null;
    }

    private void ApplyResults(IList<IDadataEntity> response, in Notification notification)
    {
        if (_apiDescriptor.Fullname is not null)
        {
            var fullname = response.OfType<Fullname>().FirstOrDefault();

            ApplyValidationResult(_apiDescriptor.Fullname, fullname, notification);
        }
        if (_apiDescriptor.Addresses is not null)
        {
            var addresses = response.OfType<Address>();

            var zipped = _apiDescriptor.Addresses.Zip(addresses);

            foreach (var zip in zipped)
            {
                ApplyValidationResult(zip.First, zip.Second, notification);
            }
        }
    }

    private void ApplyValidationResult(FullnameDescriptor descriptor, Fullname result, in Notification notification)
    {
        var resultType = result.DetailValidationResultCode();

        var resultValueFullname = result.result;
        var resultMessageFullname = GetValidationMessage(AttributeAssignment.Fullname, resultType);
        ApplyValidationResult(descriptor.Fullname, resultMessageFullname, resultValueFullname, notification);

        var resultValueSurname = result.surname;
        var resultMessageSurname = GetValidationMessage(AttributeAssignment.FullnamePartSurname, resultType);
        ApplyValidationResult(descriptor.Surname, resultMessageSurname, resultValueSurname, notification);

        var resultValueName = result.name;
        var resultMessageName = GetValidationMessage(AttributeAssignment.FullnamePartName, resultType);
        ApplyValidationResult(descriptor.Name, resultMessageName, resultValueName, notification);

        var resultValuePatronymic = result.patronymic;
        var resultMessagePatronymic = GetValidationMessage(AttributeAssignment.FullnamePartPatronymic, resultType);
        ApplyValidationResult(descriptor.Patronymic, resultMessagePatronymic, resultValuePatronymic, notification);

        EnumerationItemAssignment? genderType = result.gender == Constants.GenderMale ? EnumerationItemAssignment.Male : result.gender == Constants.GenderFemale ? EnumerationItemAssignment.Female : null;

        if (genderType.HasValue)
        {
            var resultValueGender = GetEnumerationBinding(EnumerationAssignment.Gender, genderType.Value);
            var resultTypeGender = result.gender == Constants.GenderMale || result.gender == Constants.GenderFemale ? ValidationResultType.Success : ValidationResultType.Failure;
            var resultMessageGender = GetValidationMessage(AttributeAssignment.Gender, resultTypeGender);

            ApplyValidationResult(descriptor.Gender, resultMessageGender, resultValueGender, notification);
        }
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

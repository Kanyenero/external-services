using Dadata.Model;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Enumerations;

namespace Navicon.Mdm.ExternalServices.Infrastructure.DaData.Extensions;

internal static class DaDataEntityExtensions
{
    public static ValidationResultType DetailValidationResultCode(this IDadataEntity entity) => entity is null
        ? throw new ArgumentNullException(nameof(entity))
        : entity switch
        {
            Address address => int.Parse(address.qc) switch
            {
                0 => ValidationResultType.Success,
                1 => ValidationResultType.NotEnoughData,
                2 => ValidationResultType.Junk,
                3 => ValidationResultType.HasAlternatives,

                _ => throw new ArgumentException($"Unexpected [{nameof(address.qc)}]: '{address.qc}'.")
            },
            Birthdate birthdate => int.Parse(birthdate.qc) switch
            {
                0 => ValidationResultType.Success,
                1 => ValidationResultType.PartialSuccess,
                2 => ValidationResultType.Junk,

                _ => throw new ArgumentException($"Unexpected [{nameof(birthdate.qc)}]: '{birthdate.qc}'.")
            },
            Email email => int.Parse(email.qc) switch
            {
                0 => ValidationResultType.Success,
                1 => ValidationResultType.Failure,
                2 => ValidationResultType.Junk,
                3 => ValidationResultType.Throwaway,
                4 => ValidationResultType.Corrected,

                _ => throw new ArgumentException($"Unexpected [{nameof(email.qc)}]: '{email.qc}'.")
            },
            Fullname fullname => int.Parse(fullname.qc) switch
            {
                0 => ValidationResultType.Success,
                1 => ValidationResultType.PartialSuccess,
                2 => ValidationResultType.Junk,

                _ => throw new ArgumentException($"Unexpected [{nameof(fullname.qc)}]: '{fullname.qc}'.")
            },
            Phone phone => int.Parse(phone.qc) switch
            {
                0 => ValidationResultType.Success,
                1 => ValidationResultType.PartialSuccess,
                2 => ValidationResultType.Junk,
                3 => ValidationResultType.Several,
                7 => ValidationResultType.Foreign,

                _ => throw new ArgumentException($"Unexpected [{nameof(phone.qc)}]: '{phone.qc}'.")
            },
            Vehicle vehicle => int.Parse(vehicle.qc) switch
            {
                0 => ValidationResultType.Success,
                1 => ValidationResultType.PartialSuccess,
                2 => ValidationResultType.Junk,
                3 => ValidationResultType.BrandRecognized,

                _ => throw new ArgumentException($"Unexpected [{nameof(vehicle.qc)}]: '{vehicle.qc}'.")
            },

            _ => throw new ArgumentException($"Unexpected [{nameof(IDadataEntity)}] derived type: '{entity.GetType()}'.")
        };

    public static string DetailFias(this Address address)
    {
        ArgumentNullException.ThrowIfNull(address, nameof(address));

        var detalizedFiasLevel = int.Parse(address.fias_level) switch
        {
            0   => "Страна",
            1   => "Регион",
            3   => "Район",
            4   => "Город",
            5   => "Район города",
            6   => "Населенный пункт",
            7   => "Улица",
            8   => "Дом",
            9   => "Квартира",
            65  => "Планировочная структура",
            90  => "Дополнительная территория",
            91  => "Улица в дополнительной территории",
            -1  => "Иностранный или пустой",

            _ => throw new ArgumentException($"Unexpected fias level: '{address.fias_level}'.")
        };

        var detalizedFiasActualityState = int.Parse(address.fias_actuality_state) switch
        {
            0               => "Актуальный",
            51              => "Переподчинен",
            99              => "Удален",
            > 0 and < 51    => "Переименован",

            _ => throw new ArgumentException($"Unexpected fias actuality state: '{address.fias_actuality_state}'.")
        };

        var format = "Сервис автоматической проверки детализировал адрес в системе ФИАС до уровня [{0}] и установил признак актуальности адреса - [{1}].";

        return string.Format(format, detalizedFiasLevel, detalizedFiasActualityState);
    }
}

using Navicon.Mdm.ExternalServices.Extensions.Data;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.AttributeDescriptors;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.EntityDataDescriptors;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.EntityDescriptors;

namespace Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.ApiDescriptors;

/*
 * 
 *  https://dadata.ru/api/clean/record/
 *
 *  Максимальное количество полей в одной записи:
 *  1 ФИО,
 *  3 адреса,
 *  3 телефона,
 *  3 email,
 *  1 дата рождения,
 *  1 паспорт,
 *  1 автомобиль.
 *
 */

internal class CompositePersonRecordApiDescriptor : ApiDescriptorBase
{
    public const int MaxAddresses = 3;

    public FullnameDescriptor Fullname { get; set; }

    public ICollection<AddressDescriptor> Addresses { get; set; }

    public override bool HasAnyDescriptor => Fullname is not null || Addresses is not null && Addresses.Any();

    public override void Collect(EntityDescriptorBase source)
    {
        var descriptor = (source as PhysicalPersonEntityDescriptor) ?? throw new InvalidCastException($"Unexpected entity descriptor: '{source.GetType()}'.");

        var dataDescriptor = descriptor.DataDescriptor;
        var addressDataDescriptors = descriptor.AddressDataDescriptors?.TakeLast(MaxAddresses);

        Fullname = collectFullname(dataDescriptor);
        Addresses = addressDataDescriptors is null ? null : collectAddresses(addressDataDescriptors).AsList();

        static FullnameDescriptor collectFullname(PhysicalPersonEntityDataDescriptor data) => data.Fullname.HasValue || data.Surname.HasValue || data.Name.HasValue || data.Patronymic.HasValue
            ? new FullnameDescriptor
            {
                Fullname = data.Fullname,
                Surname = data.Surname,
                Name = data.Name,
                Patronymic = data.Patronymic,
                Gender = data.Gender
            }
            : null;

        static IEnumerable<AddressDescriptor> collectAddresses(IEnumerable<AddressEntityDataDescriptor> dataCollection)
        {
            foreach (var data in dataCollection)
            {
                if (data is null || !data.Address.HasValue)
                {
                    continue;
                }

                yield return new AddressDescriptor
                {
                    Address = data.Address,
                    Country = data.Country,
                    City = data.City,
                    Fias = data.Fias
                };
            }
        }
    }

    public class FullnameDescriptor
    {
        public AttributeDescriptor Fullname { get; set; }

        public AttributeDescriptor Surname { get; set; }

        public AttributeDescriptor Name { get; set; }

        public AttributeDescriptor Patronymic { get; set; }

        public AttributeDescriptor Gender { get; set; }

        public bool PartExists => Surname is not null && Surname.HasValue || Name is not null && Name.HasValue || Patronymic is not null && Patronymic.HasValue;

        public override string ToString() => PartExists ? string.Join(' ', Surname?.Value, Name?.Value, Patronymic?.Value).Trim() : Fullname?.Value;
    }

    public class AddressDescriptor
    {
        public LinkAttributeDescriptor Country { get; set; }

        public LinkAttributeDescriptor City { get; set; }

        public AttributeDescriptor Fias { get; set; }

        public AttributeDescriptor Address { get; set; }

        public override string ToString()
        {
            if (Address is not null && Address.HasValue)
            {
                var result = string.Empty;

                if (Country is not null && Country.HasValue && !Address.Value.Contains(Country.Value))
                {
                    result += Country.Value;
                    result += ", ";
                }
                if (City is not null && City.HasValue && !Address.Value.Contains(City.Value))
                {
                    result += City.Value;
                    result += ", ";
                }

                result += Address.Value;

                return result.Trim(' ', ',');
            }

            return string.Join(", ", Country?.Value, City?.Value).Trim(' ', ',');
        }
    }
}

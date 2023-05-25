using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.AttributeDescriptors;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.EntityDescriptors;

namespace Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.ApiDescriptors;

/*
 * 
 *  https://dadata.ru/api/find-party/
 *
 */

internal class PartyApiDescriptor : ApiDescriptorBase
{
    public InnDescriptor Inn { get; set; }

    public OgrnDescriptor Ogrn { get; set; }

    public KppDescriptor Kpp { get; set; }

    public PartyNameDescriptor PartyName { get; set; }

    public PartyRegistrationDateDescriptor PartyRegistrationDate { get; set; }

    public OkvedDescriptor Okved { get; set; }

    public OpfDescriptor Opf { get; set; }

    public AddressDescriptor Address { get; set; }

    public override bool HasAnyDescriptor => Inn is not null || Ogrn is not null;

    public override void Collect(EntityDescriptorBase source)
    {
        var descriptor = (source as LegalEntityEntityDescriptor) ?? throw new InvalidCastException($"Unexpected entity descriptor type: '{source.GetType()}'. Expected type is '{nameof(LegalEntityEntityDescriptor)}'.");

        var dataDescriptor = descriptor.DataDescriptor;
        var addressDataDescriptor = descriptor.AddressDataDescriptors?.FirstOrDefault();

        Inn = dataDescriptor.Inn.HasValue ? new InnDescriptor { Inn = dataDescriptor.Inn } : null;

        if (Inn is not null)
        {
            Ogrn = new OgrnDescriptor { Ogrn = dataDescriptor.Ogrn };
            Kpp = new KppDescriptor { Kpp = dataDescriptor.Kpp };
            Opf = new OpfDescriptor { Opf = dataDescriptor.Opf };
            Okved = new OkvedDescriptor { Okved = dataDescriptor.Okved };
            PartyName = new PartyNameDescriptor { PartyName = dataDescriptor.Name };
            PartyRegistrationDate = new PartyRegistrationDateDescriptor { PartyRegistrationDate = dataDescriptor.RegistrationDate };
            Address = addressDataDescriptor is null ? null : new AddressDescriptor { Country = addressDataDescriptor.Country, City = addressDataDescriptor.City, Address = addressDataDescriptor.Address, Fias = addressDataDescriptor.Fias };

            return;
        }

        Ogrn = dataDescriptor.Ogrn.HasValue ? new OgrnDescriptor { Ogrn = dataDescriptor.Ogrn } : null;

        if (Ogrn is not null)
        {
            Inn = new InnDescriptor { Inn = dataDescriptor.Inn };
            Kpp = new KppDescriptor { Kpp = dataDescriptor.Kpp };
            Opf = new OpfDescriptor { Opf = dataDescriptor.Opf };
            Okved = new OkvedDescriptor { Okved = dataDescriptor.Okved };
            PartyName = new PartyNameDescriptor { PartyName = dataDescriptor.Name };
            PartyRegistrationDate = new PartyRegistrationDateDescriptor { PartyRegistrationDate = dataDescriptor.RegistrationDate };
            Address = addressDataDescriptor is null ? null : new AddressDescriptor { Country = addressDataDescriptor.Country, City = addressDataDescriptor.City, Address = addressDataDescriptor.Address, Fias = addressDataDescriptor.Fias };
        }
    }

    public class InnDescriptor
    {
        public AttributeDescriptor Inn { get; set; }

        public override string ToString() => Inn?.Value;
    }

    public class OgrnDescriptor
    {
        public AttributeDescriptor Ogrn { get; set; }

        public override string ToString() => Ogrn?.Value;
    }

    public class KppDescriptor
    {
        public AttributeDescriptor Kpp { get; set; }

        public override string ToString() => Kpp?.Value;
    }

    public class PartyNameDescriptor
    {
        public AttributeDescriptor PartyName { get; set; }

        public override string ToString() => PartyName?.Value;
    }

    public class PartyRegistrationDateDescriptor
    {
        public AttributeDescriptor PartyRegistrationDate { get; set; }

        public override string ToString() => PartyRegistrationDate?.Value;
    }

    public class OkvedDescriptor
    {
        public AttributeDescriptor Okved { get; set; }

        public override string ToString() => Okved?.Value;
    }

    public class OpfDescriptor
    {
        public LinkAttributeDescriptor Opf { get; set; }

        public override string ToString() => Opf?.Value;
    }

    public class AddressDescriptor
    {
        public LinkAttributeDescriptor Country { get; set; }

        public LinkAttributeDescriptor City { get; set; }

        public AttributeDescriptor Address { get; set; }

        public AttributeDescriptor Fias { get; set; }

        public override string ToString() => Address?.Value;
    }
}

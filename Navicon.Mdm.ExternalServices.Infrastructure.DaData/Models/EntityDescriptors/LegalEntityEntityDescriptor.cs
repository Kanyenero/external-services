using Navicon.Mdm.ExternalServices.Extensions.Entities;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Caching;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Enumerations;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.Data;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.EntityDataDescriptors;
using Navicon.Mdm.ExternalServices.Model.Entities;

namespace Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.EntityDescriptors;

internal class LegalEntityEntityDescriptor : EntityDescriptorBase
{
    public LegalEntityEntityDescriptor(EntityData entityData, IEntityListCacheManager cacheManager) : base(entityData, cacheManager)
    {
    }

    public LegalEntityEntityDataDescriptor DataDescriptor { get; } = new();

    public ICollection<AddressEntityDataDescriptor> AddressDataDescriptors { get; set; }

    public override void Collect(Entity entity)
    {
        collectEntityData(entity);
        collectChildren(entity);

        void collectEntityData(Entity entity)
        {
            var entityData = entity.EntityData;

            DataDescriptor.PrimitiveEntityName = entityData.PrimitiveEntityName;
            DataDescriptor.MdmCode = entityData.MdmCode;

            var innExists = entityData.TryGetAttribute(EntityData.AttributeNames[AttributeAssignment.LegalEntityPartInn], out var inn);
            if (innExists)
            {
                DataDescriptor.Inn.Attribute = inn;
            }

            var ogrnExists = entityData.TryGetAttribute(EntityData.AttributeNames[AttributeAssignment.LegalEntityPartOgrn], out var ogrn);
            if (ogrnExists)
            {
                DataDescriptor.Ogrn.Attribute = ogrn;
            }

            var kppExists = entityData.TryGetAttribute(EntityData.AttributeNames[AttributeAssignment.LegalEntityPartKpp], out var kpp);
            if (kppExists)
            {
                DataDescriptor.Kpp.Attribute = kpp;
            }

            var okvedExists = entityData.TryGetAttribute(EntityData.AttributeNames[AttributeAssignment.LegalEntityPartOkved], out var okved);
            if (okvedExists)
            {
                DataDescriptor.Okved.Attribute = okved;
            }

            var nameExists = entityData.TryGetAttribute(EntityData.AttributeNames[AttributeAssignment.LegalEntityPartName], out var name);
            if (nameExists)
            {
                DataDescriptor.Name.Attribute = name;
            }

            var dateExists = entityData.TryGetAttribute(EntityData.AttributeNames[AttributeAssignment.LegalEntityPartRegistrationDate], out var date);
            if (dateExists)
            {
                DataDescriptor.RegistrationDate.Attribute = date;
            }

            var opfExists = entityData.TryGetLinkAttribute(EntityData.AttributeNames[AttributeAssignment.LegalEntityPartOpf], out var opf);
            if (opfExists)
            {
                DataDescriptor.Opf.LinkAttribute = opf;
            }
        }

        void collectChildren(Entity entity)
        {
            var addressChildrenExists = entity.TryGetChildren(EntityData.EntityNames[EntityAssignment.Address], out var addressChildren);
            if (!addressChildrenExists)
            {
                return;
            }

            AddressDataDescriptors = new List<AddressEntityDataDescriptor>();

            foreach (var addressChild in addressChildren)
            {
                var addressChildData = addressChild.EntityData;

                var countryExists = addressChildData.TryGetLinkAttribute(EntityData.AttributeNames[AttributeAssignment.AddressPartCountry], out var country);
                var cityExists = addressChildData.TryGetLinkAttribute(EntityData.AttributeNames[AttributeAssignment.AddressPartCity], out var city);
                var addressExists = addressChildData.TryGetAttribute(EntityData.AttributeNames[AttributeAssignment.Address], out var address);
                var fiasExists = addressChildData.TryGetAttribute(EntityData.AttributeNames[AttributeAssignment.AddressPartFias], out var fias);

                if (countryExists || cityExists || addressExists || fiasExists)
                {
                    var entityDataDescriptor = new AddressEntityDataDescriptor
                    {
                        PrimitiveEntityName = addressChildData.PrimitiveEntityName,
                        MdmCode = addressChildData.MdmCode
                    };
                    entityDataDescriptor.Country.LinkAttribute = country;
                    entityDataDescriptor.City.LinkAttribute = city;
                    entityDataDescriptor.Address.Attribute = address;
                    entityDataDescriptor.Fias.Attribute = fias;

                    AddressDataDescriptors.Add(entityDataDescriptor);
                }
            }
        }
    }
}

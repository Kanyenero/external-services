using Navicon.Mdm.ExternalServices.Extensions.Entities;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Caching;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Enumerations;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.Data;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.EntityDataDescriptors;
using Navicon.Mdm.ExternalServices.Model.Entities;

namespace Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.EntityDescriptors;

internal class PhysicalPersonEntityDescriptor : EntityDescriptorBase
{
    public PhysicalPersonEntityDescriptor(EntityData entityData, IEntityListCacheManager cacheManager) : base(entityData, cacheManager)
    {
    }

    public PhysicalPersonEntityDataDescriptor DataDescriptor { get; } = new();

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

            var fullnameExists = entityData.TryGetAttribute(EntityData.AttributeNames[AttributeAssignment.Fullname], out var fullname);
            if (fullnameExists)
            {
                DataDescriptor.Fullname.Attribute = fullname;
            }

            var surnameExists = entityData.TryGetAttribute(EntityData.AttributeNames[AttributeAssignment.FullnamePartSurname], out var surname);
            if (surnameExists)
            {
                DataDescriptor.Surname.Attribute = surname;
            }

            var nameExists = entityData.TryGetAttribute(EntityData.AttributeNames[AttributeAssignment.FullnamePartName], out var name);
            if (nameExists)
            {
                DataDescriptor.Name.Attribute = name;
            }

            var patronymicExists = entityData.TryGetAttribute(EntityData.AttributeNames[AttributeAssignment.FullnamePartPatronymic], out var patronymic);
            if (patronymicExists)
            {
                DataDescriptor.Patronymic.Attribute = patronymic;
            }

            var genderExists = entityData.TryGetAttribute(EntityData.AttributeNames[AttributeAssignment.Gender], out var gender);
            if (genderExists)
            {
                DataDescriptor.Gender.Attribute = gender;
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
                if (countryExists)
                {
                    if (country.LinkedEntityId.HasValue)
                    {
                        var cachedAttributeExists = CacheManager.TryGetCachedAttribute(country.LinkedEntityInfoId, country.LinkedEntityId, out var cachedAttribute);
                        if (cachedAttributeExists)
                        {
                            country.Value = cachedAttribute.Value;
                        }
                    }
                }

                var cityExists = addressChildData.TryGetLinkAttribute(EntityData.AttributeNames[AttributeAssignment.AddressPartCity], out var city);
                if (cityExists)
                {
                    if (city.LinkedEntityId.HasValue)
                    {
                        var cachedAttributeExists = CacheManager.TryGetCachedAttribute(city.LinkedEntityInfoId, city.LinkedEntityId, out var cachedAttribute);
                        if (cachedAttributeExists)
                        {
                            city.Value = cachedAttribute.Value;
                        }
                    }
                }

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

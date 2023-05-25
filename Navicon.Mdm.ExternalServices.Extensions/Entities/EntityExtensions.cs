using Navicon.Mdm.ExternalServices.Extensions.Data;
using Navicon.Mdm.ExternalServices.Model.Entities;
using Navicon.Mdm.MasterData.BusinessRules.Dto;
using Navicon.Mdm.MasterData.BusinessRules.Dto.Extensions;
using Attribute = Navicon.Mdm.ExternalServices.Model.Entities.Attribute;

namespace Navicon.Mdm.ExternalServices.Extensions.Entities;

public static class EntityExtensions
{
    public static Entity ToEntity(this EntityDto entityDto)
    {
        ArgumentNullException.ThrowIfNull(entityDto, nameof(entityDto));

        var entityChildDto = entityDto.AsEntityChild();
        return toEntityRecursively(entityChildDto);

        static Entity toEntityRecursively(EntityChildDto source) => source is null
            ? null
            : new Entity
            {
                EntityData = new PrimitiveEntity
                {
                    PrimitiveEntityName = source.EntityData?.PrimitiveEntityName,
                    MdmCode = source.EntityData?.MdmCode,
                    Attributes = source.EntityData?.Attributes?.Select(attributeDto =>
                    {
                        var attribute = attributeDto.AttributeType switch
                        {
                            AttributeTypeDto.String => new Attribute { AttributeType = AttributeType.String },
                            AttributeTypeDto.Number => new Attribute { AttributeType = AttributeType.Number },
                            AttributeTypeDto.DateTime => new Attribute { AttributeType = AttributeType.DateTime },
                            AttributeTypeDto.Bool => new Attribute { AttributeType = AttributeType.Bool },
                            AttributeTypeDto.Enumeration => new Attribute { AttributeType = AttributeType.Enumeration },
                            AttributeTypeDto.File => new Attribute { AttributeType = AttributeType.File },
                            AttributeTypeDto.Date => new Attribute { AttributeType = AttributeType.Date },
                            AttributeTypeDto.Multiline => new Attribute { AttributeType = AttributeType.Multiline },
                            AttributeTypeDto.FileLink => new Attribute { AttributeType = AttributeType.FileLink },
                            _ => throw new InvalidOperationException($"Unexpected [{nameof(AttributeTypeDto)}]: '{Enum.GetName(attributeDto.AttributeType)}'."),
                        };

                        attribute.Name = attributeDto.Name;
                        attribute.Value = attributeDto.Value;

                        return attribute;
                    }).AsList(),
                    LinkAttributes = source.EntityData?.LinkAttributes?.Select(linkAttributeDto => new LinkAttribute
                    {
                        LinkedEntityInfoId = linkAttributeDto.LinkedEntityInfoId,
                        LinkedEntityId = linkAttributeDto.LinkedEntityId,
                        IsMultiLink = linkAttributeDto.IsMultiLink,
                        Name = linkAttributeDto.Name
                    }).AsList(),
                },
                Children = source.Children?.Select(toEntityRecursively).AsList()
            };
    }

    public static EntityDto ToEntityDto(this Entity entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        var converted = toEntityDtoRecursively(entity);

        return new EntityDto
        {
            EntityData = converted.EntityData,
            Children = converted.Children
        };

        static EntityChildDto toEntityDtoRecursively(Entity source) => source is null
            ? null
            : new EntityChildDto
            {
                EntityData = new PrimitiveEntityDto
                {
                    PrimitiveEntityName = source.EntityData?.PrimitiveEntityName,
                    MdmCode = source.EntityData?.MdmCode,
                    Attributes = source.EntityData?.Attributes?.Select(attribute =>
                    {
                        var attributeDto = attribute.AttributeType switch
                        {
                            AttributeType.String => new AttributeDto { AttributeType = AttributeTypeDto.String },
                            AttributeType.Number => new AttributeDto { AttributeType = AttributeTypeDto.Number },
                            AttributeType.DateTime => new AttributeDto { AttributeType = AttributeTypeDto.DateTime },
                            AttributeType.Bool => new AttributeDto { AttributeType = AttributeTypeDto.Bool },
                            AttributeType.Enumeration => new AttributeDto { AttributeType = AttributeTypeDto.Enumeration },
                            AttributeType.File => new AttributeDto { AttributeType = AttributeTypeDto.File },
                            AttributeType.Date => new AttributeDto { AttributeType = AttributeTypeDto.Date },
                            AttributeType.Multiline => new AttributeDto { AttributeType = AttributeTypeDto.Multiline },
                            AttributeType.FileLink => new AttributeDto { AttributeType = AttributeTypeDto.FileLink },
                            _ => throw new InvalidOperationException($"Unexpected [{nameof(AttributeType)}]: '{Enum.GetName(attribute.AttributeType)}'."),
                        };
                        attributeDto.Name = attribute.Name;
                        attributeDto.Value = attribute.Value;

                        return attributeDto;
                    }).AsList(createEmptyListIfCastingFailed: true),
                    LinkAttributes = source.EntityData?.LinkAttributes?.Select(linkAttribute => new LinkAttributeDto
                    {
                        LinkedEntityInfoId = (int)linkAttribute.LinkedEntityInfoId,
                        LinkedEntityId = linkAttribute.LinkedEntityId,
                        IsMultiLink = linkAttribute.IsMultiLink,
                        Name = linkAttribute.Name
                    }).AsList(createEmptyListIfCastingFailed: true)
                },
                Children = source.Children?.Select(toEntityDtoRecursively).AsList()
            };
    }

    public static bool TryGetChild(this Entity entity, string primitiveEntityName, string mdmCode, out Entity entityChild)
    {
        if (entity is null || primitiveEntityName is null || mdmCode is null)
        {
            entityChild = null;

            return false;
        }

        entityChild = entity.Children?.SingleOrDefault(entityChild =>
            string.Equals(entityChild.EntityData.PrimitiveEntityName, primitiveEntityName, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(entityChild.EntityData.MdmCode, mdmCode, StringComparison.OrdinalIgnoreCase));

        return entityChild is not null;
    }

    public static bool TryGetChildren(this Entity entity, string primitiveEntityName, out IEnumerable<Entity> children)
    {
        if (entity is null || primitiveEntityName is null)
        {
            children = null;

            return false;
        }

        children = entity.Children?.Where(child => string.Equals(child.EntityData.PrimitiveEntityName, primitiveEntityName, StringComparison.OrdinalIgnoreCase));

        return children is not null;
    }
}

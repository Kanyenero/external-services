using Navicon.Mdm.ExternalServices.Dto.MdmApi;
using Navicon.Mdm.ExternalServices.Extensions.Data;
using Navicon.Mdm.ExternalServices.Model.Entities;
using Navicon.Mdm.ExternalServices.Model.MdmApi;
using Attribute = Navicon.Mdm.ExternalServices.Model.Entities.Attribute;

namespace Navicon.Mdm.ExternalServices.Extensions.MdmApi;

public static class EntityListExtensions
{
    public static EntityList ToEntityList(this EntityListDto source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        return new()
        {
            Entities = source.Entities.Select(toEntityListItem).AsList(),
            ScrollKey = source.ScrollKey,
            Total = source.Total
        };

        static EntityListItem toEntityListItem(EntityListItemDto dto) => dto is null
            ? null
            : new EntityListItem
            {
                EntityId = dto.EntityId,
                EntityInfoId = dto.EntityInfoId,
                Code = dto.Code,
                Title = dto.Title,
                AttributeList = dto.AttributeList.Select(attributeDto =>
                {
                    var attributeType =
                        attributeDto.StringValue is null
                            ? attributeDto.BoolValue is null
                                ? attributeDto.DateValue is null
                                    ? attributeDto.DateTimeValue is null
                                        ? attributeDto.NumberValue is null
                                            ? attributeDto.MultilineValue is null
                                                ? AttributeType.String // default
                                                : AttributeType.Multiline
                                            : AttributeType.Number
                                        : AttributeType.DateTime
                                    : AttributeType.Date
                                : AttributeType.Bool
                            : AttributeType.String;

                    return new Attribute
                    {
                        AttributeInfoId = attributeDto.AttributeInfoId,
                        AttributeType = attributeType,
                        Value = attributeType switch
                        {
                            AttributeType.String     => attributeDto.StringValue,
                            AttributeType.Date       => attributeDto.DateValue.HasValue ? attributeDto.DateValue.Value.ToString() : null,
                            AttributeType.DateTime   => attributeDto.DateTimeValue.HasValue ? attributeDto.DateTimeValue.Value.ToString() : null,
                            AttributeType.Bool       => attributeDto.BoolValue.HasValue ? attributeDto.BoolValue.Value.ToString() : null,
                            AttributeType.Number     => attributeDto.NumberValue.HasValue ? attributeDto.NumberValue.Value.ToString() : null,
                            AttributeType.Multiline  => attributeDto.MultilineValue,
                            _ => throw new InvalidOperationException($"Unexpected [{nameof(AttributeType)}]: '{Enum.GetName(attributeType)}'.")
                        }
                    };
                }).AsList(),
                LinkAttributeList = dto.AttributeList
                    .Where(attributeDto => attributeDto.LinkedEntityId is not null && attributeDto.LinkedEntityInfoId is not null)
                    .Select(attributeDto => new LinkAttribute
                    {
                        LinkedEntityId = attributeDto.LinkedEntityId,
                        LinkedEntityInfoId = attributeDto.LinkedEntityInfoId ?? default,
                        IsMultiLink = attributeDto.IsMultiLink
                    }).AsList()
            };

    }

    public static bool TryGetEntityListItem(this EntityList entityList, long? entityId, out EntityListItem entityListItem)
    {
        if (entityList is null || !entityId.HasValue)
        {
            entityListItem = null;

            return false;
        }

        entityListItem = entityList.Entities?.FirstOrDefault(item => item.EntityId == entityId);

        return entityListItem is not null;
    }

    public static bool TryGetEntityListItemAttribute(this EntityList entityList, string attributeValue, out Attribute attribute, out long? entityId)
    {
        if (entityList is null || string.IsNullOrWhiteSpace(attributeValue))
        {
            attribute = null;
            entityId = null;

            return false;
        }

        var entityListItem = entityList.Entities?.FirstOrDefault(item => item.AttributeList.Any(attribute => attribute.Value == attributeValue));
        if (entityListItem is null)
        {
            attribute = null;
            entityId = null;

            return false;
        }

        entityId = entityListItem.EntityId;
        attribute = entityListItem.AttributeList.FirstOrDefault(attribute => attribute.Value == attributeValue);

        return attribute is not null;
    }
}

using Microsoft.Extensions.Caching.Memory;
using Navicon.Mdm.ExternalServices.Model.MdmApi;
using Attribute = Navicon.Mdm.ExternalServices.Model.Entities.Attribute;

namespace Navicon.Mdm.ExternalServices.Infrastructure.DaData.Caching;

/// <summary>
/// Представляет контракт менеджера кэша объектов <see cref="EntityList"/>.
/// </summary>
public interface IEntityListCacheManager
{
    /// <summary>
    /// Позволяет получить признак заполненности кэша.
    /// </summary>
    bool CacheIsEmpty { get; }

    /// <summary>
    /// Позволяет создать или перезаписать запись в кэше.
    /// </summary>
    /// <param name="entityInfoId">Идентификатор данных сущности, являющийся ключом записи.</param>
    /// <returns>Новый экземпляр <see cref="ICacheEntry"/>.</returns>
    ICacheEntry CreateEntityListEntry(long entityInfoId);

    /// <summary>
    /// Позволяет добавить запись в кэш с ключом в виде идентификатора данных сущности и значением <see cref="EntityList"/>.
    /// </summary>
    /// <param name="entityInfoId">Идентификатор данных сущности, являющийся ключом записи.</param>
    /// <param name="value">Объект для записи в кэш.</param>
    /// <returns>Добавленную запись <see cref="EntityList"/>.</returns>
    EntityList SetEntityList(long entityInfoId, EntityList value);

    /// <summary>
    /// Позволяет получить <see cref="EntityList"/> из кэша по идентификатору данных сущности.
    /// </summary>
    /// <param name="entityInfoId">Идентификатор данных сущности, являющийся ключом записи.</param>
    /// <param name="value">Объект, возвращаемый из кэша.</param>
    /// <returns><see langword="true"/> - если ключ существует; иначе, <see langword="false"/>.</returns>
    bool TryGetEntityList(long entityInfoId, out EntityList value);

    /// <summary>
    /// Позволяет получить первый атрибут сущности из кэша, соответствующий заданным параметрам.
    /// </summary>
    /// <param name="entityInfoId">Идентификатор данных сущности, являющийся ключом записи.</param>
    /// <param name="entityId">Идентификатор сущности.</param>
    /// <param name="result">Атрибут, возвращаемый из кэша.</param>
    /// <returns><see langword="true"/> - если существует запись с заданными идентификаторами и не пустым списком атрибутов; иначе, <see langword="false"/>.</returns>
    bool TryGetCachedAttribute(long entityInfoId, long? entityId, out Attribute result);

    /// <summary>
    /// Позволяет получить первый атрибут сущности из кэша, соответствующий заданным параметрам.
    /// </summary>
    /// <param name="entityInfoId">Идентификатор данных сущности, являющийся ключом записи.</param>
    /// <param name="attributeValue">Значение атрибута для поиска.</param>
    /// <param name="result">Атрибут, возвращаемый из кэша.</param>
    /// <param name="entityId">Идентификатор сущности, возвращаемый из кэша.</param>
    /// <returns><see langword="true"/> - если существует запись с заданным идентификатором и сущность с требуемым значением атрибута; иначе, <see langword="false"/>.</returns>
    bool TryGetCachedAttribute(long entityInfoId, string attributeValue, out Attribute result, out long? entityId);

    /// <summary>
    /// Позволяет осуществить HTTP-запрос и последующее кэширование объектов <see cref="EntityList"/>.
    /// </summary>
    void RequestAndCacheEntityLists();

    /// <summary>
    /// Позволяет удалить запись из кэша.
    /// </summary>
    /// <param name="entityInfoId">Идентификатор данных сущности, являющийся ключом записи.</param>
    void RemoveEntityList(long entityInfoId);

    void Dispose();
}

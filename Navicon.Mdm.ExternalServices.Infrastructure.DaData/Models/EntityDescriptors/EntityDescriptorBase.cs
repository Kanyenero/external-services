using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Caching;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.Data;
using Navicon.Mdm.ExternalServices.Model.Entities;

namespace Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.EntityDescriptors;

internal abstract class EntityDescriptorBase
{
    protected EntityData EntityData { get; }

    protected IEntityListCacheManager CacheManager { get; }

    public EntityDescriptorBase(EntityData entityData, IEntityListCacheManager cacheManager)
    {
        EntityData = entityData ?? throw new ArgumentNullException(nameof(entityData));
        CacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    }

    public abstract void Collect(Entity entity);
}

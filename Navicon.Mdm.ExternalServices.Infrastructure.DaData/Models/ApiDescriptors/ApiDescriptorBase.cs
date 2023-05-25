using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.EntityDescriptors;

namespace Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.ApiDescriptors;

internal abstract class ApiDescriptorBase
{
    public abstract bool HasAnyDescriptor { get; }

    public abstract void Collect(EntityDescriptorBase source);
}

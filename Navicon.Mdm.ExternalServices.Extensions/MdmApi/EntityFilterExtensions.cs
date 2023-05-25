using Navicon.Mdm.ExternalServices.Dto.MdmApi;
using Navicon.Mdm.ExternalServices.Model.MdmApi;

namespace Navicon.Mdm.ExternalServices.Extensions.MdmApi;

public static class EntityFilterExtensions
{
    public static EntityFilterDto ToEntityFilterDto(this EntityFilter source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        return new()
        {
            PagingInfo = new PagingInfoDto
            {
                Skip = source.PagingInfo is null ? 0 : source.PagingInfo.Skip,
                Take = source.PagingInfo is null ? 0 : source.PagingInfo.Take,
            }
        };
    }
}

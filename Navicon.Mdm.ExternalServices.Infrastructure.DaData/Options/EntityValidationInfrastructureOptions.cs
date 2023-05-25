using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Enumerations;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Models.Data;
using Navicon.Mdm.ExternalServices.Model.MdmApi;

namespace Navicon.Mdm.ExternalServices.Infrastructure.DaData.Options;

public class EntityValidationInfrastructureOptions
{
    public AuthorizationData AuthorizationData { get; set; }

    public ValidationData ValidationData { get; set; }

    public EntityData EntityData { get; set; }

    public IDictionary<string, ApiType> ValidationScenarios { get; set; }

    public CachingSection Caching { get; set; }

    public bool LogDailyStats { get; set; }

    public int PartySuggestionFuzzySearchThreshold { get; set; }

    public class CachingSection
    {
        public ICollection<EntityRequest> MdmApiEntityRequests { get; set; }

        public ICollection<EntityListItemLoadOption> EntityListItemLoadOptions { get; set; }

        public class EntityListItemLoadOption
        {
            public long EntityInfoId { get; set; }

            public int? RequiredAttributeInfoId { get; set; }
        }
    }
}

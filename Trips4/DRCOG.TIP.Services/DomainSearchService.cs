using DRCOG.Common.ServiceInterfaces.ActiveDirectorySupport;
using WoodGroupEsp.Common.ServiceInterfaces.ActiveDirectorySupport;
using DRCOG.Common.Services;
using DRCOG.Domain.ServiceInterfaces;

namespace DRCOG.TIP.Services
{
    /// <summary>
    /// Service responsible for querying active directory for informaiton about users
    /// </summary>
    public class DomainSearchService : GenericDomainSearchService, IDomainSearchService
    {
        public DomainSearchService(IDomainSearchConfig searchConfig, ISearchFilterFactory searchFactory)
            :
            base(searchConfig, searchFactory) { }
    }
}

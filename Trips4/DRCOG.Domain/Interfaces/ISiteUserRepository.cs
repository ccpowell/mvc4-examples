using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Domain.Models;
using DRCOG.Common.PersistenceSupport;

namespace DRCOG.Domain.Interfaces
{
    public interface ISiteUserRepository : ILinqEnabledRepository<SiteUser, Int64>
    {

    }
}

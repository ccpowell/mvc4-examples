using System;
using DRCOG.Domain.Interfaces;
using System.Web.Security;
using DRCOG.Common.Service.MemberShipServiceSupport.Interfaces;
using System.Web.Profile;
using DRCOG.Common.Services.MemberShipServiceSupport;

namespace DRCOG.Data
{
    /// <summary>
    /// Account Repository
    /// </summary>
    /// <remarks>Assumes unique UserName and unique Email.</remarks>
    public class AccountRepository : BaseRepository, IAccountRepository
    {

        
    }
}

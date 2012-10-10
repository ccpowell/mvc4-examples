using System;
using System.Security.Principal;

namespace DRCOG.Domain.Security
{
    public interface ISitePrincipal : IPrincipal
    {
        ISiteIdentity SiteIdentity { get; }
    }
}

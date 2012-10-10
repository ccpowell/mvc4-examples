using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace DRCOG.Domain.Security
{
    public class SiteUserPrincipal : ISitePrincipal
    {
        public SiteUserPrincipal(IList<String> roles, ISiteIdentity identity)
        {
            Roles = new List<String>(roles);
            SiteIdentity = identity;
        }

        #region IPrincipal Members

        public IIdentity Identity
        {
            get
            {
                return SiteIdentity;
            }
        }

        public ISiteIdentity SiteIdentity
        {
            get;
            private set;
        }

        private IList<String> Roles
        {
            get;
            set;
        }

        public bool IsInRole(string role)
        {
            if (Roles == null)
            {
                return false;
            }
            else
            {
                return Roles.Contains(role);
            }
        }

        #endregion
    }
}

using System;
using DRCOG.Common.Security;
using System.Collections.Generic;

namespace DRCOG.Domain.Security
{
    public class SiteUserIdentity<IdType> : GenericUserIdentity<IdType>, ISiteIdentity
    {
        #region IIdentity Members

        public SiteUserIdentity(Boolean isAuthenticated) : base(isAuthenticated) { }

        public SiteUserIdentity(Boolean isAuthenticated, IDictionary<ActiveDirectoryAttribute, String> adUser,
            IdType id)
            : base(isAuthenticated, adUser, id)
        {
            Email = adUser[ActiveDirectoryAttribute.Mail];
            PhoneNumber = adUser[ActiveDirectoryAttribute.TelephoneNumber];
        }

        public String PhoneNumber
        {
            get;
            private set;
        }

        public String Email
        {
            get;
            private set;
        }

        #endregion
    }
}

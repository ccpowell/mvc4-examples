using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Profile;
using System.Web.Security;

namespace DRCOG.Domain.Security
{
    public class MemberProfile : ProfileBase
    {
        static public MemberProfile CurrentUser
        {
            get
            {
                MembershipUser user = Membership.GetUser();
                return (MemberProfile)
                    Create(user.UserName, user.IsApproved);
            }
        }

        public virtual string HomeState
        {
            get
            {
                return ((string)(this.GetPropertyValue("HomeState")));
            }
            set
            {
                this.SetPropertyValue("HomeState", value);
            }
        }

        public virtual string HomeZipCode
        {
            get
            {
                return ((string)(this.GetPropertyValue("HomeZipCode")));
            }
            set
            {
                this.SetPropertyValue("HomeZipCode", value);
            }
        }

        public virtual string LastName
        {
            get
            {
                return ((string)(this.GetPropertyValue("LastName")));
            }
            set
            {
                this.SetPropertyValue("LastName", value);
            }
        }

        public virtual string HomeEmailAddress
        {
            get
            {
                return ((string)(this.GetPropertyValue("HomeEmailAddress")));
            }
            set
            {
                this.SetPropertyValue("HomeEmailAddress", value);
            }
        }

        public virtual string HomeCity
        {
            get
            {
                return ((string)(this.GetPropertyValue("HomeCity")));
            }
            set
            {
                this.SetPropertyValue("HomeCity", value);
            }
        }

        public virtual string HomeAddress
        {
            get
            {
                return ((string)(this.GetPropertyValue("HomeAddress")));
            }
            set
            {
                this.SetPropertyValue("HomeAddress", value);
            }
        }

        public virtual string FirstName
        {
            get
            {
                return ((string)(this.GetPropertyValue("FirstName")));
            }
            set
            {
                this.SetPropertyValue("FirstName", value);
            }
        }

        public virtual string HomeUnit
        {
            get
            {
                return ((string)(this.GetPropertyValue("HomeUnit")));
            }
            set
            {
                this.SetPropertyValue("HomeUnit", value);
            }
        }

        public virtual string HomePhoneNumber
        {
            get
            {
                return ((string)(this.GetPropertyValue("HomePhoneNumber")));
            }
            set
            {
                this.SetPropertyValue("HomePhoneNumber", value);
            }
        }
    }
}

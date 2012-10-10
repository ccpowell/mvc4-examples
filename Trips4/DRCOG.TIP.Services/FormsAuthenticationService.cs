using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using DRCOG.Common.Security;
using System.Security.Principal;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.Domain;
using DRCOG.Domain.Security;
using DRCOG.Domain.Models;
using DRCOG.Common.DesignByContract;
//using DRCOG.Domain.Caching;
//using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using DRCOG.Domain.Interfaces;
using System.Web.UI;
using System.Web.Security;
using DRCOG.Common.Services.MemberShipServiceSupport;

namespace DRCOG.TIP.Services
{
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");

            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        public void SetAuthCookie(string userName, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }
    }
}

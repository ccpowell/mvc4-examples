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
using DRCOG.Common.Service.MemberShipServiceSupport.Interfaces;

namespace DRCOG.TIP.Services
{
    public class UserService : IUserService
    {
        public UserService()
        {
            //UserCacheManager = CacheManager.Instance.UserCache;
        }
        public UserService(IUserRepositoryExtension userRepository)
        {
            UserRepository = userRepository;
        }

        //private Microsoft.Practices.EnterpriseLibrary.Caching.ICacheManager UserCacheManager
        //{
        //    get;
        //    set;
        //}

        public IUserRepositoryExtension UserRepository
        {
            get;
            set;
        }

        //public IDomainSearchService DomainSearchService
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// The time in second indicating how long to cache the user.
        /// </summary>
        //public Int32 UserCacheTime
        //{
        //    get;
        //    set;
        //}

        //private HttpContextWrapper _context;
        //private HttpContextWrapper Context
        //{
        //    get
        //    {
        //        if (_context == null)
        //        {
        //            return new HttpContextWrapper(HttpContext.Current);
        //        }
        //        return _context;
        //    }
        //    set { _context = value; }
        //}

        //private IEnumerable<ActiveDirectoryAttribute> _propertiesToLoad;
        //private IEnumerable<ActiveDirectoryAttribute> PropertiesToLoad
        //{
        //    get
        //    {
        //        if (_propertiesToLoad == null)
        //        {
        //            _propertiesToLoad = new List<ActiveDirectoryAttribute>(4)
        //            {
        //                ActiveDirectoryAttribute.DisplayName,
        //                ActiveDirectoryAttribute.Mail,
        //                ActiveDirectoryAttribute.TelephoneNumber,
        //                ActiveDirectoryAttribute.UserPrincipalName
        //            };
        //        }
        //        return _propertiesToLoad;
        //    }
        //}

        #region IUserService Members

        public void LoadPerson(ref Person person)
        {

            UserRepository.LoadPerson(ref person);
        }

        public bool CheckPersonHasProjects(Person person, int timePeriodId)
        {
            return UserRepository.CheckPersonHasProjects(person, timePeriodId);
        }

        public void ReplaceSponsor(Guid newSponsor, int currentSponsorId)
        {
            UserRepository.ReplaceSponsor(newSponsor, currentSponsorId);
        }


        public void CreatePerson(Profile profile)
        {
            UserRepository.CreatePerson(ref profile);

            if (!String.IsNullOrEmpty(profile.SponsorCode))
            {
                UserRepository.LinkUserWithSponsor(profile);
            }
        }

        public bool GetUserApproval(string userName)
        {
            return UserRepository.GetUserApproval(userName);
           
        }

        //public ISitePrincipal GetCurrentUser()
        //{
        //    ISitePrincipal siteUser = null;
        //    IPrincipal currentUser = Context.User;
        //    if (currentUser != null && currentUser.Identity != null && currentUser.Identity.IsAuthenticated)
        //    {
        //        if (UserCacheManager.GetData(currentUser.Identity.Name) != null)
        //        {
        //            siteUser = (ISitePrincipal)UserCacheManager.GetData(currentUser.Identity.Name);
        //        }
        //        else
        //        {
        //            IDictionary<ActiveDirectoryAttribute, String> userValues =
        //                DomainSearchService.GetUser(currentUser.Identity.Name, PropertiesToLoad);
        //            if (userValues.Count > 0 && !String.IsNullOrEmpty(userValues[ActiveDirectoryAttribute.UserPrincipalName]))
        //            {
        //                SiteUser user = UserRepository.GetQueryable().Where(u => u.ActiveDirectoryName ==
        //                    userValues[ActiveDirectoryAttribute.UserPrincipalName]).FirstOrDefault();
        //                Int64? id = null;
        //                SiteUserIdentity<Int64?> siteIdentity;
        //                if (user != null)
        //                {
        //                    id = user.EntityId;
        //                    siteIdentity = new SiteUserIdentity<Int64?>(
        //                        true,
        //                        userValues,
        //                        id);
        //                }
        //                else
        //                {
        //                    siteIdentity = new SiteUserIdentity<Int64?>(
        //                        false,
        //                        userValues,
        //                        id);
        //                }

        //                IList<String> roles = new List<String>();

        //                if (siteIdentity.IsAuthenticated)
        //                {
        //                    foreach (String role in Enum.GetNames(typeof(Enums.Roles)))
        //                    {
        //                        if (Context.User.IsInRole(WebConfigurationManager.AppSettings.Get(role)))
        //                        {
        //                            roles.Add(role);
        //                        }
        //                    }
        //                }
        //                siteUser = new SiteUserPrincipal(roles, siteIdentity);

        //                //don't cache a user without an account.
        //                if (siteIdentity.IsAuthenticated)
        //                {
        //                    UserCacheManager.Add(currentUser.Identity.Name, siteUser,
        //                        Microsoft.Practices.EnterpriseLibrary.Caching.CacheItemPriority.Normal,
        //                        null, new SlidingTime(TimeSpan.FromSeconds(UserCacheTime)));
        //                }
        //            }
        //        }
        //    }

        //    return siteUser;
        //}

        //public SiteUser GetCurrentSiteUser()
        //{
        //    SiteUser user = null;
        //    ISitePrincipal principal = GetCurrentUser();
        //    //if (principal != null && principal.Identity.IsAuthenticated)
        //    //{
        //    //    user = UserRepository.Load(((SiteUserIdentity<long?>)principal.SiteIdentity).Id.Value);
        //    //}
        //    return user;
        //}

        //public bool CheckCurrentUserRole(Enums.Roles role)
        //{
        //    IPrincipal principal = GetCurrentUser();
        //    if (principal == null)
        //    {
        //        return false;
        //    }
        //    return principal.IsInRole(role.ToString());
        //}

        //public bool CheckCurrentUserRoleAny(IEnumerable<Enums.Roles> list)
        //{
        //    Check.Require(list != null, "list cannot be null");
        //    IPrincipal principal = GetCurrentUser();
        //    if (principal != null)
        //    {
        //        foreach (Enums.Roles role in list)
        //        {
        //            if (principal.IsInRole(role.ToString()))
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}

        //public bool CheckCurrentUserRoleAll(IEnumerable<Enums.Roles> list)
        //{
        //    Check.Require(list != null, "list cannot be null");
        //    IPrincipal principal = GetCurrentUser();
        //    if (principal != null)
        //    {
        //        Boolean hasAllRoles = true;
        //        foreach (Enums.Roles role in list)
        //        {
        //            if (!principal.IsInRole(role.ToString()))
        //            {
        //                hasAllRoles = false;
        //                break;
        //            }
        //        }
        //        return hasAllRoles;
        //    }
        //    return false;
        //}

        #endregion
    }
}

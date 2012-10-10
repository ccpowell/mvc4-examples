using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Common.Services.MemberShipServiceSupport;
using DRCOG.Domain.Interfaces;
using System.Web.Security;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Security.Principal;
using DRCOG.Common.Service.MemberShipServiceSupport.Interfaces;

namespace DRCOG.Domain.Models
{
    public partial class Person : IPerson
    {
        public Profile profile;

        public IMembershipService MembershipService { get; set; }

        public int LastProjectVersionId { get; set; }
        public int LastSponsorContactId { get; set; }
        public int SponsorOrganizationId { get; set; }
        public string SponsorOrganizationName { get; set; }

        public bool HasProjects { get; set; }

        public List<int> SponsoredProjectVersionIds { get; set; }

        public bool SponsorsProject()
        {
            return SponsoredProjectVersionIds.Contains(LastProjectVersionId);
        }

        public bool SponsorsProject(int projectVersionId)
        {
            if (SponsoredProjectVersionIds == null) return false;
            return SponsoredProjectVersionIds.Contains(projectVersionId);
        }

        public Person()
        {
            Initialize();
        }

        public Person(string userName)
        {
            Initialize();
            profile.UserName = userName;

        }

        private void Initialize()
        {
            profile = new Profile();
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }
            if (SponsoredProjectVersionIds == null) { SponsoredProjectVersionIds = new List<int>(); }
        }

        public virtual bool IsInRole(string role)
        {
            if (this.HasProjects)
            {
                return true;
            }

            if ((profile.Roles != null) && (profile.Roles.Count > 0))
            {
                var collection = profile.Roles.Where(x => x.Key == "TripsRoleProvider")
                    .Select(x => x.Value)
                    .Select(x => x.Where(y => (y.Value == true && y.Key == role) || (y.Key == "Administrator" && y.Value == true)))
                    .FirstOrDefault();

                var value = collection.Count() > 0 ? true : false;
                return value;
            }
            return false;
        }

        public virtual void AddRole(string role)
        {
            var count = profile.Roles.Where(x => x.Key == "TripsRoleProvider")
                .Select(x => x.Value)
                .Select(x => x.Where(y => (y.Key == role))).Count();

            var dictionary = profile.Roles.Where(x => x.Key == "TripsRoleProvider")
                    .Select(x => x.Value).FirstOrDefault();

            if (count > 0)
            {
                dictionary[role] = true;
            }
            else
            {
                dictionary.Add(role, true);
            }
        }

        public void Load()
        {
            Profile person = new Profile() { BusinessEmail = profile.BusinessEmail, UserName = profile.UserName };
            //ProfileService.Load(ref person, Membership.Providers["AspNetSqlMembershipProvider"]);
            //person = ProfileService.GetUserProfile(profile.UserName, Common.Services.MemberShipServiceSupport.Enums.MembershipProvider.DRCOG);
            person.PersonGUID = MembershipService.PersonGuid;
            
            profile = person;
        }

        public bool ValidateUser(string userName, string password)
        {
            //return AuthenticationService.ValidateUser(userName, password);
            return MembershipService.ValidateUser(userName, password);
            
        }
    }
}

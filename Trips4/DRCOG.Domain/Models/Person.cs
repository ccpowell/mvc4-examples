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

    public class ShortProfile
    {
        public ShortProfile()
        {
            Roles = new List<string>();
        }

        public int PersonID { get; set; }
        public Guid PersonGUID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RecoveryEmail { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SponsorCode { get; set; }
        public string Organization { get; set; }
        public string Title { get; set; }
        public List<string> Roles { get; set; }

        // TODO: get rid of this
        public bool Success { get; set; }
    }

    public partial class Person
    {
        public ShortProfile profile;
        public bool IsApproved { get; set; }
        public int LastProjectVersionId { get; set; }
        public int LastSponsorContactId { get; set; }
        public int SponsorOrganizationId { get; set; }
        public string SponsorOrganizationName { get; set; }

        public bool HasProjects { get; set; }

        public List<int> SponsoredProjectVersionIds { get; private set; }

        public bool SponsorsProject()
        {
            return SponsoredProjectVersionIds.Contains(LastProjectVersionId);
        }

        public bool SponsorsProject(int projectVersionId)
        {
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
            profile = new ShortProfile();
            SponsoredProjectVersionIds = new List<int>(); 
        }

        public virtual bool IsInRole(string role)
        {
            // TODO: WTF?
            if (this.HasProjects)
            {
                return true;
            }

            return Roles.IsUserInRole(profile.UserName, role);
        }

    }
}

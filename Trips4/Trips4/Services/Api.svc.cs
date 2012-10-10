using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.Domain.Models;
using DRCOG.Domain.Helpers;

namespace DRCOG.Web.Services
{
    public class Api : IApi
    {
        public ContactRoles GetSponsorContactRolesByUser(string userShortGuid)
        {
            return ApiFederationHelper.FederationObject.GetSponsorContactRolesByUser(userShortGuid);
        }

        public bool SetSponsorContactRoleByUser(string userShortGuid, string organization, string role)
        {
            return ApiFederationHelper.FederationObject.SetSponsorContactRoleByUser(userShortGuid, organization, role);
        }

        public bool DeleteSponsorContactRoleByUser(string userShortGuid, string organization, string role)
        {
            return ApiFederationHelper.FederationObject.DeleteSponsorContactRoleByUser(userShortGuid, organization, role);
        }
    }
}

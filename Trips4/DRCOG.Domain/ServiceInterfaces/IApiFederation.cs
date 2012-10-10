using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Domain.Models;

namespace DRCOG.Domain.ServiceInterfaces
{
    public interface IApiFederation
    {
        ContactRoles GetSponsorContactRolesByUser(string userShortGuid);
        bool SetSponsorContactRoleByUser(string userShortGuid, string organization, string role);
        bool DeleteSponsorContactRoleByUser(string userShortGuid, string organization, string role);
    }
}

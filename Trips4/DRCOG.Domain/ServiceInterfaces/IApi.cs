using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DRCOG.Domain.Models;

namespace DRCOG.Domain.ServiceInterfaces
{
    [ServiceContract]
    public interface IApi
    {
        [OperationContract]
        ContactRoles GetSponsorContactRolesByUser(string userShortGuid);

        [OperationContract]
        bool SetSponsorContactRoleByUser(string userShortGuid, string organization, string role);

        [OperationContract]
        bool DeleteSponsorContactRoleByUser(string userShortGuid, string organization, string role);
    }
}

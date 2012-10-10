using System;
using DRCOG.Domain;
using DRCOG.Domain.Security;
using DRCOG.Domain.Models;
using System.Collections.Generic;
using DRCOG.Common.Services.MemberShipServiceSupport;

namespace DRCOG.Domain.ServiceInterfaces
{
    /// <summary>
    /// Service responsible for saving, updating, and retrieving user information.
    /// </summary>
    public interface IUserService
    {
        void ReplaceSponsor(Guid newSponsor, int currentSponsorId);
        void CreatePerson(Profile profile);
        void LoadPerson(ref Person person);
        bool GetUserApproval(string userName);
        bool CheckPersonHasProjects(Person person, int timePeriodId);

        //ISitePrincipal GetCurrentUser();

        //SiteUser GetCurrentSiteUser();

        /// <summary>
        /// Checks the current user's roles for the specified role.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        //Boolean CheckCurrentUserRole(Enums.Roles role);

        /// <summary>
        /// Checks the current user's roles for any role in the list.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        //Boolean CheckCurrentUserRoleAny(IEnumerable<Enums.Roles> list);

        /// <summary>
        /// Checks that the current user's roles contain all roles in the list.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        //Boolean CheckCurrentUserRoleAll(IEnumerable<Enums.Roles> list);
    }
}

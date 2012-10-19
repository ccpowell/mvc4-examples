using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
//using DRCOG.Common.Services.MemberShipServiceSupport;
using DRCOG.Domain.Models;
//using DRCOG.Common.Service.MemberShipServiceSupport.Interfaces;

namespace DRCOG.Domain.Interfaces
{

    public interface ITripsUserRepository 
    {
        //void LoadPerson(Person person); //???
        Person GetUserByName(string userName, bool loadRoles);
        Person GetUserByID(Guid userId, bool loadRoles);
        Person GetUserByEmail(string emailAddress, bool loadRoles);

        //void LinkUserWithSponsor(Profile profile);
        //void ReplaceSponsor(Guid newSponsor, int currentSponsorId);
        bool GetUserApproval(string userName);
        bool CheckPersonHasProjects(Person person, int timePeriodId);

        void UpdateUserApproval(Guid userId, bool isApproved);
        void ChangePassword(Guid userId, string oldPaassword, string newPassword);
        string ResetPassword(Guid userId);
        bool ValidateUser(string userName, string password);
    }
}

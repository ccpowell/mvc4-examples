using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Common.Services.MemberShipServiceSupport;
using DRCOG.Domain.Models;
using DRCOG.Common.Service.MemberShipServiceSupport.Interfaces;

namespace DRCOG.Domain.Interfaces
{
    public interface IUserRepositoryExtension : IUserRepository
    {
        void LoadPerson(ref Person person);
        void LinkUserWithSponsor(Profile profile);
        void ReplaceSponsor(Guid newSponsor, int currentSponsorId);
        void CreatePerson(ref Profile profile);
        bool GetUserApproval(string userName);
        bool CheckPersonHasProjects(Person person, int timePeriodId);
    }
}

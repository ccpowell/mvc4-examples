using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Models;
using DRCOG.Domain;
using DRCOG.Domain.Models.RTP;

namespace DRCOG.TIP.Services.AmendmentStrategy.Survey
{
    public class PendingToAdopted : IAmendmentStrategy
    {
        protected readonly IRtpProjectRepository RtpProjectRepository;
        protected readonly CycleAmendment Amendment;
        private int pendingDeleteId { get; set; }

        public PendingToAdopted(IRtpProjectRepository repo, CycleAmendment amendment)
        {
            RtpProjectRepository = repo;
            Amendment = this.UpdateStatus(amendment);
        }

        protected CycleAmendment UpdateStatus(CycleAmendment amendment)
        {
            // need to check the database to see if the project version is new or from existing
            if (GetProjectHistory().Count() > 1 && GetProjectHistory().ElementAt(1).Value == (int)Enums.RTPAmendmentStatus.Submitted)
            {
                amendment.AmendmentStatusId = (int)Enums.RTPAmendmentStatus.Approved;
                pendingDeleteId = GetProjectHistory().ElementAt(1).Key;
            }
            else
            {
                amendment.AmendmentStatusId = (int)Enums.RTPAmendmentStatus.Amended;
            }

            amendment.VersionStatusId = (Int32)Enums.RTPVersionStatus.Active;
            return amendment;
        }


        private SortedList<int, int> GetProjectHistory()
        {
            SortedList<int, int> history = new SortedList<int, int>();

            return history;
        }

        //protected void Copy()
        //{
        //    Amendment.PreviousVersionId = Amendment.ProjectVersionId;
        //    Amendment.ProjectVersionId = RtpProjectRepository.CopyProject(Amendment.PreviousVersionId);
        //}

        protected void Delete(int projectVersionId)
        {
            RtpProjectRepository.DeleteProjectVersion(projectVersionId/*, Enums.RTPAmendmentStatus.Submitted*/);
            //TODO: need to throw some sort of alert later to admin
        }

        #region IAmendmentStrategy Members

        public Int32 Amend()
        {
            try
            {
                RtpProjectRepository.UpdateProjectAmendmentStatus(Amendment);
                RtpProjectRepository.UpdateProjectVersionStatusId(Amendment.PreviousVersionId, (Int32)Enums.RTPVersionStatus.Active);
                if (Amendment.AmendmentStatusId.Equals((int)Enums.RTPAmendmentStatus.Approved)
                    && !pendingDeleteId.Equals(default(int)))
                {
                    Delete(pendingDeleteId);
                }
                return (Int32)Amendment.ProjectVersionId;
                //return default(int);
            }
            catch
            {
                // try and return the new project version if we got that far.
                if (!Amendment.ProjectVersionId.Equals(Amendment.PreviousVersionId))
                    return (Int32)Amendment.ProjectVersionId;
                return default(Int32);
            }
        }

        #endregion
    }
}

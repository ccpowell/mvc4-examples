using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Models;
using DRCOG.Domain;
using DRCOG.Domain.Models.RTP;

namespace DRCOG.TIP.Services.AmendmentStrategy.RTP
{
    public class SubmittedToPending : IAmendmentStrategy
    {
        protected readonly IRtpProjectRepository RtpProjectRepository;
        protected readonly CycleAmendment Amendment;

        public SubmittedToPending(IRtpProjectRepository repo, CycleAmendment amendment)
        {
            RtpProjectRepository = repo;
            Amendment = this.UpdateStatus(amendment);
        }

        protected CycleAmendment UpdateStatus(CycleAmendment amendment)
        {
            amendment.AmendmentStatusId = (int)Enums.RTPAmendmentStatus.Pending;
            amendment.VersionStatusId = (Int32)Enums.RTPVersionStatus.Pending;
            return amendment;
        }

        protected void Copy()
        {
            Amendment.PreviousVersionId = Amendment.ProjectVersionId;
            Amendment.ProjectVersionId = (int)RtpProjectRepository.CopyProject(Amendment.Id, Amendment.PreviousVersionId).ProjectVersionId;
        }

        #region IAmendmentStrategy Members

        public Int32 Amend()
        {
            try
            {
                Copy();
                RtpProjectRepository.UpdateProjectAmendmentStatus(Amendment);
                return (Int32)Amendment.ProjectVersionId;
            }
            catch
            {
            }

            // try and return the new project version if we got that far.
            if (!Amendment.ProjectVersionId.Equals(Amendment.PreviousVersionId))
                return (Int32)Amendment.ProjectVersionId;
            return default(Int32);
        }

        #endregion
    }
}

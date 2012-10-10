using System;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.Domain;
using DRCOG.Domain.Models;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Models.RTP;

namespace DRCOG.TIP.Services.AmendmentStrategy.RTP
{
    public class DeleteToSubmitted : IAmendmentStrategy
    {
        protected readonly IRtpProjectRepository RtpProjectRepository;
        protected readonly CycleAmendment Amendment;

        public DeleteToSubmitted(IRtpProjectRepository repo, CycleAmendment amendment)
        {
            RtpProjectRepository = repo;
            Amendment = UpdateStatus(amendment);
        }

        protected CycleAmendment UpdateStatus(CycleAmendment amendment)
        {
            //set the Amendment Reason to "Deleting previous amendment"
            amendment.AmendmentStatusId = (Int32)Enums.RTPAmendmentStatus.Submitted;
            amendment.VersionStatusId = (Int32)Enums.RTPVersionStatus.Pending;
            return amendment;
        }

        protected void Copy()
        {
            Amendment.PreviousVersionId = (Int32)Amendment.ProjectVersionId; // if we went the previous of the deleted then change this.
            Amendment.ProjectVersionId = RtpProjectRepository.CopyProject(Amendment.PreviousVersionId).ProjectVersionId;
        }

        #region IAmendmentStrategy Members

        public int Amend()
        {
            Copy();
            RtpProjectRepository.UpdateProjectAmendmentStatus(Amendment);
            return (Int32)Amendment.ProjectVersionId;
        }

        #endregion
    }
}

using System;
using DRCOG.Data;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.Domain.Models;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Models.RTP;
using DRCOG.Domain;

namespace DRCOG.TIP.Services.DeleteStrategy.RTP
{
    class PendingDrop : IDeleteStrategy
    {
        private readonly IRtpProjectRepository _repo;
        private readonly CycleAmendment _amendment;

        public PendingDrop(CycleAmendment amendment, IRtpProjectRepository repo)
        {
            _repo = repo;
            _amendment = this.UpdateStatus(amendment);
        }

        protected CycleAmendment UpdateStatus(CycleAmendment amendment)
        {
            if(amendment.VersionStatusId.Equals((Int32)Enums.RTPVersionStatus.Inactive))
            {
                amendment.VersionStatusId = (Int32)Enums.RTPVersionStatus.Pending;
                amendment.AmendmentStatusId = (Int32)Enums.RTPAmendmentStatus.Pending;
            }
            else amendment.VersionStatusId = (Int32)Enums.RTPVersionStatus.Inactive;
            
            return amendment;
        }

        #region IDeleteStrategy Members


        public int Delete()
        {
            // not really deleting this thing. We are simply changing its status to cancelled
            _repo.UpdateProjectAmendmentStatus(_amendment);
            return 0;
        }

        #endregion
    }
}

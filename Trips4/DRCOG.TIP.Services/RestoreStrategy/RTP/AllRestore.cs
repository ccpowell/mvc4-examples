using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Models;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.Domain;
using DRCOG.Domain.Models.RTP;

namespace DRCOG.TIP.Services.RestoreStrategy.RTP
{
    class AllRestore : IRestoreStrategy
    {
        private readonly IRtpProjectRepository _repo;
        private readonly Int32 _projectVersionID;
        private readonly CycleAmendment Amendment;

        public AllRestore(Int32 projectVersionID, IRtpProjectRepository repo)
        {
            _repo = repo;
            _projectVersionID = projectVersionID;
            Amendment = new CycleAmendment();
            Amendment = this.UpdateStatus(ref this.Amendment);
        }

        protected CycleAmendment UpdateStatus(ref CycleAmendment amendment)
        {
            amendment.AmendmentStatusId = (int)Enums.RTPAmendmentStatus.Pending;
            amendment.VersionStatusId = (Int32)Enums.RTPVersionStatus.Pending;
            return amendment;
        }


        #region IRestoreStrategy Members

        public object Restore(string plan)
        {
            RtpSummary result = _repo.RestoreProjectVersion(_projectVersionID, plan);
            this.Amendment.ProjectVersionId = (int)result.ProjectVersionId;
            _repo.UpdateProjectAmendmentStatus(this.Amendment);
            result.AmendmentStatus = Enums.RTPAmendmentStatus.Pending.ToString();
            return result;
        }

        #endregion
    }
}

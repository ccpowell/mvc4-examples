using System;
using DRCOG.Data;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Models.RTP;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.TIP.Services.AmendmentStrategy.RTP;
using DRCOG.Domain.Models;

namespace DRCOG.TIP.Services.DeleteStrategy.RTP
{
    public class ApprovedDelete : IDeleteStrategy
    {
        protected readonly IRtpProjectRepository ProjectRepository;
        private readonly CycleAmendment Amendment;

        public ApprovedDelete(CycleAmendment amendment, IRtpProjectRepository repo)
        {
            Amendment = amendment;
            ProjectRepository = repo;
        }

        #region IDeleteStrategy Members

        public int Delete()
        {
            /* TODO:
                 * Yes you can delete but:
                 * Show Amend window and set the Amendment Reason to "Deleting previous amendment"
                 */
            int projectVersionId = 0;
            projectVersionId = new DeleteToSubmitted(ProjectRepository, Amendment).Amend();

            return projectVersionId;
        }

        #endregion
    }
}

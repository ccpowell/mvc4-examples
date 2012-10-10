using System;
using DRCOG.Data;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.Domain.Models;

namespace DRCOG.TIP.Services.DeleteStrategy.TIP
{
    public class ApprovedDelete : IDeleteStrategy
    {
        protected readonly IProjectRepository ProjectRepository;
        private readonly ProjectAmendments Amendment;

        public ApprovedDelete(ProjectAmendments amendment, IProjectRepository repo)
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

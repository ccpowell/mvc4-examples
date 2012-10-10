using DRCOG.Data;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.Domain.Models;

namespace DRCOG.TIP.Services.DeleteStrategy.TIP
{
    public class DeleteStrategy
    {
        private readonly IProjectRepository _repo;
        private readonly ProjectAmendments _amendment;

        public DeleteStrategy(IProjectRepository repo, ProjectAmendments amendment)
        {
            _repo = repo;
            _amendment = amendment;
        }

        public int GetAmendmentStatusId(int projectVersionId)
        {
            return _repo.GetProjectAmendmentStatus(projectVersionId);
        }

        public IDeleteStrategy PickStrategy()
        {
            IDeleteStrategy strategy = null;
            switch (GetAmendmentStatusId((int)_amendment.ProjectVersionId))
            {
                case 10734: // Amended
                case 1774:
                case 10736: // Approved
                case 1769:
                    strategy = new ApprovedDelete(_amendment, _repo);
                    break;
                case 10740: // Proposed
                case 1767:
                case 10742: // Submitted
                case 1770:
                    strategy = new InProgressDelete(_amendment, _repo);
                    break;
            }
            return strategy;
        }
    }
}

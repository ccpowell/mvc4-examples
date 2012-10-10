using DRCOG.Data;
using DRCOG.Domain;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.Domain.Models;
using DRCOG.Domain.Models.RTP;

namespace DRCOG.TIP.Services.DeleteStrategy.RTP
{
    public class DeleteStrategy
    {
        private readonly IRtpProjectRepository _repo;
        private readonly CycleAmendment _amendment;

        public DeleteStrategy(IRtpProjectRepository repo, CycleAmendment amendment)
        {
            _repo = repo;
            _amendment = amendment;
        }

        public int GetAmendmentStatusId(int projectVersionId)
        {
            return _amendment.AmendmentStatusId.Equals(default(int)) 
                ? _repo.GetProjectAmendmentStatus(projectVersionId) 
                : _amendment.AmendmentStatusId;
        }

        public IDeleteStrategy PickStrategy()
        {
            IDeleteStrategy strategy = null;
            switch (GetAmendmentStatusId(_amendment.ProjectVersionId))
            {
                //case 10734: // Amended
                //case 1774:
                //case 10736: // Approved
                //case 1769:
                //    strategy = new ApprovedDelete(_amendment, _repo);
                //    break;
                case (int)Enums.RTPAmendmentStatus.Submitted: // Submitted
                case (int)Enums.RTPAmendmentStatus.Pending: // Pending
                    strategy = new InProgressDelete(_amendment, _repo);
                    break;
                case (int)Enums.RTPAmendmentStatus.Cancelled: // Pending
                    strategy = new PendingDrop(_amendment, _repo);
                    break;
            }
            return strategy;
        }
    }
}

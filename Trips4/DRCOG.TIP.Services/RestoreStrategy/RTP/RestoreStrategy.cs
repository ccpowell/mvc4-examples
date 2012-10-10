using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Models;
using DRCOG.Domain.ServiceInterfaces;

namespace DRCOG.TIP.Services.RestoreStrategy.RTP
{
    public class RestoreStrategy
    {
        private readonly IRtpProjectRepository _repo;
        private readonly Int32 _projectVersionID;

        public RestoreStrategy(IRtpProjectRepository repo, Int32 projectVersionID)
        {
            _repo = repo;
            _projectVersionID = projectVersionID;
        }

        public int GetAmendmentStatusId(int projectVersionID)
        {
            return _repo.GetProjectAmendmentStatus(projectVersionID);
        }

        public IRestoreStrategy PickStrategy()
        {
            IRestoreStrategy strategy = null;
            switch (GetAmendmentStatusId(_projectVersionID))
            {
                default:
                    strategy = new AllRestore(_projectVersionID, _repo);
                    break;
            }
            return strategy;
        }
    }
}

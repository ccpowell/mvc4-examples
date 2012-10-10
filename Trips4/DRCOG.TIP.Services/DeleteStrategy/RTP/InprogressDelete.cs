using System;
using DRCOG.Data;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.Domain.Models;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Models.RTP;

namespace DRCOG.TIP.Services.DeleteStrategy.RTP
{
    class InProgressDelete : IDeleteStrategy
    {
        private readonly IRtpProjectRepository _repo;
        private readonly CycleAmendment _amendment;

        public InProgressDelete(CycleAmendment amendment, IRtpProjectRepository repo)
        {
            _repo = repo;
            _amendment = amendment;
        }

        

        #region IDeleteStrategy Members


        public int Delete()
        {
            //ToDo: need to delete the returned filename
            _repo.DeleteProjectVersion(_amendment.ProjectVersionId);
            return 0;
        }

        #endregion
    }
}

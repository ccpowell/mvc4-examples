using System;
using DRCOG.Data;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.Domain.Models;
using DRCOG.Domain.Interfaces;

namespace DRCOG.TIP.Services.DeleteStrategy.TIP
{
    class InProgressDelete : IDeleteStrategy
    {
        private readonly IProjectRepository _repo;
        private readonly ProjectAmendments _amendment;

        public InProgressDelete(ProjectAmendments amendment, IProjectRepository repo)
        {
            _repo = repo;
            _amendment = amendment;
        }

        

        #region IDeleteStrategy Members


        public int Delete()
        {
            //ToDo: need to delete the returned filename
            _repo.DeleteProjectVersion(_amendment);
            return 0;
        }

        #endregion
    }
}

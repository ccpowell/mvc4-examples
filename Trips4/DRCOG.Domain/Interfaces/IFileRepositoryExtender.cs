using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Common.Services;
using DRCOG.Common.Services.Interfaces;

namespace DRCOG.Domain.Interfaces
{
    public interface IFileRepositoryExtender : IFileRepository
    {
        Image Load(int id);
        //void SaveInProjectVersion(Guid mediaId, int projectVersionId);
        Guid Save(File file, int projectVersionId);
        void Delete(int id, int projectVersionId);
    }
}

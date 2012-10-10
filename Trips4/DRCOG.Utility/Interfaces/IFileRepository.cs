using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Utility.Domain;

namespace DRCOG.Utility.Interfaces
{
    public interface IFileRepository
    {
        Int32 Save(Image logo);
        void Delete(Int32 id);
        Image GetById(int id);

        int GetId(string filename);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Utility.Domain;

namespace DRCOG.Utility.ServiceInterfaces
{
    public interface IImageService
    {
        Int32 Save(Image logo);
        void Delete(Int32 id);
        Image GetById(int id);
    }
}

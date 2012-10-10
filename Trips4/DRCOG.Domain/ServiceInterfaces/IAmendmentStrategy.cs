using System;
using DRCOG.Domain.Models;

namespace DRCOG.Domain.ServiceInterfaces
{
    public interface IAmendmentStrategy
    {
        //ProjectAmendments UpdateStatus(ProjectAmendments model);
        //ProjectAmendments Copy(ProjectAmendments amendment);
        Int32 Amend();
    }
}

//======================================================
#region  Data Transfer Solutions License
//Copyright (c) 2008 Data Transfer Solutions (www.edats.com)
#endregion
//======================================================
// Author: Nick Kirkes
// Date Created: 8/5/2009 1:25:32 PM
// Description:
//
//======================================================
using System;
using System.Collections.Generic;
using DRCOG.Domain.Models;
using DRCOG.Domain.ViewModels;

namespace DRCOG.Domain.Interfaces
{
    public interface IAmendmentRepository : IBaseRepository
    {
        IList<AmendmentSummary> GetProjectAmendments(int projectId);
        AmendmentViewModel GetAmendmentViewModel(int amendmentId);
        AmendmentListViewModel GetAmendmentListViewModel(int projectId);
    }
}

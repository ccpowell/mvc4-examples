#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR		    REMARKS
 * 03/16/2010	DTucker         1. Initial Creation.
 * 
 * DESCRIPTION:
 * Delete Strategy Abstract class
 * ======================================================*/
#endregion

using System;
using DRCOG.Domain.Models;
using DRCOG.Domain.Models.TIPProject;

namespace DRCOG.Domain.Models.TIPProject.Amendment
{
    public abstract class DeleteStrategy
    {
        public abstract Boolean CanDelete();
    }
}

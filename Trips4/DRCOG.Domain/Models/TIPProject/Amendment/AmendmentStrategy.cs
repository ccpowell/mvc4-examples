#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR		    REMARKS
 * 03/05/2010	DTucker         1. Initial Creation.
 * 
 * DESCRIPTION:
 * Amendment Strategy Abstract class
 * ======================================================*/
#endregion

using System;
using DRCOG.Domain.Models;
using DRCOG.Domain.Models.TIPProject;

namespace DRCOG.Domain.Models.TIPProject.Amendment
{
    public abstract class AmendmentStrategy
    {
        //private Int32 _versionStatus;
        public abstract TipProjectAmendments UpdateStatus(TipProjectAmendments model);
        public abstract Boolean RequireProjectCopy();
        public Int32 VersionStatus
        {
            get;
            set;
        }

        public virtual Boolean RequireVersionStatusUpdate()
        {
            return false;
        }
    }
}

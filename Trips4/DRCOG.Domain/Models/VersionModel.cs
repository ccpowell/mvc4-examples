#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR		    REMARKS
 * 03/05/2010	DTucker         1. Initial Creation.
 * 
 * DESCRIPTION:
 * Context class for handling Project Versions
 * ======================================================*/
#endregion

using System;
using DRCOG.Domain.Interfaces;
using System.Collections.Generic;

namespace DRCOG.Domain.Models
{
    public abstract class VersionModel : ProjectModel
    {
        //private string[] CAN_DELETE_STATUS = { "approved", "submitted", "amended", "proposed" };

        public int? ProjectVersionId { get; set; } // RtpProjectVersion
        public string Year { get; set; }
        protected int _amendmentStatusId;

        //public abstract IEnumerable<int> InAmendmentCheck { get; set; }
        //public abstract bool IsInAmendment { get; set; }
        

        public int AmendmentStatusId { set { _amendmentStatusId = value; } get { return _amendmentStatusId; } }

        /// <summary>
        /// Is this version editable
        /// Rules: ProjectVersion is in the CurrentRtp and it is the CurrentScenario
        /// </summary>
        public Boolean IsEditable { get; set; }
        public Boolean IsActive { get; set; }
        public Boolean IsCurrent { get; set; }
        public Boolean IsTopStatus { get; set; }
    }
}

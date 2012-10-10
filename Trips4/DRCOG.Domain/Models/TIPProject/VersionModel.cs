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
//using DRCOG.Domain.Models.TIPProject.Amendment;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Enums;
using System.Collections.Generic;

namespace DRCOG.Domain.Models.TIPProject
{
    public abstract class VersionModel : ProjectModel
    {
        private string[] CAN_DELETE_STATUS = { "approved", "submitted", "amended", "proposed" };

        public int? ProjectVersionId { get; set; } // TipProjectVersion
        public string TipYear { get; set; }

        /// <summary>
        /// Is this version editable
        /// Rules: ProjectVersion is in the CurrentTIP and it is the CurrentScenario
        /// </summary>
        public Boolean IsEditable { get; set; }
        public Boolean IsActive { get; set; }
        public Boolean IsTipCurrent { get; set; }
        public Boolean IsTopStatus { get; set; }
            

        public Boolean CanDelete(String status)
        {
            if (((IList<string>) CAN_DELETE_STATUS).Contains(status.ToLower()))
            {
                return true;
            }
            return false;
        }
        // TimePeriod via TipProjectVersion.TimePeriodId
    }
}

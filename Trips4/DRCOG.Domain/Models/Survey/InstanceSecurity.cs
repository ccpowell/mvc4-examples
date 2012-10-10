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
using DRCOG.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DRCOG.Domain.Helpers;

namespace DRCOG.Domain.Models.Survey
{
    public class InstanceSecurity
    {
        private string[] CAN_DELETE_STATUS = { "approved", "submitted", "amended", "proposed" };
        public int UpdateStatusId { get; set; }
        public int VersionStatusId { get; set; }
        public int ActionStatusId { get; set; }
        public bool IsSponsorContact { get; set; }

        public Boolean CanDelete(String status)
        {
            if (((IList<string>)CAN_DELETE_STATUS).Contains(status.ToLower()))
            {
                return true;
            }
            return false;
        }

        public enum EditLevel
        {
            Admin
            ,
            Sponsor
        }

        public string UpdateStatus
        {
            get
            {
                switch (this.UpdateStatusId)
                {
                    case (int)Enums.SurveyUpdateStatus.Edited:
                        return Enums.SurveyUpdateStatus.Edited.ToString();
                    case (int)Enums.SurveyUpdateStatus.Completed:
                        return Enums.SurveyUpdateStatus.Completed.ToString();
                    case (int)Enums.SurveyUpdateStatus.Carryover:
                        return Enums.SurveyUpdateStatus.Carryover.ToString();
                    case (int)Enums.SurveyUpdateStatus.Reclassified:
                        return Enums.SurveyUpdateStatus.Reclassified.ToString();
                    case (int)Enums.SurveyUpdateStatus.Reviewed:
                        return Enums.SurveyUpdateStatus.Reviewed.ToString();
                    case (int)Enums.SurveyUpdateStatus.Withdrawn:
                        return Enums.SurveyUpdateStatus.Withdrawn.ToString();
                    case (int)Enums.SurveyUpdateStatus.Accepted:
                        return Enums.SurveyUpdateStatus.Accepted.ToString();
                    case (int)Enums.SurveyUpdateStatus.Current:
                        return Enums.SurveyUpdateStatus.Current.GetStringValue();
                    case (int)Enums.SurveyUpdateStatus.AwaitingAction:
                        return Enums.SurveyUpdateStatus.AwaitingAction.ToString();
                    default:
                        return String.Empty;
                }
            }
        }

        public IEnumerable<int> InEditCheck = new int[] { (int)Enums.SurveyUpdateStatus.Edited };


        public bool IsInEditMode
        {
            get { return InEditCheck.Contains(UpdateStatusId); }
        }


        public bool IsEditable()
        {
            return IsEditable(EditLevel.Sponsor);
        }

        public bool IsEditable(EditLevel level)
        {
            bool CanEdit;
            CanEdit = HttpContext.Current.User.IsInRole("Survey Administrator") || HttpContext.Current.User.IsInRole("Administrator") ? true : false;
            switch (level)
            {
                case EditLevel.Admin:
                    return CanEdit;
                case EditLevel.Sponsor:
                    return (CanEdit || (IsSponsorContact && IsInEditMode)) ? true : false;
                    //Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin) || (Model.Project.IsSponsorContact && (Model.Project.IsEditable() || Model.Current.IsOpen()))
                default:
                    return CanEdit;
            }
        }

        

    }
}

#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR		    REMARKS
 * 08/03/2009	DBouwman        1. Initial Creation (DTS).
 * 
 * DESCRIPTION:
 * 
 * ======================================================*/
#endregion

using System;

namespace DRCOG.Domain.Models
{
    /// <summary>
    /// Baseclass for the ProjectViewModels.
    /// 
    /// </summary>
    public class ProjectVersionModelBase
    {
        private String _Limits;
        private String _BeginAt;
        private String _EndAt;
        private String _FacilityName;
        private String _ProjectName;

        public Int32 ProjectVersionId { get; set; }

        public Int32 ProjectID { get; set; }
        public Int32 PoolID { get; set; }
        public Int32 PoolMasterVersionID { get; set; }
        public String Limits
        {
            get { return _Limits; }
            set
            {
                if (value.Length <= 255)
                {
                    _Limits = value;
                }
                else _Limits = null;
            }
        }
        public String BeginAt
        {
            get { return _BeginAt; }
            set
            {
                if (value.Length <= 50)
                {
                    _BeginAt = value;
                }
                else _BeginAt = null;
            }
        }
        public String EndAt
        {
            get { return _EndAt; }
            set
            {
                if (value.Length <= 50)
                {
                    _EndAt = value;
                }
                else _EndAt = null;
            }
        }
        public String FacilityName
        {
            get { return _FacilityName; }
            set
            {
                if (value.Length <= 75)
                {
                    _FacilityName = value;
                }
                else _FacilityName = null;
            }
        }  
        public Int32 AmendmentTypeID { get; set; }
        public Int32 AmendmentReasonID { get; set; }
        public Int32 Temp_PreviousScope { get; set; }
        public Int32 PreviousProjectVersionID { get; set; }
        public Int32 AmendmentStatusID { get; set; }
        public DateTime AmendmentDate { get; set; }
        public DateTime CreationDate { get; set; }
        public Int32 AmendmentFundingTypeID { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public String ProjectName
        {
            get { return _ProjectName; }
            set
            {
                if (value.Length <= 100)
                {
                    _ProjectName = value;
                }
                else _ProjectName = null;
            }
        }  
        public String Scope { get; set; }
        public Int32 SponsorContactID { get; set; }
        public String SponsorNotes { get; set; }
        public String DRCOGNotes { get; set; }
        public Int32 LocationMapID { get; set; }
        public String AmendmentCharacter { get; set; }
        public String AmendmentReason { get; set; }
        public Int32 ModelingStatusID { get; set; }
        public Int32 ShapeFileID { get; set; }
        public Int16 BeginConstructionYear { get; set; }

    }
}

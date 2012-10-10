#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR		    REMARKS
 * 02/24/2009	DTucker        1. Initial Creation.
 * DESCRIPTION:
 * 
 * ======================================================*/
#endregion
using System;

namespace DRCOG.Domain.Models.TIPProject
{
    public class ProjectModelCodingModel : VersionModel
    {
        private String _scenarioNameID;
        private String _temp_OldEndConstr;
        private String _temp_RegionRank;
        private String _temp_TIPSelectNum;
        private String _temp_UniqueID;

        public Int32 ProjectModelCodingID { get; set; }
        public Int32? ProjectSegmentID { get; set; }
        public String ScenarioNameID
        {
            get { return _scenarioNameID; }
            set
            {
                if (value.Length <= 50)
                {
                    _scenarioNameID = value;
                }
                else throw (new ArgumentException("ScenarioNameID must be less than or equal to 50 characters"));
            }
        }
        public Int32? CodingStatusID { get; set; }
        public String Notes { get; set; }
        public String Temp_OldEndConstr
        {
            get { return _temp_OldEndConstr; }
            set
            {
                if (value.Length <= 50)
                {
                    _temp_OldEndConstr = value;
                }
                else throw (new ArgumentException("Temp_OldEndConstr must be less than or equal to 50 characters"));
            }
        }
        public String Temp_RegionRank
        {
            get { return _temp_RegionRank; }
            set
            {
                if (value.Length <= 50)
                {
                    _temp_RegionRank = value;
                }
                else throw (new ArgumentException("Temp_RegionRank must be less than or equal to 50 characters"));
            }
        }
        public String Temp_TIPSelectNum
        {
            get { return _temp_TIPSelectNum; }
            set
            {
                if (value.Length <= 50)
                {
                    _temp_TIPSelectNum = value;
                }
                else throw (new ArgumentException("Temp_TIPSelectNum must be less than or equal to 50 characters"));
            }
        }
        public String Temp_UniqueID
        {
            get { return _temp_UniqueID; }
            set
            {
                if (value.Length <= 50)
                {
                    _temp_UniqueID = value;
                }
                else throw (new ArgumentException("Temp_UniqueID must be less than or equal to 50 characters"));
            }
        }
    }
}

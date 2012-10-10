#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR		    REMARKS
 * 02/25/2010   DTucker         1. Initial Creation.
 * DESCRIPTION:
 * 
 * ======================================================*/
#endregion
using System;

namespace DRCOG.Domain.Models.RTP
{
    public class PoolProject
    {
        private String _ProjectName;
        private String _Description;
        private String _BeginAt;
        private String _EndAt;

        public Int32 PoolProjectID { get; set; }
        public String ProjectName
        {
            get { return _ProjectName; }
            set
            {
                if (value.Length <= 255)
                {
                    _ProjectName = value;
                }
                else throw (new ArgumentException("ProjectName must be less than or equal to 255 characters"));
            }
        }
        public String Description
        {
            get { return _Description; }
            set
            {
                if (value.Length <= 75)
                {
                    _Description = value;
                }
                else throw (new ArgumentException("Description must be less than or equal to 75 characters"));
            }
        }
        public String BeginAt
        {
            get { return _BeginAt; }
            set
            {
                if (value.Length <= 75)
                {
                    _BeginAt = value;
                }
                else throw (new ArgumentException("BeginAt must be less than or equal to 75 characters"));
            }
        }
        public String EndAt
        {
            get { return _EndAt; }
            set
            {
                if (value.Length <= 75)
                {
                    _EndAt = value;
                }
                else throw (new ArgumentException("EndAt must be less than or equal to 75 characters"));
            }
        }
        public Decimal? Cost { get; set; }
        public Int32? PoolMasterVersionID { get; set; }
    }
}

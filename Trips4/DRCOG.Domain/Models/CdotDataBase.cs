using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Models.TIPProject;
using DRCOG.Domain.Factories;

namespace DRCOG.Domain.Models
{
    public class CdotDataBase : ICdotData
    {
        private String _routeSegment;
        private String _STIPID;
        private String _STIPProjectDivision;
        private String _TPRAbbr;
        private String _InvestmentCategory;
        private String _CorridorID;
        private String _CDOTProjectNumber;
        private String _CMSNumber;

        protected bool HasDate(DateTime datetime)
        {
            return !datetime.Equals(DateTime.MinValue);
        }

        public string ShowShortDate(DateTime datetime)
        {
            return HasDate(datetime) ? datetime.ToShortDateString() : String.Empty;
        }


        public Int16? Region { get; set; }
        public Int16? CommDistrict { get; set; }
        public String RouteSegment
        {
            get { return _routeSegment; }
            set
            {
                if (value.Length <= 8)
                {
                    _routeSegment = value;
                }
                else throw (new ArgumentException("RouteSegment must be less than or equal to 8 characters"));
            }
        }
        public Double BeginMilePost { get; set; }
        public Double EndMilePost { get; set; }
        public String STIPID
        {
            get { return _STIPID; }
            set
            {
                if (value.Length <= 20)
                {
                    _STIPID = value;
                }
                else throw (new ArgumentException("STIPID must be less than or equal to 20 characters"));
            }
        }
        public String STIPProjectDivision
        {
            get { return _STIPProjectDivision; }
            set
            {
                if (value.Length <= 15)
                {
                    _STIPProjectDivision = value;
                }
                else throw (new ArgumentException("STIPProjectDivision must be less than or equal to 15 characters"));
            }
        }
        public Int32 CDOTProjectManager { get; set; }
        public String TPRAbbr
        {
            get { return _TPRAbbr; }
            set
            {
                if (value.Length <= 3) _TPRAbbr = value;
                else throw (new ArgumentException("TPRAbbr must be less than or equal to 3 characters"));
            }
        }
        public Int16 TPRID { get; set; }
        public Int32 LRPNumber { get; set; }
        public String InvestmentCategory
        {
            get { return _InvestmentCategory; }
            set
            {
                if (value.Length <= 50) _InvestmentCategory = value;
                else throw (new ArgumentException("InvestmentCategory must be less than or equal to 50 characters"));
            }
        }
        public String CorridorID
        {
            get { return _CorridorID; }
            set
            {
                if (value.Length <= 50) _CorridorID = value;
                else throw (new ArgumentException("CorridorID must be less than or equal to 50 characters"));
            }
        }
        public String CDOTProjectNumber
        {
            get { return _CDOTProjectNumber; }
            set
            {
                if (value.Length <= 15) _CDOTProjectNumber = value;
                else throw (new ArgumentException("CDOTProjectNumber must be less than or equal to 15 characters"));
            }
        }
        public Int32 SubAccount { get; set; }
        public Int32 ConstructionRE { get; set; }
        public String CMSNumber
        {
            get { return _CMSNumber; }
            set
            {
                if (value.Length <= 6) _CMSNumber = value;
                else throw (new ArgumentException("CMSNumber must be less than or equal to 6 characters"));
            }
        }

        private DateTime _scheduledADDate;

        public DateTime ScheduledADDate {
            get
            {
                return _scheduledADDate;
            }
            set { _scheduledADDate = value.Date; }
        }
        private DateTime _projectStageDate;

        public DateTime ProjectStageDate
        {
            get
            {
                return _projectStageDate;
            }
            set { _projectStageDate = value.Date; }
        }
        private DateTime _constructionDate;

        public DateTime ConstructionDate
        {
            get
            {
                return _constructionDate;
            }
            set { _constructionDate = value.Date; }
        }
        private DateTime _projectClosed;

        public DateTime ProjectClosed
        {
            get
            {
                return _projectClosed;
            }
            set { _projectClosed = value.Date; }
        }
        public Int32 ProjectStage { get; set; }
        public String Notes { get; set; }
    }
}

using System;

namespace DRCOG.Domain.Models.TIPProject
{
    public class SegmentModel : TipVersionModel
    {
        private String _FacilityName;
        private String _StartAt;
        private String _EndAt;
        private String _LRSLinkage;

        public int SegmentId { get; set; }
        
        public int? ImprovementTypeId { get; set; }
        public int? ModelingFacilityTypeId { get; set; }
        public int? PlanFacilityTypeId { get; set; }
        public int? NetworkId { get; set; }
        public short? OpenYear { get; set; }
        public string Network { get; set; }
        
        public String FacilityName
        {
            get { return _FacilityName; }
            set
            {
                if (value != null)
                {
                    if (value.Length <= 75)
                    {
                        _FacilityName = value;
                    }
                    else throw (new ArgumentException("FacilityName must be less than or equal to 75 characters"));
                }
                else throw (new ArgumentNullException("FacilityName"));
            }
        }
        public String StartAt
        {
            get { return _StartAt; }
            set
            {
                if (value.Length <= 50)
                {
                    _StartAt = value;
                }
                else throw (new ArgumentException("StartAt must be less than or equal to 50 characters"));
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
                else throw (new ArgumentException("EndAt must be less than or equal to 50 characters"));
            }
        }
        public decimal? Length { get; set; }
        //ToDo: Check out database DBType
        //public Single? Length { get; set; }
        public short? LanesBase { get; set; }
        public short? LanesFuture { get; set; }
        public short? SpacesFuture { get; set; }
        public short? VehiclesFuture { get; set; }
        public Int32? LRSObjectID { get; set; }
        public Int32? AssignmentStatusID { get; set; }
        public String LRSLinkage
        {
            get { return _LRSLinkage; }
            set
            {
                if (value.Length <= 50)
                {
                    _LRSLinkage = value;
                }
                else throw (new ArgumentException("LRSLinkage must be less than or equal to 50 characters"));
            }
        }
        public Int32? LRSLinkageStatusID { get; set; }
        public Boolean? NeedLocationMap { get; set; }
        public Int32? Temp_PreviousImproveID { get; set; }
        public Boolean? ModelingCheck { get; set; }
        //public Single? Cost { get; set; }
        public decimal? Cost { get; set; }

        // not seen in db
        //public int? PreviousSegmentId { get; set; }

    }
}

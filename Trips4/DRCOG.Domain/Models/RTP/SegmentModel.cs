using System;

namespace DRCOG.Domain.Models.RTP
{
    public class SegmentModel : RtpVersionModel
    {
        public SegmentModel() 
        {
            //LRS = new LRS();
            LRSRecord = new LRSRecord();
        }
        public SegmentModel(string planFacilityType, string improvementType, string network)
        {
            ImprovementType = !String.IsNullOrEmpty(improvementType) ? improvementType : String.Empty;
            PlanFacilityType = !String.IsNullOrEmpty(planFacilityType) ? planFacilityType : String.Empty;
            Network = !String.IsNullOrEmpty(network) ? network : String.Empty;
        }


        private String _FacilityName;
        private String _StartAt;
        private String _EndAt;
        private String _LRSLinkage;
        public readonly string ImprovementType;
        public readonly string PlanFacilityType;
        public readonly string Network;

        public int SegmentId { get; set; }

        //public LRS LRS { get; set; }

        public string LRSRecordRaw { get; set; }
        public Scheme LRSSchemeBase { get; set; }
        //public LRSRecord _LRS { get; set; }
        public LRSRecord LRSRecord { get; set; }
        public LRSRecords _LRS { get; set; }
        public string LRSxml { get; set; }

        public int ImprovementTypeId { get; set; }
        public int ModelingFacilityTypeId { get; set; }
        public int PlanFacilityTypeId { get; set; }
        public int NetworkId { get; set; }
        public string Staging { get; set; }
        public short OpenYear { get; set; }
        
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
        public short LanesBase { get; set; }
        public short LanesFuture { get; set; }
        public short SpacesFuture { get; set; }
        public short VehiclesFuture { get; set; }
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

    

    public class LRSAttr
    {
        private DataType _dataType;

        public String Id { get; set; }
        public String LRSSchemeId { get; set; }
        public String ColumnName { get; set; }

        public Int32 MaxLength { get; set; }
        public String Default { get; set; }
        public Boolean IsNullable { get; set; }
        public Boolean IsRequired { get; set; }

        public DataType GetDataType() { return _dataType; }
        public void SetDataType(string value)
        {
            switch (value)
            {
                case "int": this._dataType = DataType.Int; break;
                case "nvarchar": this._dataType = DataType.Nvarchar; break;
                case "float": this._dataType = DataType.Float; break;
            }
        }
    }

    public enum DataType
    {
        Int
        ,
        Nvarchar
        ,
        Float
    }
}

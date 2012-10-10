using System;

using DRCOG.Domain.Models.TIPProject;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.Domain.Interfaces;
using System.Collections.Generic;

namespace DRCOG.Domain.Models
{
    public class ProjectAmendments : VersionModel
    {
        public Int32 PreviousProjectVersionId { get; set; }
        //public Int32 AmendmentStatusId { get; set; }
        public String AmendmentStatus { get; set; }
        public String VersionStatus { get; set; }
        public DateTime AmendmentDate { get; set; }
        public Int32 AmendmentTypeId { get; set; }
        public Int32 VersionStatusId { get; set; }
        public String AmendmentReason { get; set; }
        public String AmendmentCharacter { get; set; }
        public Boolean UpdateLocationMap { get; set; }
        public String LocationMapPath { get; set; }

        public ProjectAmendments()
        {
        }
    }
}

using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class Project
    {
        public Project()
        {
            this.ProjectMuniGeographies = new List<ProjectMuniGeography>();
            this.ProjectSponsors = new List<ProjectSponsor>();
            this.Strikes = new List<Strike>();
        }

        public int ProjectID { get; set; }
        public string COGID { get; set; }
        public Nullable<int> ImprovementTypeID { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<bool> RegionalSignificance { get; set; }
        public Nullable<int> SelectorID { get; set; }
        public Nullable<int> RouteID { get; set; }
        public Nullable<int> AdministrativeLevelID { get; set; }
        public Nullable<int> TransportationTypeID { get; set; }
        public Nullable<int> SubmittedByID { get; set; }
        public Nullable<bool> ComplexProject { get; set; }
        public Nullable<int> temp_projectIDfromtip2010 { get; set; }
        public virtual Category Category { get; set; }
        public virtual Category Category1 { get; set; }
        public virtual Category Category2 { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<ProjectMuniGeography> ProjectMuniGeographies { get; set; }
        public virtual ICollection<ProjectSponsor> ProjectSponsors { get; set; }
        public virtual ICollection<Strike> Strikes { get; set; }
    }
}

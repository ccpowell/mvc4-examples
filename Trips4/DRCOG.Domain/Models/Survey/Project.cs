using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DRCOG.Domain.Models.Survey
{

    public class Project : Instance, IEquatable<Project>
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string COGID { get; set; }
        public bool IsNew { get; set; }

        public Funding Funding { get; set; }
        public IList<FundingSource> FundingSources { get; set; }
        public IList<int> EligibleContributors { get; set; }

        public bool IsContributor(int personId)
        {
            if (EligibleContributors == null) return false;
            return EligibleContributors.Contains(personId);
        }

        public bool Equals(Project other)
        {
            if (this.UpdateStatusId == other.UpdateStatusId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string ReportOnlyOpenDate { get; set; }

        
    }
}

using System;

namespace DRCOG.Domain.Models.TIPProject
{
    public abstract class ProjectModel_obs
    {
        public int? ProjectId { get; set; } // TipProject
        public string ProjectName { get; set; } // Project 
    }
}
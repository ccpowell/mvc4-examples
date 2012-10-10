using System;

namespace DRCOG.Domain.Models
{
    public abstract class ProjectModel
    {
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
}
using System;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.Domain;
using DRCOG.Domain.Models;
using DRCOG.Domain.Interfaces;

namespace DRCOG.TIP.Services.DeleteStrategy.TIP
{
    public class DeleteToSubmitted : IAmendmentStrategy
    {
        protected readonly IProjectRepository ProjectRepository;
        protected readonly ProjectAmendments Amendment;

        public DeleteToSubmitted(IProjectRepository repo, ProjectAmendments amendment)
        {
            ProjectRepository = repo;
            Amendment = UpdateStatus(amendment);
        }

        protected ProjectAmendments UpdateStatus(ProjectAmendments amendment)
        {
            //set the Amendment Reason to "Deleting previous amendment"
            amendment.AmendmentStatusId = (Int32)Enums.TIPAmendmentStatus.Submitted;
            amendment.AmendmentTypeId = (Int32)Enums.AmendmentType.Administrative;
            amendment.AmendmentReason = "Deleting previous amendment";
            amendment.AmendmentCharacter = "";
            amendment.VersionStatusId = (Int32)Enums.TIPVersionStatus.Pending;
            return amendment;
        }

        protected void Copy()
        {
            Amendment.PreviousProjectVersionId = (Int32)Amendment.ProjectVersionId; // if we went the previous of the deleted then change this.
            Amendment.ProjectVersionId = ProjectRepository.CopyProject(Amendment.PreviousProjectVersionId);
        }

        #region IAmendmentStrategy Members

        public int Amend()
        {
            Copy();
            ProjectRepository.UpdateProjectAmendmentStatus(Amendment);
            return (Int32)Amendment.ProjectVersionId;
        }

        #endregion
    }
}

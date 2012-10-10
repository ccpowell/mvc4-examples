#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR		    REMARKS
 * 03/05/2010	DTucker         1. Initial Creation.
 * 
 * DESCRIPTION:
 * Concrete business rules
 * ======================================================*/
#endregion

using System;
using DRCOG.Domain;
using DRCOG.Domain.Models;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.ServiceInterfaces;

namespace DRCOG.TIP.Services.TIP
{
    public class ProposedToAmended : IAmendmentStrategy
    {
        protected readonly IProjectRepository ProjectRepository;
        protected readonly ProjectAmendments Amendment;

        public ProposedToAmended(IProjectRepository repo, ProjectAmendments amendment)
        {
            ProjectRepository = repo;
            Amendment = UpdateStatus(amendment);
        }

        public ProjectAmendments UpdateStatus(ProjectAmendments amendment)
        {
            amendment.AmendmentStatusId = (Int32)Enums.TIPAmendmentStatus.Amended;
            //amendment.AmendmentTypeId = (Int32)Enums.AmendmentType.Administrative;
            amendment.VersionStatusId = (Int32)Enums.TIPVersionStatus.Active;
            return amendment;
        }


        #region IAmendmentStrategy Members

        public Int32 Amend()
        {
            ProjectRepository.UpdateProjectAmendmentStatus(Amendment);
            ProjectRepository.UpdateProjectVersionStatusId((Int32)Amendment.PreviousProjectVersionId, (Int32)Enums.TIPVersionStatus.Inactive);
            ProjectRepository.UpdateProjectVersionStatusId(
                GetPreviousActiveProjectVersion((Int32)Amendment.ProjectVersionId)
                , (Int32)Enums.TIPVersionStatus.Inactive);
            return (Int32)Amendment.ProjectVersionId;
        }

        private Int32 GetPreviousActiveProjectVersion(int projectVersionId)
        {
            return ProjectRepository.GetPreviousProjectVersionId(projectVersionId);
        }

        #endregion
    }
}

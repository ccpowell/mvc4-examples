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
    public class AmendedToSubmitted : IAmendmentStrategy
    {
        protected readonly IProjectRepository ProjectRepository;
        protected readonly ProjectAmendments Amendment;

        public AmendedToSubmitted(IProjectRepository repo, ProjectAmendments amendment)
        {
            ProjectRepository = repo;
            Amendment = UpdateStatus(amendment);
        }

        protected ProjectAmendments UpdateStatus(ProjectAmendments amendment)
        {
            amendment.AmendmentStatusId = (Int32)Enums.TIPAmendmentStatus.Submitted;
            //amendment.AmendmentTypeId = (Int32)Enums.AmendmentType.Administrative;
            amendment.VersionStatusId = (Int32)Enums.TIPVersionStatus.Pending;
            return amendment;
        }

        protected void Copy()
        {
            Amendment.PreviousProjectVersionId = (Int32)Amendment.ProjectVersionId;
            Amendment.ProjectVersionId = ProjectRepository.CopyProject(Amendment.PreviousProjectVersionId);
        }

        #region IAmendmentStrategy Members

        public Int32 Amend()
        {
            Copy();
            ProjectRepository.UpdateProjectAmendmentStatus(Amendment);
            return (Int32)Amendment.ProjectVersionId;
        }

        #endregion
    }
}

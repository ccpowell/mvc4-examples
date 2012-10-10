#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR		    REMARKS
 * 03/05/2010	DTucker         1. Initial Creation.
 * 
 * DESCRIPTION:
 * Amendment Strategy Abstract class
 * ======================================================*/
#endregion

using System;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.Domain.Models;
using DRCOG.Domain;

namespace DRCOG.TIP.Services.TIP
{
    public class AmendmentStrategy
    {
        protected readonly IProjectRepository ProjectRepository;
        protected readonly ProjectAmendments Amendment;

        /*
            *Get Current Amendment Status
             * Does project need to be copied?
             * Amend Project
             * Check if Previous Active Amendment needs to be changed to inactive
             * Return to details page
            
            ProjectAmendments amendment = new ProjectAmendments();
            amendment = amendmentViewModel.ProjectAmendments;
            if (amendment.RequiresProjectCopy(_projectRepository.GetProjectAmendmentStatus((Int32)amendment.ProjectVersionId)))
            {
                amendment.LocationMapPath = DRCOGConfig.GetConfig().LocationMapPath;
                amendment.PreviousProjectVersionId = (Int32)amendment.ProjectVersionId; 
                amendment.ProjectVersionId = _projectRepository.CopyProject(amendment.PreviousProjectVersionId);
            }
            amendment = amendment.AmendProject();
            
            _projectRepository.UpdateProjectAmendmentStatus(amendment);
            if (amendment.RequireVersionStatusUpdate())
            {
                _projectRepository.UpdateProjectVersionStatusId((Int32)amendment.PreviousProjectVersionId, amendment.VersionStatusId);
            }
        */

        public AmendmentStrategy(IProjectRepository repo, ProjectAmendments amendment)
        {
            ProjectRepository = repo;
            Amendment = amendment;
        }

        public int GetAmendmentStatusId(int projectVersionId)
        {
            return ProjectRepository.GetProjectAmendmentStatus(projectVersionId);
        }

        public IAmendmentStrategy PickStrategy()
        {
            IAmendmentStrategy strategy = null;

            switch (GetAmendmentStatusId((Int32)Amendment.ProjectVersionId))
            {
                case 10734: // Amended
                case (Int32)Enums.TIPAmendmentStatus.Amended:
                case (Int32)Enums.TIPAmendmentStatus.Adopted:
                    strategy = new AmendedToSubmitted(ProjectRepository, Amendment);
                    break;
                case 10736: // Approved
                case 1769:
                    strategy = new ApprovalToSubmitted(ProjectRepository, Amendment);
                    break;
                case 10740: // Proposed
                case 1767:
                    strategy = new ProposedToAmended(ProjectRepository, Amendment);
                    break;
                case 10742: // Submitted
                case 1770:
                    strategy = new SubmittedToProposed(ProjectRepository, Amendment);
                    break;
            }
            return strategy;
        }
        
    }
}

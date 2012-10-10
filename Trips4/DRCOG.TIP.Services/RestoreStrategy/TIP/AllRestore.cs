using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Models;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.Domain;

namespace DRCOG.TIP.Services.RestoreStrategy.TIP
{
    class AllRestore : IRestoreStrategy
    {
        private readonly IProjectRepository _repo;
        private Int32 _projectVersionID;

        public AllRestore(Int32 projectVersionID, IProjectRepository repo)
        {
            _repo = repo;
            _projectVersionID = projectVersionID;
        }

        protected int Copy(string tipYear, ProjectAmendments amendment)
        {
            if (amendment.AmendmentStatusId == (Int32)Enums.TIPAmendmentStatus.Proposed)
            {
                amendment.ProjectVersionId = _projectVersionID;
                amendment.PreviousProjectVersionId = (Int32)amendment.ProjectVersionId;
                amendment.ProjectVersionId = _repo.CopyProject(amendment.PreviousProjectVersionId);
                amendment.AmendmentReason = String.Empty;
                amendment.AmendmentCharacter = "Proposed for adoption into " + tipYear;
            }
            else
            {
                amendment.ProjectVersionId = _repo.RestoreProjectVersion(_projectVersionID, tipYear);
                _repo.RestoreProjectVersionFinancials(_projectVersionID, (int)amendment.ProjectVersionId, tipYear);
                amendment.AmendmentReason = String.Empty;
                amendment.AmendmentCharacter = "Imported from " + tipYear;
                //amendment.ProjectVersionId = newProjectVersionID;
            }
            _repo.UpdateProjectAmendmentStatus(amendment);
            _repo.UpdateAmendmentDetails(amendment);
            return (int)amendment.ProjectVersionId;
        }

        #region IRestoreStrategy Members

        public object Restore(string tipYear)
        {
            _projectVersionID = Copy(tipYear, new ProjectAmendments() 
            { 
                AmendmentStatusId = (Int32)Enums.TIPAmendmentStatus.Submitted
                , 
                VersionStatusId = (Int32)Enums.TIPVersionStatus.Inactive 
            });

            return Copy(tipYear, new ProjectAmendments()
            {
                AmendmentStatusId = (Int32)Enums.TIPAmendmentStatus.Proposed
                ,
                VersionStatusId = (Int32)Enums.TIPVersionStatus.Pending
            });
        }

        #endregion
    }
}

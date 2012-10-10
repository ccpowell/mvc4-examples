using DRCOG.Domain.ServiceInterfaces;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Models;
using DRCOG.Domain;
using DRCOG.Domain.Models.Survey;
using System;

namespace DRCOG.TIP.Services.AmendmentStrategy.Survey
{
    public class SponsorNew : IAmendmentStrategy
    {
        protected readonly ISurveyRepository SurveyRepository;
        protected readonly Instance Version;

        public SponsorNew(ISurveyRepository repo, Instance version)
        {
            SurveyRepository = repo;
            Version = version;
        }

        protected void Copy()
        {
            this.Version.PreviousVersionId = this.Version.ProjectVersionId;
            this.Version.ProjectVersionId = !this.Version.TimePeriodId.Equals(default(int)) 
                ? (int)SurveyRepository.CopyProject(this.Version.PreviousVersionId, this.Version.TimePeriodId).ProjectVersionId 
                : (int)SurveyRepository.CopyProject(this.Version.PreviousVersionId).ProjectVersionId;
        }

        #region IAmendmentStrategy Members

        public Int32 Amend()
        {
            try
            {
                Copy();
                SurveyRepository.SetSurveyStatus(Version);
                return (Int32)Version.ProjectVersionId;
            }
            catch
            {
            }

            // try and return the new project version if we got that far.
            if (!Version.ProjectVersionId.Equals(Version.PreviousVersionId))
                return (Int32)Version.ProjectVersionId;
            return default(Int32);
        }

        #endregion
    }
}

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
using DRCOG.Domain.Models.Survey;
using DRCOG.Domain;

namespace DRCOG.TIP.Services.AmendmentStrategy.Survey
{
    public class AmendmentStrategy
    {
        protected readonly ISurveyRepository SurveyRepository;
        protected readonly Instance Version;

        /*
            *Get Current Amendment Status
             * Does project need to be copied?
             * Amend Project
             * Check if Previous Active Amendment needs to be changed to inactive
             * Return to details page
        */

        public AmendmentStrategy(ISurveyRepository repo, Instance version)
        {
            SurveyRepository = repo;
            Version = version;
        }

        public IAmendmentStrategy PickStrategy()
        {
            IAmendmentStrategy strategy = null;

            switch (Version.UpdateStatusId)
            {
                case (int)Enums.SurveyUpdateStatus.Edited:
                    strategy = new SponsorEdit(SurveyRepository, Version);
                    break;
                case (int)Enums.SurveyUpdateStatus.AwaitingAction:
                    strategy = new SponsorNew(SurveyRepository, Version);
                    break;
            }
            return strategy;
        }
        
    }
}

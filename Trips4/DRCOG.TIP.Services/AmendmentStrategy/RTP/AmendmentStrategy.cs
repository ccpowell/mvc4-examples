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
using DRCOG.Domain.Models.RTP;
using DRCOG.Domain;

namespace DRCOG.TIP.Services.AmendmentStrategy.RTP
{
    public class AmendmentStrategy
    {
        protected readonly IRtpProjectRepository RtpProjectRepository;
        protected readonly CycleAmendment Amendment;

        /*
            *Get Current Amendment Status
             * Does project need to be copied?
             * Amend Project
             * Check if Previous Active Amendment needs to be changed to inactive
             * Return to details page
        */

        public AmendmentStrategy(IRtpProjectRepository repo, CycleAmendment amendment)
        {
            RtpProjectRepository = repo;
            Amendment = amendment;
        }

        public int GetAmendmentStatusId(int projectVersionId)
        {
            return RtpProjectRepository.GetProjectAmendmentStatus(projectVersionId);
        }

        public IAmendmentStrategy PickStrategy()
        {
            IAmendmentStrategy strategy = null;

            switch (GetAmendmentStatusId(Amendment.ProjectVersionId))
            {
                case (int)Enums.RTPAmendmentStatus.Pending:
                    strategy = new PendingToAdopted(RtpProjectRepository, Amendment);
                    break;
                case (int)Enums.RTPAmendmentStatus.Submitted:
                    strategy = new SubmittedToPending(RtpProjectRepository, Amendment);
                    break;
                case (int)Enums.RTPAmendmentStatus.Amended:
                case (int)Enums.RTPAmendmentStatus.Approved:
                    strategy = new AdoptedToPending(RtpProjectRepository, Amendment);
                    break;
            }
            return strategy;
        }
        
    }
}

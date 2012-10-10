using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.Domain.Models.RTP;
using DRCOG.Data;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain;
using DRCOG.Domain.Models;

namespace DRCOG.TIP.Services
{
    public static class CycleService
    {
        private readonly static IRtpRepository RtpRepository = new RtpRepository();


        private static ICollection<CycleAmendment> GetCollectionOfCycles(int timePeriodId)
        {
            return RtpRepository.GetCollectionOfCycles(timePeriodId);
        }

        #region ICycleService Members

        public static Cycle GetCurrentCycle(int timePeriodId)
        {
            var cycles = GetCollectionOfCycles(timePeriodId);

            var value = (Cycle)cycles.SingleOrDefault(x => x.StatusId == (int)Enums.RTPCycleStatus.Active) ?? (Cycle)cycles.SingleOrDefault(x => x.StatusId == (int)Enums.RTPCycleStatus.Pending);

            return value;
        }

        #endregion
    }
}

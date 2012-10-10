using System;
using DRCOG.Domain.Models.RTP;
using DRCOG.Domain.Factories;
using DRCOG.Domain.Interfaces;

namespace DRCOG.Domain.Models.RTP
{
    public partial class RtpCdotData : CdotDataBase
    {
        public IInstance versionModel;
        public RtpCdotData()
        {
            versionModel = GenericAddOnFactory<IInstance, RtpVersionModel>.CreateInstance();
        }
    }
}

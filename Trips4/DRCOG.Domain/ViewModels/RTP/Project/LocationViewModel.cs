using System;
using System.Collections.Generic;
using DRCOG.Domain.Models;
using DRCOG.Domain.Models.RTP;

namespace DRCOG.Domain.ViewModels.RTP.Project
{
    public class LocationViewModel : ProjectBaseViewModel
    {

        public LocationModel RtpProjectLocation { get; set; }
        public IDictionary<int, string> AvailableCounties { get; set; }
        public IDictionary<int, string> AvailableMunicipalities { get; set; }
        public IDictionary<int, string> AvailableRoutes { get; set; }
        public IList<CountyShareModel> CountyShares { get; set; }       
        public IList<MunicipalityShareModel> MuniShares { get; set; }
        public RtpCdotData RtpCdotData { get; set; }

    }
}

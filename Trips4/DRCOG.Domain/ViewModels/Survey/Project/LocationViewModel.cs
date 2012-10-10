using System;
using System.Collections.Generic;
using DRCOG.Domain.Models;
using DRCOG.Domain.Models.Survey;

namespace DRCOG.Domain.ViewModels.Survey
{
    public class LocationViewModel : ProjectBaseViewModel
    {

        public LocationModel Location { get; set; }
        public IDictionary<int, string> AvailableCounties { get; set; }
        public IDictionary<int, string> AvailableMunicipalities { get; set; }
        public IDictionary<int, string> AvailableRoutes { get; set; }
        public IList<CountyShareModel> CountyShares { get; set; }       
        public IList<MunicipalityShareModel> MuniShares { get; set; }


    }
}

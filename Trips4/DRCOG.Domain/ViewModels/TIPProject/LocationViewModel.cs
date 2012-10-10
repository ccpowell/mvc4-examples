using System;
using System.Collections.Generic;
using DRCOG.Domain.Models;
using DRCOG.Domain.Models.TIPProject;
using System.Web.Mvc;

namespace DRCOG.Domain.ViewModels.TIPProject
{
    public class LocationViewModel : ProjectBaseViewModel
    {

        public LocationModel TipProjectLocation { get; set; }
        public IDictionary<int, string> AvailableCounties { get; set; }
        public IDictionary<int, string> AvailableMunicipalities { get; set; }
        public IList<CountyShareModel> CountyShares { get; set; }       
        public IList<MunicipalityShareModel> MuniShares { get; set; }
        public IEnumerable<SelectListItem> CDOTRegions { get; set; }
        public IEnumerable<SelectListItem> AffectedProjectDelaysLocation { get; set; }
    }
}

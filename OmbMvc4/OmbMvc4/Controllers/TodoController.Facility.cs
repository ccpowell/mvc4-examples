using System.Linq;
using System.Web.Http;
using System.Web.Http.Data.EntityFramework;

namespace OmbMvc4.Controllers
{
    public partial class TodoController : DbDataController<OmbMvc4.Models.TodoContext>
    {
        public IQueryable<OmbMvc4.Models.Facility> GetFacilities() {
            return DbContext.Facilities.Include("Ombudsman").OrderBy(f => f.FacilityId);
        }

        public void InsertFacility(OmbMvc4.Models.Facility entity) {
            InsertEntity(entity);
        }

        public void UpdateFacility(OmbMvc4.Models.Facility entity) {
            UpdateEntity(entity);
        }

        public void DeleteFacility(OmbMvc4.Models.Facility entity) {
            DeleteEntity(entity);
        }

        public IQueryable<OmbMvc4.Models.Ombudsman> GetOmbudsmanOptionsForFacility() {
            return DbContext.Ombudsmen;
        }
    }
}

using System.Linq;
using System.Web.Http;
using System.Web.Http.Data.EntityFramework;

namespace OmbMvc4.Controllers
{
    public partial class TodoController : DbDataController<OmbMvc4.Models.TodoContext>
    {
        public IQueryable<OmbMvc4.Models.Ombudsman> GetOmbudsmen() {
            return DbContext.Ombudsmen.OrderBy(o => o.OmbudsmanId);
        }

        public void InsertOmbudsman(OmbMvc4.Models.Ombudsman entity) {
            InsertEntity(entity);
        }

        public void UpdateOmbudsman(OmbMvc4.Models.Ombudsman entity) {
            UpdateEntity(entity);
        }

        public void DeleteOmbudsman(OmbMvc4.Models.Ombudsman entity) {
            DeleteEntity(entity);
        }
    }
}

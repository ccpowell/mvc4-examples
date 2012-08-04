using System.Linq;
using System.Web.Http;
using System.Web.Http.Data.EntityFramework;

using System.Web.Http.Data.Helpers;
namespace OmbMvc4.Controllers
{
    public partial class TodoController : DbDataController<OmbMvc4.Models.TodoContext>
    {
        public IQueryable<OmbMvc4.Models.TodoItem> GetTodoItems() {
            return DbContext.TodoItems.OrderBy(t => t.TodoItemId);
        }

        public void InsertTodoItem(OmbMvc4.Models.TodoItem entity) {
            InsertEntity(entity);
        }

        public void UpdateTodoItem(OmbMvc4.Models.TodoItem entity) {
            UpdateEntity(entity);
        }

        public void DeleteTodoItem(OmbMvc4.Models.TodoItem entity) {
            DeleteEntity(entity);
        }
    }
}

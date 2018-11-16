using SIS.Framework.ActionResults;
using SIS.Framework.Attributes.Action;
using SIS.Framework.Attributes.Method;
using TORSHIA.App.Controllers.Base;
using TORSHIA.Services.Contracts;
using TORSHIA.ViewModels.Tasks;

namespace TORSHIA.App.Controllers
{
    public class TasksController : BaseController
    {

        private readonly ITasksService tasksService;

        public TasksController(ITasksService tasksService)
        {
            this.tasksService = tasksService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Report(int taskId)
        {
            this.tasksService.ReportTask(taskId, this.Identity.Username);

            return this.RedirectToAction("/");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Details(int id)
        {
            this.Model["TasksDetails"] = this.tasksService.GetTask(id);

            return this.View();
        }

        [HttpGet]
        [Authorize("Admin")]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize("Admin")]
        public IActionResult Create(TasksCreateViewModel model)
        {
            this.tasksService.CreateTask(model);

            return this.RedirectToAction("/");
        }
    }
}

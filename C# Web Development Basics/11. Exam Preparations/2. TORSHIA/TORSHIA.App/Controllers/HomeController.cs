using System.Collections.Generic;
using System.Linq;
using SIS.Framework.ActionResults;
using SIS.Framework.Attributes.Method;
using TORSHIA.App.Controllers.Base;
using TORSHIA.Services.Contracts;
using TORSHIA.ViewModels.Tasks;

namespace TORSHIA.App.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ITasksService tasksService;

        public HomeController(ITasksService tasksService)
        {
            this.tasksService = tasksService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (this.Identity != null)
            {
                IndexTasksViewModel[] tasksViewModels = this.tasksService.GetAllNonReportedTasks().ToArray();

                var tasksRowViewModels = new List<IndexTasksRowViewModel>();

                for (int i = 0; i < tasksViewModels.Length; i++)
                {
                    if (i % 5 == 0)
                    {
                        tasksRowViewModels.Add(new IndexTasksRowViewModel());
                    }

                    tasksRowViewModels[tasksRowViewModels.Count - 1].Tasks.Add(tasksViewModels[i]);
                }

                this.Model["TasksRows"] = tasksRowViewModels;
            }

            return this.View();
        }
    }
}

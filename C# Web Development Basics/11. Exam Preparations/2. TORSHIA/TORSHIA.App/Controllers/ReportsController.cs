using SIS.Framework.ActionResults;
using SIS.Framework.Attributes.Action;
using SIS.Framework.Attributes.Method;
using TORSHIA.App.Controllers.Base;
using TORSHIA.Services.Contracts;

namespace TORSHIA.App.Controllers
{
    public class ReportsController : BaseController
    {
        private readonly IReportsService reportsService;

        public ReportsController(IReportsService reportsService)
        {
            this.reportsService = reportsService;
        }

        [HttpGet]
        [Authorize("Admin")]
        public IActionResult All()
        {
            this.Model["Reports"] = this.reportsService.GetAllReports();

            return this.View();
        }

        [HttpGet]
        [Authorize("Admin")]
        public IActionResult Details(int id)
        {
            this.Model["Report"] = this.reportsService.GetReport(id);

            return this.View();
        }
    }
}

using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Attributes.Methods;
using SIS.Framework.Controllers;

namespace IRunes.App.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            this.SetViewByUserAuthentication();

            if (this.Identity != null)
            {
                this.Model["username"] = this.Identity.Username;
            }

            return this.View();
        }
    }
}

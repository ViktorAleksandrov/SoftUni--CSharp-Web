using System.Linq;
using System.Runtime.CompilerServices;
using PANDA.App.Data;
using PANDA.App.Models.Enums;
using SIS.Framework.ActionResults;
using SIS.Framework.Controllers;

namespace PANDA.App.Controllers
{
    public class BaseController : Controller
    {
        protected readonly PandaDbContext context;

        public BaseController(PandaDbContext context)
        {
            this.context = context;
        }

        protected bool IsLoggedIn => this.Identity != null;

        protected override IViewable View([CallerMemberName] string actionName = "")
        {
            this.Model["NotLoggedIn"] = "none";
            this.Model["LoggedInUser"] = "none";
            this.Model["LoggedInAdmin"] = "none";

            if (this.IsLoggedIn)
            {
                if (this.Identity.Roles.Contains(nameof(Role.Admin)))
                {
                    this.Model["LoggedInAdmin"] = "block";
                }
                else
                {
                    this.Model["LoggedInUser"] = "block";
                }
            }
            else
            {
                this.Model["NotLoggedIn"] = "block";
            }

            return base.View(actionName);
        }
    }
}

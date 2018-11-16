using System;
using System.Linq;
using System.Runtime.CompilerServices;
using SIS.Framework.ActionResults;
using SIS.Framework.Controllers;

namespace TORSHIA.App.Controllers.Base
{
    public abstract class BaseController : Controller
    {
        protected override IViewable View([CallerMemberName] string actionName = "")
        {
            this.Model["NotLoggedIn"] = "none";
            this.Model["LoggedInUser"] = "none";
            this.Model["LoggedInAdmin"] = "none";

            if (this.Identity != null)
            {
                this.Model["Username"] = this.Identity.Username;

                if (this.Identity.Roles.Contains("Admin"))
                {
                    this.Model["LoggedInAdmin"] = "inline";
                }
                else
                {
                    this.Model["LoggedInUser"] = "inline";
                }
            }
            else
            {
                this.Model["NotLoggedIn"] = "inline";
            }

            return base.View(actionName);
        }
    }
}

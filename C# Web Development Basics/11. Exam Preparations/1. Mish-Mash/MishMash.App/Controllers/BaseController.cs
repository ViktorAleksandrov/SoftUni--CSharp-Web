using System;
using System.Linq;
using System.Runtime.CompilerServices;
using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Controllers;

namespace MishMash.App.Controllers
{
    public abstract class BaseController : Controller
    {
        protected bool IsLoggedIn => this.Identity != null;

        protected override IViewable View([CallerMemberName] string actionName = "")
        {
            this.Model["guest"] = "none";
            this.Model["user"] = "none";
            this.Model["admin"] = "none";

            if (this.IsLoggedIn)
            {
                if (this.Identity.Roles.Contains("Admin"))
                {
                    this.Model["admin"] = "inline";
                }
                else
                {
                    this.Model["user"] = "inline";
                }
            }
            else
            {
                this.Model["guest"] = "inline";
            }

            return base.View(actionName);
        }
    }
}

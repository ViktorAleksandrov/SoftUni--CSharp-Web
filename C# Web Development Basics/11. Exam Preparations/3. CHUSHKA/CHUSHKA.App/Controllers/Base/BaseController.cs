using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CHUSHKA.Models.Enums;
using SIS.Framework.ActionResults;
using SIS.Framework.Controllers;

namespace CHUSHKA.App.Controllers.Base
{
    public abstract class BaseController : Controller
    {
        protected bool IsLoggedIn => this.Identity != null;

        protected override IViewable View([CallerMemberName] string actionName = "")
        {
            this.Model["NotLoggedIn"] = "none";
            this.Model["LoggedInUser"] = "none";
            this.Model["LoggedInAdmin"] = "none";

            if (this.Identity != null)
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

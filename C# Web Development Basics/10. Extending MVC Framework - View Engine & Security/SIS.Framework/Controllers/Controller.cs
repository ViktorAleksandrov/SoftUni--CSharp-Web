using System.IO;
using System.Runtime.CompilerServices;
using SIS.Framework.ActionResults;
using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Models;
using SIS.Framework.Security.Contracts;
using SIS.Framework.Utilities;
using SIS.Framework.Views;
using SIS.HTTP.Requests.Contracts;

namespace SIS.Framework.Controllers
{
    public abstract class Controller
    {
        protected Controller()
        {
            this.Model = new ViewModel();
            this.ModelState = new Model();
            this.ViewEngine = new ViewEngine();
        }

        public IHttpRequest Request { get; set; }

        public Model ModelState { get; }

        public IIdentity Identity => (IIdentity)this.Request.Session.GetParameter("auth");

        protected ViewModel Model { get; }

        private ViewEngine ViewEngine { get; }

        protected IViewable View([CallerMemberName] string actionName = "")
        {
            string controllerName = ControllerUtilities.GetControllerName(this);

            string viewContent = null;

            try
            {
                viewContent = this.ViewEngine.GetViewContent(controllerName, actionName);
            }
            catch (FileNotFoundException e)
            {
                this.Model["Error"] = e.Message;

                viewContent = this.ViewEngine.GetErrorContent();
            }

            string renderedContent = this.ViewEngine.RenderHtml(viewContent, this.Model.Data);

            return new ViewResult(new View(renderedContent));
        }

        protected IRedirectable RedirectToAction(string redirectUrl)
        {
            return new RedirectResult(redirectUrl);
        }

        protected void SignIn(IIdentity auth)
        {
            this.Request.Session.AddParameter("auth", auth);
        }

        protected void SignOut()
        {
            this.Request.Session.ClearParameters();
        }

        protected void SetViewByUserAuthentication()
        {
            if (this.Identity == null)
            {
                this.Model.Data["IsAuthenticated"] = "none";
                this.Model.Data["IsNotAuthenticated"] = "inline";
            }
            else
            {
                this.Model.Data["IsAuthenticated"] = "inline";
                this.Model.Data["IsNotAuthenticated"] = "none";
            }
        }
    }
}

using System.Runtime.CompilerServices;
using SIS.Framework.ActionResults;
using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Models;
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
        }

        public IHttpRequest Request { get; set; }

        public Model ModelState { get; } = new Model();

        protected ViewModel Model { get; }

        protected IViewable View([CallerMemberName] string caller = "")
        {
            string controllerName = ControllerUtilities.GetControllerName(this);

            string fullyQualifiedName = ControllerUtilities.GetViewFullQualifiedName(controllerName, caller);

            var view = new View(fullyQualifiedName, this.Model.Data);

            return new ViewResult(view);
        }

        protected IRedirectable RedirectToAction(string redirectUrl)
        {
            return new RedirectResult(redirectUrl);
        }
    }
}

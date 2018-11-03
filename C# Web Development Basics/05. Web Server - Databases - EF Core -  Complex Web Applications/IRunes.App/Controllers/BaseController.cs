using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using IRunes.Data;
using IRunes.Services;
using IRunes.Services.Contracts;
using SIS.HTTP.Cookies;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Results;

namespace IRunes.App.Controllers
{
    public abstract class BaseController
    {
        private const string RootDirectoryRelativePath = "../../../";

        private const string ViewsFolderName = "Views";

        private const string DirectorySeparator = "/";

        private const string ControllerSuffixName = "Controller";

        private const string HtmlFileExtension = ".html";

        private readonly string[] notLoggedPaths = { "Index", "Register", "Login" };

        protected BaseController()
        {
            this.Db = new IRunesContext();
            this.UserCookieService = new UserCookieService();
            this.ViewBag = new Dictionary<string, string>();
        }

        protected IRunesContext Db { get; set; }

        protected IUserCookieService UserCookieService { get; set; }

        protected IDictionary<string, string> ViewBag { get; set; }

        protected bool IsAuthenticated(IHttpRequest request)
        {
            return request.Session.ContainsParameter("username");
        }

        protected IHttpResponse View([CallerMemberName] string viewName = "")
        {
            string filePath = RootDirectoryRelativePath +
                ViewsFolderName +
                DirectorySeparator +
                this.GetCurrentControllerName() +
                DirectorySeparator +
                viewName +
                HtmlFileExtension;

            string layoutFilePath = this.GetLayoutFilePath(viewName);

            string viewFileContent = File.ReadAllText(filePath);
            string layoutFileContent = File.ReadAllText(layoutFilePath);

            foreach (string viewBagKey in this.ViewBag.Keys)
            {
                string dynamicDataPlaceholder = $"{{{{{viewBagKey}}}}}";

                if (viewFileContent.Contains(dynamicDataPlaceholder))
                {
                    viewFileContent = viewFileContent.Replace(dynamicDataPlaceholder, this.ViewBag[viewBagKey]);
                }
            }

            string view = layoutFileContent.Replace("@RenderBody()", viewFileContent);

            var response = new HtmlResult(view, HttpResponseStatusCode.Ok);

            return response;
        }

        private string GetLayoutFilePath(string viewName)
        {
            string layoutFilePath;

            if (this.notLoggedPaths.Contains(viewName))
            {
                layoutFilePath = RootDirectoryRelativePath +
                ViewsFolderName +
                DirectorySeparator +
                "_LayoutNotLogged" +
                HtmlFileExtension;
            }
            else
            {
                layoutFilePath = RootDirectoryRelativePath +
                ViewsFolderName +
                DirectorySeparator +
                "_LayoutLogged" +
                HtmlFileExtension;
            }

            return layoutFilePath;
        }

        protected void SignInUser(string username, IHttpRequest request, IHttpResponse response)
        {
            request.Session.AddParameter("username", username);

            string cookieContent = this.UserCookieService.GetUserCookie(username);

            var userCookie = new HttpCookie(".auth-irunes", cookieContent);

            response.Cookies.Add(userCookie);
        }

        private string GetCurrentControllerName()
        {
            return this.GetType().Name.Replace(ControllerSuffixName, string.Empty);
        }
    }
}

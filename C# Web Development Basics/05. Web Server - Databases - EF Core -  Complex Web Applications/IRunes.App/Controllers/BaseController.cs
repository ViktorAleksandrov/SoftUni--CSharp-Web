using System.Collections.Generic;
using System.IO;
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

            string fileContent = File.ReadAllText(filePath);

            foreach (string viewBagKey in this.ViewBag.Keys)
            {
                string dynamicDataPlaceholder = $"{{{{{viewBagKey}}}}}";

                if (fileContent.Contains(dynamicDataPlaceholder))
                {
                    fileContent = fileContent.Replace(dynamicDataPlaceholder, this.ViewBag[viewBagKey]);
                }
            }

            var response = new HtmlResult(fileContent, HttpResponseStatusCode.Ok);

            return response;
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

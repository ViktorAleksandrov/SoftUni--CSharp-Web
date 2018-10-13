using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;

namespace IRunes.App.Controllers
{
    public class HomeController : BaseController
    {
        public IHttpResponse Index(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return this.View();
            }

            string username = request.Session.GetParameter("username").ToString();

            this.ViewBag["username"] = username;

            return this.View("IndexLoggedIn");
        }
    }
}

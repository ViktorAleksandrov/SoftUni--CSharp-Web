using System.Linq;
using IRunes.Models;
using IRunes.Services;
using IRunes.Services.Contracts;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Results;

namespace IRunes.App.Controllers
{
    public class UsersController : BaseController
    {
        private IHashService hashService;

        public UsersController()
            : base()
        {
            this.hashService = new HashService();
        }

        public IHttpResponse Register(IHttpRequest request)
        {
            return this.View();
        }

        public IHttpResponse PostRegister(IHttpRequest request)
        {
            string username = request.FormData["username"].ToString().Trim();
            string password = request.FormData["password"].ToString();
            string confirmPassword = request.FormData["confirmPassword"].ToString();
            string email = request.FormData["email"].ToString();

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword) ||
                string.IsNullOrWhiteSpace(email) ||
                password != confirmPassword ||
                this.Db.Users.Any(u => u.Username == username))
            {
                return new RedirectResult("/Users/Register");
            }

            string hashedPassword = this.hashService.Hash(password);

            var user = new User
            {
                Username = username,
                Password = hashedPassword,
                Email = email
            };

            this.Db.Users.Add(user);
            this.Db.SaveChanges();

            var response = new RedirectResult("/");

            this.SignInUser(username, request, response);

            return response;
        }

        public IHttpResponse Login(IHttpRequest request)
        {
            return this.View();
        }

        public IHttpResponse PostLogin(IHttpRequest request)
        {
            string usernameOrEmail = request.FormData["usernameOrEmail"].ToString().Trim();
            string password = request.FormData["password"].ToString();

            string hashedPassword = this.hashService.Hash(password);

            User user = this.Db.Users.FirstOrDefault(
                u => (u.Username == usernameOrEmail || u.Email == usernameOrEmail) && u.Password == hashedPassword);

            if (user == null)
            {
                return new RedirectResult("/Users/Login");
            }

            var response = new RedirectResult("/");

            this.SignInUser(user.Username, request, response);

            return response;
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            if (!request.Session.ContainsParameter("username"))
            {
                return new RedirectResult("/");
            }

            request.Session.ClearParameters();

            return new RedirectResult("/");
        }
    }
}

using IRunes.Extensions;
using IRunes.Models;
using IRunes.Services;
using SIS.HTTP.Cookies;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Results;
using System.Linq;

namespace IRunes.Controllers
{
    public class UserController : BaseController
    {
        private HashService hashService;

        public UserController()
        {
            this.hashService = new HashService();
        }

        public IHttpResponse Login(IHttpRequest request)
        {
            var username = this.GetUsername(request);

            if (username != null)
            {
                this.ViewBag["username"] = username;
                return this.View("Home/Index");
            }

            return this.View("User/Login");
        }   

        public IHttpResponse DoLogin(IHttpRequest request)
        {
            string username = request.FormData["username"].ToString().UrlDecode();
            string password = request.FormData["password"].ToString().UrlDecode();

            var hashedPassword = this.hashService.Hash(password);

            var user = this.context.Users.FirstOrDefault(u => u.Username == username && u.Password == hashedPassword);

            if (user == null)
            {
                return this.View("User/Login");
            }

            this.IsUserAuthenticated = true;

            request.Session.AddParameter("username", username);

            var userCookieValue = this.userCookieService.GetUserCookie(username);

            this.ViewBag["username"] = username;

            var response = this.View("Home/Index");

            response.Cookies.Add(new HttpCookie("_auth", userCookieValue));

            return response;
        }

        public IHttpResponse Register(IHttpRequest request)
        {
            return this.View("User/Register");
        }

        public IHttpResponse DoRegister(IHttpRequest request)
        {
            string username = request.FormData["username"].ToString().UrlDecode();
            string password = request.FormData["password"].ToString().UrlDecode();
            string confirmPassword = request.FormData["confirm-password"].ToString().UrlDecode();
            string email = request.FormData["email"].ToString().UrlDecode();

            if (this.context.Users.Any(u => u.Username == username))
            {
                return this.View("User/Register");
            }

            if (password != confirmPassword)
            {
                return this.View("User/Register");
            }

            if (username.Contains("@"))
            {
                return this.View("User/Register");
            }

            if (!email.Contains("@"))
            {
                return this.View("User/Register");
            }

            string hashedPassword = this.hashService.Hash(password);

            User user = new User
            {
                Username = username,
                Password = hashedPassword,
                Email = email
            };

            this.context.Users.Add(user);
            this.context.SaveChanges();

            return this.View("User/Login");
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            var username = this.GetUsername(request);

            if (username == null)
            {
                return this.View("Home/Index");
            }

            var response = new RedirectResult("/");

            var cookie = request.Cookies.GetCookie("_auth");

            cookie.Delete();

            response.Cookies.Add(cookie);

            return response;
        }
    }
}
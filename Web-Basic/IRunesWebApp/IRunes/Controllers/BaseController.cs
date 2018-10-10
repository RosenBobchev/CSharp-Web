using IRunes.Data;
using IRunes.Services;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Results;
using System.Collections.Generic;
using System.IO;

namespace IRunes.Controllers
{
    public abstract class BaseController
    {
        protected IRunesDbContext context;

        private const string Root = "../../../Views/";
        private const string FileExtention = ".html";
        private const string Layout = "_Layout";

        protected BaseController()
        {
            this.context = new IRunesDbContext();
            this.userCookieService = new UserCookieService();
            this.ViewBag = new Dictionary<string, string>();
        }

        public Dictionary<string, string> ViewBag { get; set; }

        public UserCookieService userCookieService { get; }

        protected bool IsUserAuthenticated { get; set; } = false;

        protected IHttpResponse View(string viewName)
        {
            string bodyContent = ViewFactory(viewName);

            this.SetViewBagParameters(bodyContent);

            string fullViewContent = ViewFactory(Layout);

            return new HtmlResult(HttpResponseStatusCode.Ok, fullViewContent);
        }

        private void SetViewBagParameters(string bodyContent)
        {
            this.ViewBag["body"] = bodyContent;
            this.ViewBag["NonAuthenticated"] = "";
            this.ViewBag["Authenticated"] = "";

            if (IsUserAuthenticated)
            {
                this.ViewBag["NonAuthenticated"] = "d-none";
            }
            else
            {
                this.ViewBag["Authenticated"] = "d-none";
            }
        }

        protected string ViewFactory(string viewName)
        {
            var content = File.ReadAllText(Root + viewName + FileExtention);

            foreach (var viewBagKey in ViewBag.Keys)
            {
                string placeHolder = $"{{{{{viewBagKey}}}}}";

                if (content.Contains(viewBagKey))
                {
                    content = content.Replace(placeHolder, this.ViewBag[viewBagKey]);
                }
            }

            return content;
        }

        protected string GetUsername(IHttpRequest request)
        {
            if (!request.Cookies.ContainsCookie("_auth"))
            {
                return null;
            }

            this.IsUserAuthenticated = true;

            var cookie = request.Cookies.GetCookie("_auth");
            var cookieContent = cookie.Value;
            var userName = this.userCookieService.GetUserData(cookieContent);
            return userName;
        }
    }
}
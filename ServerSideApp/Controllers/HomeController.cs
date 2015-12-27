using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using ServerSideApp.Helpers;

namespace ServerSideApp.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index() {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Request.Cookies[LanguageHelper.LANGUAGECOOKIE]?.Value ?? LanguageHelper.DefaultLanguage);

            return View();
        }

        public ActionResult About() {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
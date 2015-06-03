using Carcassonne_Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Carcassonne_Web.Controllers
{
    public class LanguageController : Controller
    {
        public ActionResult SetLanguage(String name)
        {
            
            return RedirectToRoute(new{
                culture = name,
                controller = "Home",
                action = "Index"
            });
        }

    }
}

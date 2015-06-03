﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Carcassonne_Web.Helpers
{
    public class LocalizationRedirectRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            
           var routeValues = requestContext.RouteData.Values;

            var cookieLocale = requestContext.HttpContext.Request.Cookies["locale"];
            if (cookieLocale != null)
            {
                routeValues["culture"] = cookieLocale.Value;
                return new RedirectHandler(new UrlHelper(requestContext).RouteUrl(routeValues));
            }

            var uiCulture = CultureInfo.CurrentUICulture;
            routeValues["culture"] = uiCulture.Name;
            return new RedirectHandler(new UrlHelper(requestContext).RouteUrl(routeValues));
        }
    }
}
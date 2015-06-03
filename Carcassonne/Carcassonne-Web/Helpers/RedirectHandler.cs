using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;

namespace Carcassonne_Web.Helpers
{
    public class RedirectHandler : IHttpHandler
    {
        private string _newUrl;

        [SuppressMessage(category: "Microsoft.Design", checkId: "CA1054:UriParametersShouldNotBeStrings",
            Justification = "We just use string since HttpResponse.Redirect only accept as string parameter.")]
        public RedirectHandler(string newUrl)
        {
            this._newUrl = newUrl;
        }

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Redirect(this._newUrl);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebApi.Server
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{action}/{fileName}"
            );
        }
    }
}

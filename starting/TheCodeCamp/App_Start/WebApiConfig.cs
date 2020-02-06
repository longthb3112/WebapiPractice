using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace TheCodeCamp
{
  public static class WebApiConfig
  {
    public static void Register(HttpConfiguration config)
    {
      // Web API configuration and services
      AutofacConfig.Register();

     //Web API versioning
     config.AddApiVersioning(cfg => {
         cfg.DefaultApiVersion = new Microsoft.Web.Http.ApiVersion(1, 0);
         cfg.AssumeDefaultVersionWhenUnspecified = true;
         cfg.ReportApiVersions = true;

            });
      // Web API routes
      config.MapHttpAttributeRoutes();

      //config.Routes.MapHttpRoute(
      //    name: "DefaultApi",
      //    routeTemplate: "api/{controller}/{id}",
      //    defaults: new { id = RouteParameter.Optional }
      //);
    }
  }
}

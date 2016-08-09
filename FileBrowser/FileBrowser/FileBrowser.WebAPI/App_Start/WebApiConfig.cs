using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

using Newtonsoft.Json.Serialization;


namespace FileBrowser.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes       
            config.MapHttpAttributeRoutes();


            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
              new CamelCasePropertyNamesContractResolver();

            config.EnableCors();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}

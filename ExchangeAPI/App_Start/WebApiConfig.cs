using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ExchangeAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
         name: "ExchangeRateAPI",
            routeTemplate: "api/ExchangeRateAPI/GetAPI/{dates}/{baseCurrency}/{targetCurrency}",
            defaults: new { controller = "ExchangeRateAPI", action = "GetAPI", baseCurrency = RouteParameter.Optional, targetCurrency = RouteParameter.Optional, dates = RouteParameter.Optional });

            //Test
           // config.Routes.MapHttpRoute(
           //name: "ExchangeRateAPI",
           //   routeTemplate: "api/ExchangeRateAPI/GetExchangeAPI/{dates}/{baseCurrency}/{targetCurrency}",
           //   defaults: new { controller = "ExchangeRateAPI", action = "GetGetExchangeAPI", baseCurrency = RouteParameter.Optional, targetCurrency = RouteParameter.Optional, dates = RouteParameter.Optional });

        }
    }
}

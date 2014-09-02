using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcAuction
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //Creates a new IgnoreRoute which prevents access to urls with "php-app"
            routes.IgnoreRoute("php-app/{*pathInfo*}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                //This statment adds a constrant which requires that all ID parameters be numerical values 
                /*
                    constraints: new { id = @"\d+" }
                */
            );

            /*This is an example map route 
            routes.MapRoute(
                "Blog Posts",
                //this url is a literal value followed by year, month, and ID
                "posts/{year}/{month}/{id}",
                //this statement sets the default url
                new { controller = "Blog", action = "Post", id = UrlParameter.Optional },
                //This statment contrains the year and month placeholders to have 4 digits and 2 digits respectively
                new {year = @"\d{4}", month = @"\d{2}"}
            );
            */
        }
    }
}
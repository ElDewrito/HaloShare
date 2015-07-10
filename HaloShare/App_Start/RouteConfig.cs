using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Routing;
using System.Web.Routing;

namespace HaloShare
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.LowercaseUrls = true;

            var constraintsResolver = new DefaultInlineConstraintResolver();
            constraintsResolver.ConstraintMap.Add("slug", typeof(SlugRouteConstraint));

            routes.MapMvcAttributeRoutes(constraintsResolver);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "HaloShare.Controllers" }
            );
        }
    }

    public class SlugRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            string value = values[parameterName].ToString();
            string[] slugSplit = value.Split(new[] { '-' }, 2, StringSplitOptions.RemoveEmptyEntries);

            int id;
            if (int.TryParse(slugSplit[0], out id))
            {
                values[parameterName] = id;
                return true;
            }
            return false;
        }
    }
}

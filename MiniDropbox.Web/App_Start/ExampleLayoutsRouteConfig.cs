using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BootstrapMvcSample.Controllers;
using MiniDropbox.Web.Controllers;
using NavigationRoutes;

namespace BootstrapMvcSample
{
    public class ExampleLayoutsRouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapNavigationRoute<AccountController>("Inicio", c => c.LogIn());

            routes.MapNavigationRoute<ExampleLayoutsController>("Perfil", c => c.Starter())
                  .AddChildRoute<DiskController>("Lista Archivos", c => c.ListAllContent())
                  .AddChildRoute<AccountController>("Editar Perfil", c => c.UpdateProfile())
                  .AddChildRoute<AccountController>("Referral a friend", c => c.Referral())
                ;
        }
    }
}

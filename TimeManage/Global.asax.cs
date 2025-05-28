using System;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TimeManage.Models;

namespace TimeManage
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Add these lines to enable EF DB auto-creation:
            Database.SetInitializer(new CreateDatabaseIfNotExists<ApplicationDbContext>());

            using (var context = new ApplicationDbContext())
            {
                context.Database.Initialize(force: false);
            }
        }
    }
}

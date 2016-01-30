using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using lionsEventManagement.Models;

namespace lionsEventManagement
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Always recreate the database if the model changes
            // TODO: Remove when database model is done
            //Database.SetInitializer<ApplicationDbContext>(new DropCreateDatabaseAlways<ApplicationDbContext>());
            Database.SetInitializer<EventManagementDbContext>(new DropCreateDatabaseIfModelChanges<EventManagementDbContext>());
            //Database.SetInitializer<EventManagementDbContext>(new DropCreateDatabaseAlways<EventManagementDbContext>());
        }
    }
}

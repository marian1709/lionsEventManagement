using lionsEventManagement.Models;

namespace lionsEventManagement.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "lionsEventManagement.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            this.AddUserAndRoles();
        }

        bool AddUserAndRoles()
        {
            bool success = false;

            var idManager = new IdentityManager();
            success = idManager.CreateRole("Administrator");
            if (!success) return success;

            success = idManager.CreateRole("Manager");
            if (!success) return success;


            var newUser = new ApplicationUser()
            {
                UserName = "marian.schiemann",
                FirstName = "Marian",
                LastName = "Schiemann",
                Email = "schiemann@dajama-dev.de"
            };

            // Be careful here - you  will need to use a password which will 
            // be valid under the password rules for the application, 
            // or the process will abort:
            success = idManager.CreateUser(newUser, "mvcm28837625S*");
            if (!success) return success;

            success = idManager.AddUserToRole(newUser.Id, "Administrator");
            if (!success) return success;

            return success;
        }
    }
}

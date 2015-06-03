using System.Diagnostics;

namespace Carcassonne_Web.Migrations
{
    using Carcassonne_Web.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Carcassonne_Web.DAL.CarcassonneContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DAL.CarcassonneContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            if (!(context.Users.Any(u => u.UserName == "Admin")))
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var userToInsert = new ApplicationUser { UserName = "Admin", EmailConfirmed = true, Avatar = "/Content/images/p_pic6.png" };
                var result = userManager.Create(userToInsert, "Admin2015!");
                if (result.Succeeded)
                {

                    var rolestore = new RoleStore<IdentityRole>(context);
                    var roleManager = new RoleManager<IdentityRole>(rolestore);
                    var rol = roleManager.FindByName("Admin");
                    if (rol == null)
                    {
                        roleManager.Create(new ApplicationRole("Admin"));
                    }
                    userManager.AddToRoleAsync(userToInsert.Id, "Admin").Wait();
                }
                else
                {
                    Debug.WriteLine(result.Errors.FirstOrDefault().ToString());
                }
            }
            //context.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0; update applicationusers set Avatar = concat('/Content/images/p_pic',FLOOR(RAND() * 8) + 1,'.png');");
        }
    }
}

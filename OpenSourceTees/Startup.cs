using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using OpenSourceTees.Models;
using Owin;

[assembly: OwinStartupAttribute(typeof(OpenSourceTees.Startup))]
namespace OpenSourceTees
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //createRoles();
        }

    //    private void createRoles()
    //    {
    //        ApplicationDbContext context = new ApplicationDbContext();
    //        var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
    //        if (!roleManager.RoleExists("Visitor"))
    //        {
    //            var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
    //            role.Name = "Visitor";
    //            roleManager.Create(role);
    //        }
    //        if (!roleManager.RoleExists("User"))
    //        {
    //            var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
    //            role.Name = "User";
    //            roleManager.Create(role);
    //        }
    //        if (!roleManager.RoleExists("Creator"))
    //        {
    //            var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
    //            role.Name = "Creator";
    //            roleManager.Create(role);
    //        }
    //        if (!roleManager.RoleExists("Admin"))
    //        {
    //            var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
    //            role.Name = "Admin";
    //            roleManager.Create(role);
    //        }
    //    }
    }
}

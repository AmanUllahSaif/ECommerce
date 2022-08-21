using ECommerce.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.UI.Util
{
    public class AppStartUpManager
    {
        public static void RegisterRoles()
        {
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Bussines"))
            {
                var role = new IdentityRole();
                role.Name = "Bussines";
                roleManager.Create(role);
            }
        }
    }
}
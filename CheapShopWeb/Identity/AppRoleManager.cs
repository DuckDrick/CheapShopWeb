using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CheapShopWeb.DataContext;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace CheapShopWeb
{
    public class AppRoleManager : RoleManager<IdentityRole>
    {
        public AppRoleManager(IRoleStore<IdentityRole, string> store) : base(store)
        {
            // if (!this.RoleExists(UserRoles.NORMAL))
            // {
            //     this.Create(new IdentityRole(UserRoles.NORMAL));
            // }
            // if (!this.RoleExists(UserRoles.TRUSTED))
            // {
            //     this.Create(new IdentityRole(UserRoles.TRUSTED));
            // }
            // if (!this.RoleExists(UserRoles.ADMIN))
            // {
            //     this.Create(new IdentityRole(UserRoles.ADMIN));
            // }
        }

        public static AppRoleManager Create(IdentityFactoryOptions<AppRoleManager> options,
            IOwinContext context)
        {
            var appRoleManager = new AppRoleManager(new RoleStore<IdentityRole>(context.Get<MyDbContext>()));
            return appRoleManager;
        }
    }
}
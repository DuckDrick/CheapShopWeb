using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CheapShopWeb;
using Microsoft.AspNet.Identity.Owin;

namespace CheapShopWeb.Controllers
{
    public class BaseController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private AppUserManager _userManager;
        private AppRoleManager _appRoleManager;
        public BaseController()
        {
        }

        public BaseController(AppUserManager userManager, ApplicationSignInManager signInManager, AppRoleManager appRoleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = appRoleManager;
        }

        public AppRoleManager RoleManager
        {
            get => _appRoleManager ?? HttpContext.GetOwinContext().GetUserManager<AppRoleManager>();
            private set => _appRoleManager = value;
        }

        public AppUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            private set => _userManager = value;
        }

        public ApplicationSignInManager SignInManager
        {
            get => _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            private set => _signInManager = value;
        }
    }
}
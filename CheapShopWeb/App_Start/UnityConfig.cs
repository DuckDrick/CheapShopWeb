using System;
using System.Web.Mvc;
using CheapShopWeb.Controllers;
using CheapShopWeb.Scrapers;
using Unity;
using Unity.Injection;
using Unity.Mvc5;

namespace CheapShopWeb
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        public static UnityContainer Container = new UnityContainer();
        public static void RegisterComponents()
        {
            Container.RegisterSingleton<ScraperService>();

            Container.RegisterType<AccountController>(new InjectionConstructor());

            DependencyResolver.SetResolver(new UnityDependencyResolver(Container));
        }
    }
}
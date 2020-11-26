using System.Web.Mvc;
using CheapShopWeb.Scrapers;
using Unity;
using Unity.Mvc5;

namespace CheapShopWeb
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterSingleton<ScraperService>();
         
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
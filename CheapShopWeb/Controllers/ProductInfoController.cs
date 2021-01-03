using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CheapShopWeb.DataContext;
using CheapShopWeb.Models;
using CheapShopWeb.Services;
using PagedList;

namespace CheapShopWeb.Controllers
{
    public class ProductInfoController : Controller
    {
        private List<Product> filtered = new List<Product>();

        // GET: ProductInfo
        public async Task<ActionResult> ProductInfo(string name, string link, string photo, string price, string source, string group, string searchString)
        {
            filtered = await ApiService.GetSimilarProducts(name, price, source, group, searchString);
            return View(filtered);
        }
        public async Task<ActionResult> ProductInfoGroup(string name, string link, string photo, string price, string source, string group, string itemsGroup)
        {
            filtered = await ApiService.GetSimilarGroup(name, link, photo, price, source, itemsGroup);
            return View(filtered);
        }
    }
}
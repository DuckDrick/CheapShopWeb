using System;
using System.Collections.Generic;
using System.Linq;
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
        private ProductDbContext db = new ProductDbContext();
        // GET: ProductInfo
        public ActionResult ProductInfo(String name, String link, String photo, String price, String source, String group)
        {
            
            return View(Filtering.GetSimilarProducts(new Product(name, source, price, photo, link, group)));
        }
    }
}
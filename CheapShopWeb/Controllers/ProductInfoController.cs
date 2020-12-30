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
        private MyDbContext db;

        public ProductInfoController(MyDbContext myDbContext)
        {
            this.db = myDbContext;
        }
        // GET: ProductInfo
        public ActionResult ProductInfo(String name, String link, String photo, String price, String source, String group, String searchString)
        {
            
            return View(Filtering.GetSimilarProducts(db.Products.ToList(), new Product(name, source, price, photo, link, group), searchString));
        }
        public ActionResult ProductInfoGroup(String name, String link, String photo, String price, String source, String group, string itemsGroup)
        {

            return View(Filtering.GetSimilarProductsGroup(db.Products.ToList(), new Product(name, source, price, photo, link, group), itemsGroup));
        }
    }
}
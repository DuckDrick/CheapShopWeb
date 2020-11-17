using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CheapShopWeb.DataContext;
using CheapShopWeb.Models;
using CheapShopWeb.Services;

namespace CheapShopWeb.Controllers
{
    public class ProductInfoController : Controller
    {
        private ProductDbContext db = new ProductDbContext();
        // GET: ProductInfo
        public ActionResult ProductInfo(Product product)
        {
            return View((Filtering.GetSimilarProducts(db.Products.ToList(), product)));
        }
    }
}
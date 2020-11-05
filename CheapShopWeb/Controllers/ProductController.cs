using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CheapShopWeb.DataContext;

namespace CheapShopWeb.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        private ProductDbContext _db;

        public ProductController()
        {
            _db = new ProductDbContext();
        }

        public ActionResult Product()
        {
            var products = _db.Products.ToArray();
            return View(products);
        }
    }
}
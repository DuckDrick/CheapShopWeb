using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CheapShopWeb.DataContext;

namespace CheapShopWeb.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        private ProductDbContext _db;

        public SearchController()
        {
            _db = new ProductDbContext();
        }

        public ActionResult Search()
        {
            var products = _db.Products.ToArray();
            return View(products);
        }
    }
}
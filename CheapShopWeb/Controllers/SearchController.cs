using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CheapShopWeb.DataContext;
using Microsoft.SqlServer.Server;

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
        public ActionResult Search(String searchString)
        {
            var products = _db.Search(searchString);
            return View(products);
        }
    }
}
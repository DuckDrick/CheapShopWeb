using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CheapShopWeb.DataContext;
using CheapShopWeb.Models;
using CheapShopWeb.Services;
using Microsoft.SqlServer.Server;

namespace CheapShopWeb.Controllers
{
    public class SearchController : Controller
    {
  
        private ProductDbContext db = new ProductDbContext();
        // GET: Search
        public ActionResult Search(string search, string priceFrom, string priceTo, string group, string source)
        {
            return View(Filtering.Filter(db.Products.ToList(), search, priceFrom, priceTo, group, source));
        }
    }
}
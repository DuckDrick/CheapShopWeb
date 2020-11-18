using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CheapShopWeb.DataContext;
using CheapShopWeb.Models;
using CheapShopWeb.Services;
using Microsoft.SqlServer.Server;
using PagedList;

namespace CheapShopWeb.Controllers
{
    public class SearchController : Controller
    {
  
        private ProductDbContext db = new ProductDbContext();
        // GET: Search
        public ActionResult Search(string search, string priceFrom, string priceTo, string group, string source, int? page)
        {
            ViewBag.search = search;
            ViewBag.priceFrom = priceFrom;
            ViewBag.priceTo = priceTo;
            ViewBag.group = group;
            ViewBag.source = source;

            var filtered = Filtering.Filter(db.Products.ToList(), search, priceFrom, priceTo, group, source);
            var pageSize = 20;
            var pageNumber = (page ?? 1);
            return View(filtered.ToPagedList(pageNumber, pageSize));
      
        }
    }
}
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
            //if (!string.IsNullOrEmpty(search)) //gali buti spaudziamas search mygtukas
            //var products = _db.Search(searchString);
            return View(Filtering.Filter(db.Products.ToList(), search, priceFrom, priceTo, group, source));
            //kitu atveju spaudziamas filtravimas //doesnt work?
            /*{

                ps = new ProductAndSearchString
                {
                    list = _db.Search(searchString),
                    searchString = searchString
                };
                return View(ps);
            }*/
        }
    }
}
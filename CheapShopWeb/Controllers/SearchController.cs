using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CheapShopWeb.DataContext;
using CheapShopWeb.Models;
using Microsoft.SqlServer.Server;

namespace CheapShopWeb.Controllers
{
    public class SearchController : Controller
    {
        private ProductAndSearchString ps;
        // GET: Search

        public ActionResult Search(String searchString, String priceFrom, String priceTo)
        {
            //if (!string.IsNullOrEmpty(search)) //gali buti spaudziamas search mygtukas
            //var products = _db.Search(searchString);
            ps = new ProductAndSearchString();
            ps.list = ps.Search(searchString);
            ps.searchString = searchString;
            return View(ps);  
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
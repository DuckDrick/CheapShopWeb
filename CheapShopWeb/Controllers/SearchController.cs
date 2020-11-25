using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CheapShopWeb.DataContext;
using CheapShopWeb.Models;
using CheapShopWeb.Repository;
using CheapShopWeb.Scrapers;
using CheapShopWeb.Services;
using Microsoft.SqlServer.Server;
using PagedList;

namespace CheapShopWeb.Controllers
{
    public class SearchController : Controller
    {

        private readonly ProductDbContext productDbContext;

        private readonly ScraperService scraperRepository;


        public SearchController(ScraperService scraperRepository, ProductDbContext productDbContext)
        {
            this.scraperRepository = scraperRepository;
            this.productDbContext = productDbContext;
        }

        // GET: Search
        public ActionResult Search(string search, string priceFrom, string priceTo, string group, string source, int? page)
        {
            ViewBag.search = search;
            ViewBag.priceFrom = priceFrom;
            ViewBag.priceTo = priceTo;
            ViewBag.group = group;
            ViewBag.source = source;

            var filtered = Filtering.Filter(productDbContext.Products.ToList(), search, priceFrom, priceTo, group, source);
            var pageSize = 20;
            var pageNumber = (page ?? 1);
            return View(filtered.ToPagedList(pageNumber, pageSize));
      
        }

        [HttpPost]
        public void Scrape(string query)
        {
            scraperRepository.queryList.Add(query);
        }
    }
}
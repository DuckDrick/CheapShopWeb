using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using CheapShopWeb.DataContext;
using CheapShopWeb.Scrapers;
using CheapShopWeb.Services;
using PagedList;

namespace CheapShopWeb.Controllers
{
    public class SearchController : Controller
    {
        private readonly Lazy<ProductDbContext> _productDbContext;
        private readonly Lazy<ScraperService> _scraperService;


        public SearchController(Lazy<ScraperService> scraperService, Lazy<ProductDbContext> productDbContext)
        {
            this._scraperService = scraperService;
            this._productDbContext = productDbContext;
        }

        // GET: Search
        public ActionResult Search(string search, string priceFrom, string priceTo, string group, string source, int? page)
        {
            ViewBag.search = search;
            ViewBag.priceFrom = priceFrom;
            ViewBag.priceTo = priceTo;
            ViewBag.group = group;
            ViewBag.source = source;

            var filtered = Filtering.Filter(_productDbContext.Value.Products.ToList(), search, priceFrom, priceTo, group, source);
            var pageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"] ?? "3");
            var counts = Filtering.CountAmounts(filtered);
            ViewBag.siteCounts = counts.Item1;
            ViewBag.groupCounts = counts.Item2;
            var pageNumber = (page ?? 1);
            return View(filtered.ToPagedList(pageNumber, pageSize));
      
        }


        [HttpPost]
        public void Scrape(string query, string source)
        {
            _scraperService.Value.queryList.Add(query);
        }

        public ActionResult SearchGroup(string group, int? page)
        {
            var filtered = Filtering.Filter(_productDbContext.Value.Products.ToList(), null, null, null, group, null);
            var pageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"] ?? "3");
            var counts = Filtering.CountAmounts(filtered);
            var pageNumber = (page ?? 1);
            return View(filtered.ToPagedList(pageNumber, pageSize));

        }
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using CheapShopWeb.ApiControllers;
using CheapShopWeb.DataContext;
using CheapShopWeb.Scrapers;
using CheapShopWeb.Services;
using Newtonsoft.Json;
using PagedList;
using CheapShopWeb.Models;
using Product = CheapShopWeb.Models.Product;

namespace CheapShopWeb.Controllers
{
    public class SearchController : Controller
    {
        private readonly Lazy<ScraperService> _scraperService;
        private List<Product> _filtered = new List<Product>();

        public SearchController(Lazy<ScraperService> scraperService)
        {
            this._scraperService = scraperService;
        }

        public async Task<ActionResult> Search(string search, string priceFrom, string priceTo, string group, string source, int? page)
        {
            ViewBag.search = search;
            ViewBag.priceFrom = priceFrom;
            ViewBag.priceTo = priceTo;
            ViewBag.group = group;
            ViewBag.source = source;
            var pageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"] ?? "3");
            var pageNumber = (page ?? 1);
            _filtered = await ApiService.GetProductsForViewAll(search, priceFrom,priceTo,group,source);
            var counts = Filtering.CountAmounts(_filtered);
            ViewBag.siteCounts = counts.Item1;
            ViewBag.groupCounts = counts.Item2;
            return View(_filtered.ToPagedList(pageNumber, pageSize));
        }


        [HttpPost]
        public void Scrape(string query, string source)
        {
            _scraperService.Value.queryList.Add(query);
        }

        public async Task<ActionResult> SearchGroup(string group, int? page)
        {
            var pageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"] ?? "3");
            var pageNumber = (page ?? 1);
            _filtered = await ApiService.GetProductsForViewGroup(group);
            return View(_filtered.ToPagedList(pageNumber, pageSize));
        }
    }
}
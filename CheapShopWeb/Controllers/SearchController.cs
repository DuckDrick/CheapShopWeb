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
        private readonly Lazy<ProductDbContext> _productDbContext;
        private readonly Lazy<ScraperService> _scraperService;
        private static readonly string _baseUrl = "https://localhost:44360/api/";
        private List<Models.Product> filtered = new List<Product>();

        public SearchController(Lazy<ScraperService> scraperService, Lazy<ProductDbContext> productDbContext)
        {
            this._scraperService = scraperService;
            this._productDbContext = productDbContext;
        }

        // GET: Search
        public async Task<ActionResult> Search(string search, string priceFrom, string priceTo, string group, string source, int? page)
        {
            ViewBag.search = search;
            ViewBag.priceFrom = priceFrom;
            ViewBag.priceTo = priceTo;
            ViewBag.group = group;
            ViewBag.source = source;
            var pageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"] ?? "3");
            var pageNumber = (page ?? 1);
            filtered = await ApiService.GetProductsForViewAll(search, priceFrom,priceTo,group,source);
            var counts = Filtering.CountAmounts(filtered);
            ViewBag.siteCounts = counts.Item1;
            ViewBag.groupCounts = counts.Item2;
            return View(filtered.ToPagedList(pageNumber, pageSize));
            /*
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage res = await client.GetAsync("Default/Search" + ApiService.BUrl("",search, priceFrom, priceTo, group, source));

                if (res.IsSuccessStatusCode)
                {
                    var EmpResponse = res.Content.ReadAsStringAsync().Result;
                    filtered = JsonConvert.DeserializeObject<List<Models.Product>>(EmpResponse);
                    var pageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"] ?? "3");
                    var counts = Filtering.CountAmounts(filtered);
                    ViewBag.siteCounts = counts.Item1;
                    ViewBag.groupCounts = counts.Item2;
                    var pageNumber = (page ?? 1);
                    return View(filtered.ToPagedList(pageNumber, pageSize));
                }
            }

            return View();   */
            /*ViewBag.search = search;
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
            return View(filtered.ToPagedList(pageNumber, pageSize));*/

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
            filtered = await ApiService.GetProductsForViewGroup(group);
            return View(filtered.ToPagedList(pageNumber, pageSize));
            /*using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage res = await client.GetAsync("Default/MainGroup?maingroup="+group);
                
                if (res.IsSuccessStatusCode)
                {
                    var EmpResponse = res.Content.ReadAsStringAsync().Result;
                    filtered = JsonConvert.DeserializeObject<List<Models.Product>>(EmpResponse);
                    var pageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"] ?? "3");
                    var pageNumber = (page ?? 1);
                    return View(filtered.ToPagedList(pageNumber, pageSize));
                }
            }

            return View();*/
            //returning the employee list to view  
            //return View(EmpInfo);
            //var filtered = Filtering.Filter(_productDbContext.Value.Products.ToList(), null, null, null, group, null);
            //var pageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"] ?? "3");
            //var pageNumber = (page ?? 1);
            //return View(filtered.ToPagedList(pageNumber, pageSize));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using CheapShopWeb.DataContext;
using CheapShopWeb.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace CheapShopWeb.Services
{
    public class ApiService
    {
        //private readonly Lazy<ProductDbContext> _productDbContext;
        private static readonly string _baseUrl = "https://localhost:44360/api/";
        private static List<Models.Product> filtered = new List<Product>();
        public static string BUrl(string url, string search, string priceFrom, string priceTo, string group, string source)
        {
            if (!search.IsNullOrWhiteSpace())
            {
                url += "?search=" + search;
            }

            if (!priceFrom.IsNullOrWhiteSpace())
            {
                url += "&priceFrom=" + priceFrom;
            }
            if (!priceTo.IsNullOrWhiteSpace())
            {
                url += "&priceTo=" + priceTo;
            }
            if (!source.IsNullOrWhiteSpace())
            {
                url += "&source=" + source;
            }
            if (!group.IsNullOrWhiteSpace())
            {
                url += "&group="+group;
            }

            return url;
        }

        public static async Task<List<Product>> GetProductsForViewGroup(string group)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage res = await client.GetAsync("Default/MainGroup?maingroup=" + group);

                if (res.IsSuccessStatusCode)
                {
                    var EmpResponse = res.Content.ReadAsStringAsync().Result;
                    filtered = JsonConvert.DeserializeObject<List<Models.Product>>(EmpResponse);
                    return filtered;
                }

                return (filtered);
            }
            
        }

        public static async Task<List<Product>> GetProductsForViewAll(string search, string priceFrom, string priceTo,
            string group, string source)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage res = await client.GetAsync("Default/Search" + BUrl("", search, priceFrom, priceTo, group, source));

                if (res.IsSuccessStatusCode)
                {
                    var results = res.Content.ReadAsStringAsync().Result;
                    filtered = JsonConvert.DeserializeObject<List<Models.Product>>(results);
                    return filtered;
                }

                return (filtered);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Principal;
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
        private static List<ProductResponse> filtered = new List<ProductResponse>();
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

        public static async Task<List<ProductResponse>> GetProductsForViewGroup(string group)
        {
            var s="Default/MainGroup?maingroup=" + group;
            return (await ClientResponse(s));
        }

        public static async Task<List<ProductResponse>> GetProductsForViewAll(string search, string priceFrom, string priceTo,
            string group, string source, IPrincipal user)
        {
            var s="Default/Search" + BUrl("", search, priceFrom, priceTo, group, source);

                return (await ClientResponse(s, user));
        }

        public static async Task<List<ProductResponse>> GetSimilarProducts(string name, string price,
            string source, string group, string searchString)
        {
            var s="Default/SimilarProducts?name=" + name+ "&price=" + price + "&source=" + source + "&group=" + group+  "&searchString="+searchString;
            return (await ClientResponse(s));
            
        }
        public static async Task<List<ProductResponse>> GetSimilarGroup(string name, string link, string photo, string price, string source, string itemsGroup)
        {
            var s ="Default/GroupItems?gname=" + name + "&gprice=" + price + "&gsource=" + source + "&gitemsGroup=" + itemsGroup;
            return (await ClientResponse(s));
        }

        public static async Task<List<ProductResponse>> ClientResponse(string uri, IPrincipal user = null)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                //client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage res = await client.GetAsync(uri);

                if (res.IsSuccessStatusCode)
                {
                    var results = res.Content.ReadAsStringAsync().Result;
                    filtered = JsonConvert.DeserializeObject<List<ProductResponse>>(results);
                    return filtered;
                }

                return (filtered);
            }
        }
    }
}
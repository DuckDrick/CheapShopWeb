using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CheapShopWeb.DataContext;
using CheapShopWeb.Models;
using CheapShopWeb.Services;
using PagedList;

namespace CheapShopWeb.ApiControllers
{
    //[Route("api/Default/")]
    public class DefaultController : ApiController
    {

        private readonly MyDbContext _productDbContext = new MyDbContext();
    // GET: api/Default/MainGroup?maingroup={group}

    //example https://localhost:44360/api/Default/MainGroup?maingroup=Kids

    [HttpGet]
        public List<Product> MainGroup(string maingroup)
        {
            var filtered = Filtering.Filter(_productDbContext.Products.ToList(), null, null, null, maingroup, null);
            return filtered;
            
        }

        // GET: api/Default/Search?search={search}&priceFrom={priceFrom}....

        // example https://localhost:44360/api/Default/search?search=xbox%20one&source=Rde&group=Computer  
        [HttpGet]
        public List<Product> Search(string search, string priceFrom=null,  string priceTo=null,string group=null, string source=null)
        {
            var filtered = Filtering.Filter(_productDbContext.Products.ToList(), search, priceFrom, priceTo, group, source); //_productDbContext.Value.Products.ToList()
            return filtered;
            
        }

        [HttpGet]
        public List<Product> SimilarProducts(string name , string price , string source, string group , string searchString)
        {
            var filtered = Filtering.GetSimilarProducts(_productDbContext.Products.ToList(), new Product(name, source, price, null, null, group), searchString);
            return filtered;

        }

        [HttpGet]
        public List<Product> GroupItems(string gname=null, string gprice = null, string gsource = null, string gitemsGroup = null)
        {
            var filtered = Filtering.GetSimilarProductsGroup(_productDbContext.Products.ToList(), gname, gsource, gprice, gitemsGroup);
            return filtered;
       
        }

        // GET: api/Default/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/Default
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Default/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Default/5
        public void Delete(int id)
        {
        }
    }
}

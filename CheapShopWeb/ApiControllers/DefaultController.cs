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
using Microsoft.AspNet.Identity;
using PagedList;

namespace CheapShopWeb.ApiControllers
{
    //[Route("api/Default/")]
    public class DefaultController : ApiController
    {

        private readonly Lazy<MyDbContext> _productDbContext = new Lazy<MyDbContext>();
        // GET: api/Default/MainGroup?maingroup={group}

        //example https://localhost:44360/api/Default/MainGroup?maingroup=Kids

        [HttpGet]
        public List<ProductResponse> MainGroup(string maingroup)
        {
            var filtered = Filtering.Filter(_productDbContext.Value.Products.ToList(), null, null, null, maingroup, null);
            var response = new List<ProductResponse>();
            if (User.Identity.IsAuthenticated)
            {
                var user = _productDbContext.Value.Users.Find(User.Identity.GetUserId());
                response.AddRange(filtered.ConvertAll(p => new ProductResponse(p, user.Likes.Contains(p))));
            }
            else
            {
                response.AddRange(filtered.ConvertAll(p => new ProductResponse(p, false)));
            }
            return response;

        }

        // GET: api/Default/Search?search={search}&priceFrom={priceFrom}....

        // example https://localhost:44360/api/Default/search?search=xbox%20one&source=Rde&group=Computer  
        [HttpGet]
        public List<ProductResponse> Search(string search, string priceFrom=null,  string priceTo=null,string group=null, string source=null)
        {
            var filtered = Filtering.Filter(_productDbContext.Value.Products.ToList(), search, priceFrom, priceTo, group, source);
            var response = new List<ProductResponse>();
            if (User.Identity.IsAuthenticated)
            {
                var user = _productDbContext.Value.Users.Find(User.Identity.GetUserId());
                response.AddRange(filtered.ConvertAll(p => new ProductResponse(p, user.Likes.Contains(p))));
            }
            else
            {
                response.AddRange(filtered.ConvertAll(p => new ProductResponse(p, false)));
            }
            return response;
            
        }

        [HttpGet]
        public List<ProductResponse> SimilarProducts(string name , string price , string source, string group , string searchString)
        {
            var filtered = Filtering.GetSimilarProducts(_productDbContext.Value.Products.ToList(), new Product(name, source, price, null, null, group), searchString);
            var response = new List<ProductResponse>();
            if (User.Identity.IsAuthenticated)
            {
                var user = _productDbContext.Value.Users.Find(User.Identity.GetUserId());
                response.AddRange(filtered.ConvertAll(p => new ProductResponse(p, user.Likes.Contains(p))));
            }
            else
            {
                response.AddRange(filtered.ConvertAll(p => new ProductResponse(p, false)));
            }
            return response;

        }

        [HttpGet]
        public List<ProductResponse> GroupItems(string gname=null, string gprice = null, string gsource = null, string gitemsGroup = null)
        {
            var filtered = Filtering.GetSimilarProductsGroup(_productDbContext.Value.Products.ToList(), gname, gsource, gprice, gitemsGroup);
            var response = new List<ProductResponse>();
            if (User.Identity.IsAuthenticated)
            {
                var user = _productDbContext.Value.Users.Find(User.Identity.GetUserId());
                response.AddRange(filtered.ConvertAll(p => new ProductResponse(p, user.Likes.Contains(p))));
            }
            else
            {
                response.AddRange(filtered.ConvertAll(p => new ProductResponse(p, false)));
            }
            return response;

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

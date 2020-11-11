using CheapShopWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CheapShopWeb.DataContext
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext() : base(nameOrConnectionString: "connection"){}
        public virtual DbSet<Product> Products { get; set; }

        public List<Product> Search(String search)
        {
            if (String.IsNullOrEmpty(search))
                return null;
            List<Product> products = new List<Product>();
            var searchl = search.ToLower().Split(' ');
            foreach (var product in Products)
            {
                foreach (var s in searchl)
                {
                    if (product.name.ToLower().Contains(s))
                    {
                        products.Add(product);
                        break;
                    }
                }
            }

            if (products.Count == 0)
                return null;
            return products;
        }

        public List<Product> Filter(String pfrom, String pto, String searchString)
        {
            List<Product> products = Search(searchString);
            var from = Convert.ToInt32(pfrom);
            var to = Convert.ToInt32(pto);
            foreach (var product in products)
            {
                if (!(Convert.ToInt32(product.price) > from && Convert.ToInt32(product.price) < to))
                {
                    products.Remove(product);
                }
            }
            return products;
        }
    }
}
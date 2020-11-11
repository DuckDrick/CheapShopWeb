using CheapShopWeb.Models;
using System;
using System.Collections.Generic;
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
            return products;
        }
    }
}
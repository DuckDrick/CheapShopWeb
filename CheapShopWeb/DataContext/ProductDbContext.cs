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
    }
}
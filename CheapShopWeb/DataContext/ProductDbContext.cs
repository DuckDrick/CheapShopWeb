﻿using CheapShopWeb.Models;
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
        public ProductDbContext() : base(nameOrConnectionString: "database"){}
        public virtual DbSet<Product> Products { get; set; }

    }
}
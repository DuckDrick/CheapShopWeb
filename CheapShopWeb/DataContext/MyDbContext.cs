using CheapShopWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CheapShopWeb.DataContext
{
    public class MyDbContext : IdentityDbContext<User>
    {
        public MyDbContext() : base(nameOrConnectionString: "database") {Configuration.ProxyCreationEnabled = false; }
        public virtual DbSet<Product> Products { get; set; }

        public static MyDbContext Create()
        {
            return new MyDbContext();
        }
    }
}
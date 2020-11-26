using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheapShopWeb.Scrapers
{
    public class Product
    {
        public Product(string name, string price, string link, string imageUrl, string group, string source)
        {
            Name = name;
            Price = price;
            Link = link;
            ImageUrl = imageUrl;
            Group = group;
            Source = source;
        }

        public string Name { get; set; }
        public string Link { get; set; }
        public string Price { get; set; }
        public string Group { get; set; }
        public string ImageUrl { get; set; }
        public string Source { get; set; }
    }
}
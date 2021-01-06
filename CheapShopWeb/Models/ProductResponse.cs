using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheapShopWeb.Models
{
    public class ProductResponse
    {
        public int id { get; set; }
        public string name { get; set; }
        public string source { get; set; }
        public string price { get; set; }
        public string photo_link { get; set; }
        public string product_link { get; set; }
        public string group { get; set; }
        public DateTime updated { get; set; }
        public bool liked { get; set; }

        public ProductResponse(Product product, bool liked)
        {
            id = product.id;
            name = product.name;
            source = product.source;
            price = product.price;
            photo_link = product.photo_link;
            product_link = product.product_link;
            group = product.group;
            updated = product.updated;
            this.liked = liked;
        }
    }
}
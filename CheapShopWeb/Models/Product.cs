using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CheapShopWeb.Models
{
    [Table("product", Schema = "public")]
    public class Product : IEnumerable
    {
        [Key]
        public string id { get; set; }
        public string name { get; set; }
        public string source { get; set; }
        public string price { get; set; }
        public string photo_link { get; set; }
        public string product_link { get; set; }
        public string group { get; set; }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
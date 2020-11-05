using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CheapShopWeb.Models
{
    [Table("novastar", Schema = "public")]
    public class Product
    {
        [Key] public string name { get; set; }
        public string item_group { get; set; }
        public string link { get; set; }
        public string plink { get; set; }
        public string price { get; set; }
    }
}
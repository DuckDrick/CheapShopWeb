using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheapShopWeb.Models
{
    public class ProductAndSearchString //norint i view grazinti keleta dalyku reikia tureti klase ir grazinti kaip vieneta, todel paieska ir filtravimas vykdomas sitoje klaseje
    {
        public List<Product> list { get; set; }
        public String searchString { get; set; }
        public ProductAndSearchString()
        {
            this.list = new List<Product>();
            this.searchString = " ";
        }
    }
}
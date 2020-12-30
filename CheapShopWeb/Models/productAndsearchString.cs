using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CheapShopWeb.DataContext;

namespace CheapShopWeb.Models
{
    public class ProductAndSearchString //norint i view grazinti keleta dalyku reikia tureti klase ir grazinti kaip vieneta, todel paieska ir filtravimas vykdomas sitoje klaseje
    {
        private MyDbContext _db;
        public List<Product> list { get; set; }
        public String searchString { get; set; }
        public ProductAndSearchString()
        {
            this.list = new List<Product>();
            this.searchString = " ";
            this._db = new MyDbContext();
        }
        public List<Product> Search(String search)
        {
            if (String.IsNullOrEmpty(search))
                return null;
            var searchl = search.ToLower().Split(' ');
            foreach (var product in _db.Products)
            {
                foreach (var s in searchl)
                {
                    if (product.name.ToLower().Contains(s))
                    {
                        list.Add(product);
                        break;
                    }
                }
            }

            if (list.Count == 0)
                return null;
            return list;
        }

        public List<Product> Filter(String pfrom, String pto, String searchString)
        {
            //List<Product> products = Search(searchString);
            var from = Convert.ToInt32(pfrom);
            var to = Convert.ToInt32(pto);
            foreach (var product in list)
            {
                if (!(Convert.ToInt32(product.price) > from && Convert.ToInt32(product.price) < to))
                {
                    list.Remove(product);
                }
            }
            return list;
        }
    }
}
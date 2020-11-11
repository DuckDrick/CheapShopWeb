using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CheapShopWeb.Models;

namespace CheapShopWeb.Services
{
    public class Filtering
    {
        public static List<Product> Filter(List<Product> productList, string name, string min, string max, string groups, string sources)
        {

            if (name != null)
            {
                productList = productList.FindAll(product => product.name.ToLower().Contains(name.ToLower()));
            }
            if (min != null)
            {
                productList = productList.FindAll(product => float.Parse(product.price.Replace('.',',')) >= float.Parse(min));
            }
            if (max != null)
            {
                productList = productList.FindAll(product => float.Parse(product.price.Replace('.', ',')) <= float.Parse(max));
            }
            if (groups != null)
            {
                productList = productList.FindAll(product => groups.Split(' ').Any(group => product.group.Equals(group)));
            }
            if (sources != null)
            {
                productList = productList.FindAll(product => sources.Split(' ').Any(source => product.source.Equals(source)));
            }

            return productList;
        }
    }
}
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
                productList = productList.FindAll(product => name.ToLower().Split(' ').All(query => product.name.ToLower().Contains(query)));
            }
            if (!string.IsNullOrEmpty(min))
            {
                productList = productList.FindAll(product => float.Parse(product.price.Replace('.',',')) >= float.Parse(min));
            }
            if (!string.IsNullOrEmpty(max))
            {
                productList = productList.FindAll(product => float.Parse(product.price.Replace('.', ',')) <= float.Parse(max));
            }
            if (groups != null)
            {
                productList = productList.FindAll(product => groups.Split(' ').Any(group => product.group.ToLower().Equals(group.ToLower())));
            }
            if (sources != null)
            {
                productList = productList.FindAll(product => sources.Split(' ').Any(source => product.source.ToLower().Equals(source.ToLower())));
            }

            return productList;
        }
    }
}
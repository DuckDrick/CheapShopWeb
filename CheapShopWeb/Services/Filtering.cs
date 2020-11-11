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
            var filteredList = new List<Product>(productList);
            if (name != null)
            {
                filteredList = filteredList.FindAll(product => product.name.ToLower().Contains(name.ToLower()));
            }
            if (min != null)
            {
                filteredList = filteredList.FindAll(product => float.Parse(product.price.Replace('.',',')) >= float.Parse(min));
            }
            if (max != null)
            {
                filteredList = filteredList.FindAll(product => float.Parse(product.price.Replace('.', ',')) <= float.Parse(max));
            }
            if (groups != null)
            {
                filteredList = filteredList.FindAll(product => groups.Split(' ').Any(group => product.group.Equals(group)));
            }
            if (sources != null)
            {
                filteredList = filteredList.FindAll(product => sources.Split(' ').Any(source => product.source.Equals(source)));
            }

            return filteredList;
        }
    }
}
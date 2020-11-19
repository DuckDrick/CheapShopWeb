using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using CheapShopWeb.Models;
using Comparison_shopping_engine;

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
                productList = productList.FindAll(product => SToFFunc(product.price) >= SToFFunc(min));
            }
            if (!string.IsNullOrEmpty(max))
            {
                productList = productList.FindAll(product => SToFFunc(product.price) <= SToFFunc(max));
            }
            if (!string.IsNullOrEmpty(groups))
            {
                var smallergroups = groups.Split(',');
                foreach (var sgroup in smallergroups)
                {
                    var method = typeof(SmallerGroups).GetMethod(sgroup + "Group");
                    if (!(method == null))
                    {
                        var smallerGroupList = (List<string>)method.Invoke(new SmallerGroups(), null);

                        productList = productList.FindAll(product =>
                            smallerGroupList.Any(group => product.group.ToLower().Equals(group.ToLower())));
                    }
                }

            }
            if (!string.IsNullOrEmpty(sources))
            {
                productList = productList.FindAll(product => sources.Split(',').Any(source => product.source.ToLower().Equals(source.ToLower())));
            }

            return productList;
        }

        public static List<Product> GetSimilarProducts(List<Product> productList, Product prod)
        {

            //nezinau ka darau
            List<Product> newlist= new List<Product>();
            newlist.Add(prod);
            string[] searchString = prod.name.Split(' ');
            if (!string.IsNullOrEmpty(prod.name))
            {
                foreach (var product in productList)
                {
                    
                }
            }

            return newlist;
        }
        
        public static Func<string, float> SToFFunc = num =>  //string to float function for lambda expression // makes number string comparable
        {
            float n = float.Parse(num) * 100;
            return n;
        };
    }
}
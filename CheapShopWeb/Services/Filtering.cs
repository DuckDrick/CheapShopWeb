using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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

        public static List<Product> GetSimilarProducts(List<Product> productList, Product prod, string searchString)
        {

            productList = productList.FindAll(product => searchString.ToLower().Split(' ').All(query => product.name.ToLower().Contains(query)));
            List<Product> newlist= new List<Product>();
            foreach (var product in productList)
            {
                if (LevenshteinDistance.Compute(product.name, prod.name) <= 3*prod.name.Split(' ').Length) //or 4*//
                {
                    newlist.Add(product);
                }
            }

            newlist.Sort((x, y) => String.Compare(x.price, y.price, StringComparison.Ordinal));
            newlist.Insert(0, prod);
            return newlist;
        }
        public static List<Product> GetSimilarProductsGroup(List<Product> productList, Product prod, string itemsGroup)
        {

            var method = typeof(SmallerGroups).GetMethod(itemsGroup + "Group");
            if (!(method == null))
            {
                var smallerGroupList = (List<string>)method.Invoke(new SmallerGroups(), null); 
                productList = productList.FindAll(product =>
                        smallerGroupList.Any(group => product.group.ToLower().Equals(group.ToLower())));
            } 
            List<Product> newlist = new List<Product>();
            foreach (var product in productList)
            {
                if (LevenshteinDistance.Compute(product.name, prod.name) <= 3 * prod.name.Split(' ').Length) //or 4*//
                {
                    newlist.Add(product);
                }
            }
            newlist.Sort((x, y) => String.Compare(x.price, y.price, StringComparison.Ordinal));
            newlist.Insert(0, prod);
            return newlist;
        }

        public static Func<string, float> SToFFunc =
            num => //string to float function for lambda expression // makes number string comparable
            {
                float n = float.Parse(num) * 100;
                return n;
            };
    }
    static class LevenshteinDistance
    {
        /// <summary>
        /// Compute the distance between two strings.
        /// </summary>
        public static int Compute(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }
    }
}
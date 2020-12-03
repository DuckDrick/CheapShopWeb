using System;
using System.Collections.Generic;
using System.Linq;
using CheapShopWeb.Models;
using Comparison_shopping_engine;

namespace CheapShopWeb.Services
{
    public class Filtering
    {
        public delegate bool Compare<T>(Product item1, T item2);

        private static readonly Compare<float> PriceAboveOrEqual = (item, price) => SToFFunc(item.price) >= price;
        private static readonly Compare<float> PriceBelowOrEqual = (item, price) => SToFFunc(item.price) <= price;
        private static readonly Compare<string>
            StringComparatorGroup = (item, str) => item.group.ToLower().Equals(str.ToLower());
        private static readonly Compare<string>
            StringComparatorSource = (item, str) => item.source.ToLower().Equals(str.ToLower());
        public static List<Product> Filter(List<Product> productList, string name, string min, string max, string groups, string sources)
        {


            if (name != null)
            {
                productList = productList.FindAll(product => name.ToLower().Split(' ').All(query => product.name.ToLower().Contains(query)));
            }
            if (!string.IsNullOrEmpty(min))
            {
                productList = productList.FindAll(product => PriceAboveOrEqual(product, SToFFunc(min)));
            }
            if (!string.IsNullOrEmpty(max))
            {
                productList = productList.FindAll(product => PriceBelowOrEqual(product, SToFFunc(max)));
            }
            if (!string.IsNullOrEmpty(groups))
            {
                var smallerGroups = groups.Split(',');
                foreach (var sGroup in smallerGroups)
                {
                    var method = typeof(SmallerGroups).GetMethod(sGroup + "Group");
                    if (method == null) continue;
                    var smallerGroupList = (List<string>) method.Invoke(new SmallerGroups(), null);

                    productList = productList.FindAll(product =>
                        smallerGroupList.Any(group => StringComparatorGroup(product, group)));
                }
            }
            if (!string.IsNullOrEmpty(sources))
            {
                productList = productList.FindAll(product => 
                    sources.Split(',').Any(source => StringComparatorSource(product, source)));
            }

            return productList;
        }


        public static Tuple<List<int>, List<int>> CountAmounts(List<Product> items)
        {
            var siteCounts = new List<int>();
            var groupCounts = new List<int>();
            foreach (ScrapedSites site in Enum.GetValues(typeof(ScrapedSites)))
            {
                siteCounts.Add(CountHowMany(site, items));
            }
            foreach (MainGroups group in Enum.GetValues(typeof(MainGroups)))
            {
                groupCounts.Add(CountHowMany(group, items));
            }

            return new Tuple<List<int>, List<int>>(siteCounts, groupCounts);
        }

        public static int CountHowMany<T>(T value, List<Product> items)
        {
            if (value.GetType() == typeof(MainGroups))
                return items.Count(product => CheckIfInGroup(value.ToString(), product.group));

            return items.Count(product => product.source.ToLower().Contains(value.ToString()));
        }

        private static bool CheckIfInGroup(string group, string productGroup)
        {
            var method = typeof(SmallerGroups).GetMethod(group + "Group");
                if (method == null) return false;
                var smallerGroupList = (List<string>)method.Invoke(new SmallerGroups(), null);

                return smallerGroupList.Any(g => g.Equals(productGroup));
        }


        public static List<Product> GetSimilarProducts(List<Product> productList, Product prod, string searchString)
        {

            productList = productList.FindAll(product => searchString.ToLower().Split(' ').All(query => product.name.ToLower().Contains(query)));
            var newlist= new List<Product>();
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
            var newList = new List<Product>();
            foreach (var product in productList)
            {
                if (LevenshteinDistance.Compute(product.name, prod.name) <= 3 * prod.name.Split(' ').Length) //or 4*//
                {
                    newList.Add(product);
                }
            }
            newList.Sort((x, y) => string.Compare(x.price, y.price, StringComparison.Ordinal));
            newList.Insert(0, prod);
            return newList;
        }

        public static Func<string, float> SToFFunc =
            num => //string to float function for lambda expression // makes number string comparable
            {
                var n = float.Parse(num.Replace('.',','));
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
            var n = s.Length;
            var m = t.Length;
            var d = new int[n + 1, m + 1];

            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            for (var i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (var j = 0; j <= m; d[0, j] = j++)
            {
            }
            for (var i = 1; i <= n; i++)
            {
                for (var j = 1; j <= m; j++)
                {
                    var cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            return d[n, m];
        }
    }
}
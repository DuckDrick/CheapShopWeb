using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using CheapShopWeb;
using CheapShopWeb.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.Configuration.ConfigurationBuilders;
using Npgsql;

namespace CheapShopWeb.Services
{
    public class DBService
    {
        public static List<Product> GetAll(string search, int page)
        {
            // ConfigurationManager.ConnectionStrings["ABCD"].ConnectionString;
            var list = new List<Product>();
            using (var con = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["database"].ConnectionString))
            {
                con.Open();
                var dataAdapter = new NpgsqlDataAdapter("SELECT * FROM dbo.\"Products\"", con);
                var dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "dbo.\"Products\"");
                var dataTable = dataSet.Tables["dbo.\"Products\""];

                foreach (DataRow row in dataTable.Rows)
                {
                    var item = row.ItemArray;
                    list.Add(new Product(item[1].ToString(), item[2].ToString(), item[3].ToString(), item[4].ToString(), item[5].ToString(), item[6].ToString()));
                }
                
                if (search.IsNullOrWhiteSpace()) return list.Take(10).ToList();
                var items= list.Where(product => search.Split(' ').Any(s => product.name.ToLower().Contains(s.ToLower()))).ToList();
                return items.Skip((page - 1) * 15).Take(15).ToList();
            }
        }

        public static List<Product> GetSearch(string search)
        {
            var list = new List<Product>();
            using (var con = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["database"].ConnectionString))
            {
                con.Open();
                var command = new NpgsqlCommand("Select * from dbo.\"Products\" where name LIKE @loc_name", con);
                command.Parameters.AddWithValue("@loc_name", "%" + search + "%");
                var dr = command.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new Product(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString()));
                }
            }
            return list;
        }

        private static string Check(string origCol, string newColumn, string colName)
        {
            if (!newColumn.IsNullOrWhiteSpace() && !origCol.Equals(newColumn))
            {
                return "\"" + colName + "\"=@" + colName + ",";
            }

            return "";
        }

        private static bool ShouldUpdate(string orgCol, string newCol)
        {
            return !newCol.IsNullOrWhiteSpace() && !orgCol.Equals(newCol);
        }
        public static void Update(Product product, string nname, string nsource, string nproductLink, string nprice, string ngroup,
            string nphotoLink)
        {
            var updateQuery = "";
            updateQuery += Check(product.name, nname, "name");
            updateQuery += Check(product.source, nsource, "source");
            updateQuery += Check(product.product_link, nproductLink, "product_link");
            updateQuery += Check(product.price, nprice, "price");
            updateQuery += Check(product.group, ngroup, "group");
            updateQuery += Check(product.photo_link, nphotoLink, "photo_link");
            if(updateQuery.Length > 0)
            {
                using (var con = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["database"].ConnectionString))
                {
                    con.Open();
                    updateQuery = updateQuery.TrimEnd(',', ' ');
                    //
                    var sql = new NpgsqlCommand(
                        "UPDATE dbo.\"Products\" SET " + updateQuery + " where name='" + product.name + "' and source='" + product.source + "';", con);
                    sql.Parameters.AddWithValue("@name", nname);
                    sql.Parameters.AddWithValue("@source", nsource);
                    sql.Parameters.AddWithValue("@product_link", nproductLink);
                    sql.Parameters.AddWithValue("@price", nprice);
                    sql.Parameters.AddWithValue("@group", ngroup);
                    sql.Parameters.AddWithValue("@photo_link", nphotoLink);

                    var dataAdapter = new NpgsqlDataAdapter("select * FROM dbo.\"Products\"", con)
                    {
                        UpdateCommand = sql
                    };

                    var dataSet = new DataSet();
                    dataAdapter.AcceptChangesDuringUpdate = true;
                    dataAdapter.Fill(dataSet, "dbo.\"Products\"");
                    Debug.WriteLine(sql.CommandText);
                    var dataTable = dataSet.Tables["dbo.\"Products\""];
                    //dataTable.Rows.Find(row => row[1].ToString().Equals(product.name));
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        if (!dataRow[1].ToString().Equals(product.name)) continue;
                        if (ShouldUpdate(product.name, nname)) dataRow[1] = nname;
                        if (ShouldUpdate(product.source, nsource)) dataRow[2] = nname;
                        if (ShouldUpdate(product.product_link, nproductLink)) dataRow[5] = nname;
                        if (ShouldUpdate(product.price, nprice)) dataRow[3] = nname;
                        if (ShouldUpdate(product.@group, ngroup)) dataRow[6] = nname;
                        if (ShouldUpdate(product.photo_link, nphotoLink)) dataRow[4] = nname;
                        break;
                    }
                    dataAdapter.Update(dataTable);
                  
                    dataAdapter.Dispose();
                }
            }
        }

        public static void Delete(string link)
        {
            using (var con = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["database"].ConnectionString))
            {
                con.Open();
                var command = new NpgsqlCommand("DELETE FROM dbo.\"Products\" WHERE product_link = '" + link+"';",con);
                //command.Parameters.AddWithValue("@loc_name", "\'" + link + "\'");
                command.ExecuteNonQuery();
            }
        }

        public static void Add(string name, string source, string link, string price, string group, string photoLink)
        {
            using (var con = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["database"].ConnectionString))
            {
                con.Open();
                var command = new NpgsqlCommand("INSERT INTO dbo.\"Products\" VALUES(default, '" + name +"','"+source+"','"+price.Replace(',','.')+"','"+photoLink+"','"+link+"','"+group+ "');", con);
                //command.Parameters.AddWithValue("@loc_name", "\'" + link + "\'");
                command.ExecuteNonQuery();
            }
        }
    }

}
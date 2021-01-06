using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        public static List<Product> GetAll(string search)
        {
            var list = new List<Product>();
            using (var con = new NpgsqlConnection(Secret.ConnectionString))
            {
                con.Open();
                NpgsqlCommand command;
                command = new NpgsqlCommand("SELECT * FROM Products", con);
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new Product(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString()));
                }
                if (!search.IsNullOrWhiteSpace())
                {
                    var items=new List<Product>();
                    foreach (var product in list)
                    {
                        foreach (var s in search.Split( ' '))
                        {
                            if (product.name.ToLower().Contains(s.ToLower()))
                            {
                                items.Add(product);
                                break;
                            }
                        }
                    }
                    return items.ToList();
                }
            }
            return list;
        }

        public static List<Product> GetSearch(string search)
        {
            var list = new List<Product>();
            using (var con = new NpgsqlConnection(Secret.ConnectionString))
            {
                con.Open();
                NpgsqlCommand command = new NpgsqlCommand("Select * from Products where name LIKE @loc_name", con);
                command.Parameters.AddWithValue("@loc_name", "%" + search + "%");
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new Product(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString()));
                }
            }
            return list;
        }

        public static void Update(string name, string source, string productLink, string price, string group,
            string photoLink, string bname, 
            string bsource, string bproductLink, string 
                bprice, 
            string bgroup, 
            string bphotoLink)
        {
            List<String> updatestring=new List<string>();
            List<String> updatecolumnsList = new List<string>();
            if (!name.Equals(bname) && !name.IsNullOrWhiteSpace())
            {
                updatestring.Add(name);
                updatecolumnsList.Add("name");
            }
            if (!bsource.Equals(source) && !source.IsNullOrWhiteSpace())
            {
                updatestring.Add(source);
                updatecolumnsList.Add("source");
            }
            if (!productLink.Equals(bproductLink) && !productLink.IsNullOrWhiteSpace())
            {
                updatestring.Add(productLink);
                updatecolumnsList.Add("product_link");
            }
            if (!price.Equals(bprice) && !price.IsNullOrWhiteSpace())
            {
                updatestring.Add(price.Replace(",","."));
                updatecolumnsList.Add("price");
            }
            if (!group.Equals(bgroup) && !group.IsNullOrWhiteSpace())
            {
                updatestring.Add(group);
                updatecolumnsList.Add("group");
            }
            if (!photoLink.Equals(bphotoLink) && !photoLink.IsNullOrWhiteSpace())
            {
                updatestring.Add(photoLink);
                updatecolumnsList.Add("photo_link");
            }

            using (var con = new NpgsqlConnection(Secret.ConnectionString))
            {
                con.Open();
                string sql = "UPDATE Products SET ";
                for (int i = 0; i < updatestring.Count; i++)
                {
                    sql = sql + updatecolumnsList[i] + " = '" + updatestring[i] + "',";
                }

                sql = sql.Substring(0,sql.Length - 1);
                sql = sql + " WHERE name ='"+bname +"' AND source ='"+bsource+"' ;";
                var command = new NpgsqlCommand(sql, con);
                command.ExecuteNonQueryAsync();
            }


            return;
        }

        public static void Delete(string link)
        {
            using (var con = new NpgsqlConnection(Secret.ConnectionString))
            {
                con.Open();
                var command = new NpgsqlCommand("DELETE FROM Products WHERE product_link = '"+link+"';",con);
                //command.Parameters.AddWithValue("@loc_name", "\'" + link + "\'");
                command.ExecuteNonQuery();
            }
        }

        public static void Add(string name, string source, string link, string price, string group, string photoLink)
        {
            using (var con = new NpgsqlConnection(Secret.ConnectionString))
            {
                con.Open();
                var command = new NpgsqlCommand("INSERT INTO Products VALUES('" + name +"','"+source+"','"+price.Replace(',','.')+"','"+photoLink+"','"+link+"','"+group+ "');", con);
                //command.Parameters.AddWithValue("@loc_name", "\'" + link + "\'");
                command.ExecuteNonQuery();
            }
        }
    }

}
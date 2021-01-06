using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CheapShopWeb.Models;
using CheapShopWeb.Services;

namespace CheapShopWeb.Controllers
{
    public class TrustedUserController : Controller
    {
        public ActionResult TrustedUser(string search)
        {

            return View(DBService.GetAll(search));
        }

        public ActionResult TrustedUserEdit(string name, string link, string photo, string price, string source, string group)
        {
            return View(new Product(name,source,price, photo, link, group));
        }

        public ViewResult Edited(string name, string source, string product_link, string price, 
            string group, string photo_link, string bname, string bsource, 
            string bproduct_link, string bprice, 
            string bgroup, string bphoto_link)
        {
            DBService.Update(name, source, product_link, price, group, photo_link, bname, bsource, bproduct_link,
                bprice, bgroup, bphoto_link);
            return View("TrustedUser",DBService.GetAll(""));
        }

        public ActionResult TrustedUserDelete(string link)
        {
            DBService.Delete(link);
            return View("TrustedUser", DBService.GetAll(""));
        }

        public ActionResult TrustedUserAdd(string name, string source, string link, string price, string group, string photo_link)
        {
            DBService.Add(name, source, link, price, group, photo_link);
            return View("TrustedUser", DBService.GetAll(""));
        }
    }
}
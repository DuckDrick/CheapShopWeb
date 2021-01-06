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
    [Authorize(Roles = UserRoles.ADMIN)]
    public class TrustedUserController : Controller
    {
        public ActionResult TrustedUser(string search, int? page)
        {

            return View(DBService.GetAll(search, page??1));
        }

        public ActionResult TrustedUserEdit(string name, string link, string photo, string price, string source, string group)
        {
            return View(new Product(name,source,price, photo, link, group));
        }

        public ViewResult Edited(Product product, string nname, string nsource, 
            string nproduct_link, string nprice, 
            string ngroup, string nphoto_link)
        {
            DBService.Update(product, nname, nsource, nproduct_link,
                nprice, ngroup, nphoto_link);
            return View("TrustedUser",DBService.GetAll("", 1));
        }

        public ActionResult TrustedUserDelete(string link)
        {
            DBService.Delete(link);
            return View("TrustedUser", DBService.GetAll("", 1));
        }

        public ActionResult TrustedUserAdd(string name, string source, string link, string price, string group, string photo_link)
        {
            DBService.Add(name, source, link, price, group, photo_link);
            return View("TrustedUser", DBService.GetAll("", 1));
        }
    }
}
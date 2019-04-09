using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MedicineManageSystem.Models;

namespace MedicineManageSystem.Controllers
{
    public class HomeController : Controller
    {
        Guardian_AngelEntities ga = new Guardian_AngelEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string 帳號, string 密碼)
        {
            var member = ga.Manage_Account
                .Where(m => m.MamageId == 帳號 && m.Manage_Password == 密碼)
                .FirstOrDefault();
            if (member != null)
            {
                FormsAuthentication.RedirectFromLoginPage(member.MamageId,true);
                return RedirectToAction("Index","Category");
            }
            ViewBag.IsLogin = true;
            return View();
        }
    }
}
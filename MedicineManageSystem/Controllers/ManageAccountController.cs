using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicineManageSystem.Models;

namespace MedicineManageSystem.Controllers
{
    public class ManageAccountController : Controller
    {
        Guardian_AngelEntities ga = new Guardian_AngelEntities();
        // GET: ManageAccount
        [Authorize]
        public ActionResult Index()
        {
            string uid = User.Identity.Name;
            string role = ga.Manage_Account.Where(m => m.MamageId == uid).FirstOrDefault().Manage_Role;
            if (role != "網管")
            {
                return RedirectToAction("Index", "PermissionErrorMessage", new { msg = "您的身份無管理會員的權限" });
            }

            List<Manage_Account> members = new List<Manage_Account>();
            foreach (var item in ga.Manage_Account)
            {
                var member = new Manage_Account();
                member.MamageId = item.MamageId;
                member.Manage_Password = item.Manage_Password;
                member.Manage_authorization =
                    (item.Manage_authorization.Contains("R") ? "讀取 " : "") +
                    (item.Manage_authorization.Contains("C") ? "新增 " : "") +
                    (item.Manage_authorization.Contains("U") ? "修改 " : "") +
                    (item.Manage_authorization.Contains("D") ? "刪除 " : "");
                member.Manage_Role = item.Manage_Role;
                members.Add(member);
            }
            return View(members);
        }

        [Authorize]
        public ActionResult Create()
        {
            string uid = User.Identity.Name;
            string role = ga.Manage_Account.Where(m => m.MamageId == uid).FirstOrDefault().Manage_Role;
            if (role != "網管")
            {
                return RedirectToAction("Index", "PermissionErrorMessage", new { msg = "您的身份無管理會員的權限" });
            }
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult Create(string MamageId, string Manage_Password, string Manage_Role, string[] 權限)
        {
            string userid = MamageId;
            var tempMember = ga.Manage_Account.Where(m => m.MamageId == userid).FirstOrDefault();
            if (tempMember != null)
            {
                ViewBag.IsMember = true;
                return View();
            }

            string Permission = "R";

            if (權限 != null)
            {
                for (int i = 0; i < 權限.Length; i++)
                {
                    Permission += 權限[i];
                }
            }

            Manage_Account member = new Manage_Account();
            member.MamageId = MamageId;
            member.Manage_Password = Manage_Password;
            member.Manage_Role = Manage_Role;
            member.Manage_authorization = Permission;
            ga.Manage_Account.Add(member);
            ga.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Delete(string userid)
        {
            string uid = User.Identity.Name;
            string role = ga.Manage_Account.Where(m => m.MamageId == uid).FirstOrDefault().Manage_Role;
            if (role != "網管")
            {
                return RedirectToAction("Index", "PermissionErrorMessage", new { msg = "您的身份無管理會員的權限" });
            }
            var member = ga.Manage_Account.Where(m => m.MamageId == userid).FirstOrDefault();
            ga.Manage_Account.Remove(member);
            ga.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Edit(string userid)
        {
            string uid = User.Identity.Name;
            string role = ga.Manage_Account.Where(m => m.MamageId == uid).FirstOrDefault().Manage_Role;
            if (role != "網管")
            {
                return RedirectToAction("Index", "PermissionErrorMsg", new { msg = "您的身份無管理會員的權限" });
            }

            var member = ga.Manage_Account.Where(m => m.MamageId == userid).FirstOrDefault();
            return View(member);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(string MamageId, string Manage_Password, string Manage_Role, string[] 權限)
        {
            string Permission = "R";
            if (權限 != null)
            {
                for (int i = 0; i < 權限.Length; i++)
                {
                    Permission += 權限[i];
                }
            }

            var member = ga.Manage_Account.Where(m => m.MamageId == MamageId).FirstOrDefault();
            member.Manage_Password = Manage_Password;
            member.Manage_Role = Manage_Role;
            member.Manage_authorization = Permission;
            ga.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}

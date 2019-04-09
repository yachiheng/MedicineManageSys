using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicineManageSystem.Models;

namespace MedicineManageSystem.Controllers
{
    public class CategoryController : Controller
    {
        Guardian_AngelEntities ga = new Guardian_AngelEntities();
        // GET: Category
        [Authorize]
        public ActionResult Index()
        {
            string uid = User.Identity.Name;
            string Permission = ga.Manage_Account.Where(m => m.MamageId == uid).FirstOrDefault().Manage_authorization;
            ViewBag.Permission = Permission;
            List<Pill_Factory_Category> category = new List<Pill_Factory_Category>();
            foreach (var item in ga.Pill_Factory_Category.OrderByDescending(m => m.P_update_date))
            {
                category.Add(new Pill_Factory_Category()
                {
                    P_pharmaceutical_factory_Id = item.P_pharmaceutical_factory_Id,
                    P_pharmaceutical_factory = item.P_pharmaceutical_factory,
                    P_edit_user = item.P_edit_user,
                    P_update_date = Sys.StringConverDateTimeString(item.P_update_date),
                    P_create_date = Sys.StringConverDateTimeString(item.P_create_date)
                });
            }
            return View(category);
        }

        [Authorize]
        public ActionResult Create()
        {
            string uid = User.Identity.Name;
            string Permission = ga.Manage_Account.Where(m => m.MamageId == uid).FirstOrDefault().Manage_authorization;
            if (!Permission.Contains("C"))
            {
                return RedirectToAction("Index", "PermissionErrorMessage", new { msg = "您的身份無新增的權限" });
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(string P_pharmaceutical_factory)
        {
            string editdate = DateTime.Now.ToString("yyyyMMddHHmmss");
            Pill_Factory_Category category = new Pill_Factory_Category();
            category.P_pharmaceutical_factory = P_pharmaceutical_factory;
            category.P_edit_user = User.Identity.Name;
            category.P_create_date = editdate;
            category.P_update_date= editdate;
            ga.Pill_Factory_Category.Add(category);
            ga.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Delete(int cid)
        {
            string uid = User.Identity.Name;
            string Permission = ga.Manage_Account.Where(m => m.MamageId == uid).FirstOrDefault().Manage_authorization;
            if (!Permission.Contains("D"))
            {
                return RedirectToAction("Index", "PermissionErrorMessage", new { msg = "您的身份無刪除的權限" });
            }
            var pills = ga.Pill.Where(m => m.P_pharmaceutical_factory_Id == cid).ToList();
            var category = ga.Pill_Factory_Category.Where(m => m.P_pharmaceutical_factory_Id == cid).FirstOrDefault();
            ga.Pill.RemoveRange(pills);
            ga.Pill_Factory_Category.Remove(category);
            ga.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Edit(int cid)
        {
            string uid = User.Identity.Name;
            string Permission = ga.Manage_Account.Where(m => m.MamageId == uid).FirstOrDefault().Manage_authorization;
            if (!Permission.Contains("U"))
            {
                return RedirectToAction("Index", "PermissionErrorMessage", new { msg = "您的身份無編輯的權限" });
            }

            var category = ga.Pill_Factory_Category.Where(m => m.P_pharmaceutical_factory_Id == cid).FirstOrDefault();
            return View(category);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(int P_pharmaceutical_factory_Id, string P_pharmaceutical_factory)
        {
            string editdate = DateTime.Now.ToString("yyyyMMddHHmmss");
            var category = ga.Pill_Factory_Category.Where(m => m.P_pharmaceutical_factory_Id == P_pharmaceutical_factory_Id).FirstOrDefault();
            category.P_pharmaceutical_factory = P_pharmaceutical_factory;
            category.P_edit_user = User.Identity.Name;
            category.P_update_date = editdate;
            ga.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
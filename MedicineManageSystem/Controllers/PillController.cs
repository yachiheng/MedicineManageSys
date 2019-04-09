using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicineManageSystem.Models;

namespace MedicineManageSystem.Controllers
{
    public class PillController : Controller
    {
        Guardian_AngelEntities ga = new Guardian_AngelEntities();
        // GET: Pill
        [Authorize]
        public ActionResult Index(int cid = 1)
        {
            string uid = User.Identity.Name;
            string Permission = ga.Manage_Account.Where(m => m.MamageId == uid).FirstOrDefault().Manage_authorization;
            ViewBag.Permission = Permission;
            PillCategoryViewModel vm = new PillCategoryViewModel();
            vm.Category = ga.Pill_Factory_Category
                 .OrderByDescending(m => m.P_update_date).ToList();
            var tempProduct = ga.Pill
                .Where(m => m.P_pharmaceutical_factory_Id == cid)
                .OrderByDescending(m => m.P_update_date).ToList();

            List<Pill> pill = new List<Pill>();
            foreach (var item in tempProduct)
            {
                pill.Add(new Pill()
                {
                    P_id = item.P_id,
                    P_code = item.P_code,
                    P_pharmaceutical_factory_Id = item.P_pharmaceutical_factory_Id,
                    P_name = item.P_name,
                    P_product_name = item.P_product_name,
                    P_scientific_name = item.P_scientific_name,
                    P_remark = item.P_remark,
                    P_photo = item.P_photo,
                    P_editor = item.P_editor,
                    P_update_date = Sys.StringConverDateTimeString(item.P_update_date),
                    P_create_date = Sys.StringConverDateTimeString(item.P_create_date)
                });
            }
            vm.Pill = pill;
            return View(vm);
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
            ViewBag.Category = ga.Pill_Factory_Category.ToList();
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(string P_name, int 藥廠編號,  string P_product_name, string P_code, string P_scientific_name, HttpPostedFileBase fImg, string P_remark)
        {
            var tempPill = ga.Pill.Where(m => m.P_code == P_code).FirstOrDefault();
            if (tempPill != null)
            {
                ViewBag.IsPill = true;
                ViewBag.Category = ga.Pill_Factory_Category.ToList();
                return View();
            }

            string fileName = "question.png";
            if (fImg != null)
            {
                if (fImg.ContentLength > 0)
                {
                    fileName = Guid.NewGuid().ToString() + ".jpg";
                    var path = string.Format("{0}/{1}", Server.MapPath("~/Images/p"), fileName);
                    fImg.SaveAs(path);
                }
            }

            string editdate = DateTime.Now.ToString("yyyyMMddHHmmss");
            Pill pill = new Pill();
            pill.P_code = P_code;
            pill.P_pharmaceutical_factory_Id = 藥廠編號;
            pill.P_name = P_name;
            pill.P_product_name = P_product_name;
            pill.P_scientific_name = P_scientific_name;
            pill.P_remark = P_remark;
            pill.P_photo = fileName;
            pill.P_editor = User.Identity.Name;
            pill.P_create_date = editdate;
            pill.P_update_date = editdate;
            ga.Pill.Add(pill);
            ga.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Delete(int pid)
        {
            string uid = User.Identity.Name;
            string Permission = ga.Manage_Account.Where(m => m.MamageId == uid).FirstOrDefault().Manage_authorization;
            if (!Permission.Contains("D"))
            {
                return RedirectToAction("Index", "PermissionErrorMessage", new { msg = "您的身份無刪除的權限" });
            }
            var product = ga.Pill.Where(m => m.P_id == pid).FirstOrDefault();
            var filename = product.P_photo;
            if (filename != "question.png")
            {
                System.IO.File.Delete
                    (string.Format("{0}/{1}", Server.MapPath("~/Images/p"), filename));
            }
            ga.Pill.Remove(product);
            ga.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Edit(int pid)
        {
            string uid = User.Identity.Name;
            string Permission = ga.Manage_Account.Where(m => m.MamageId == uid).FirstOrDefault().Manage_authorization;
            if (!Permission.Contains("U"))
            {
                return RedirectToAction("Index", "PermissionErrorMessage", new { msg = "您的身份無編輯的權限" });
            }
            var pill = ga.Pill.Where(m => m.P_id== pid).FirstOrDefault();
            ViewBag.Category =ga.Pill_Factory_Category.ToList();
            return View(pill);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(int P_id,string P_name, string P_code, int 藥廠編號, string P_product_name,  string P_scientific_name, string P_remark, HttpPostedFileBase fImg, string P_photo)
        {
            string fileName = "";
            if (fImg != null)
            {
                if (fImg.ContentLength > 0)
                {
                    fileName = Guid.NewGuid().ToString() + ".jpg";
                    var path = string.Format("{0}/{1}", Server.MapPath("~/Images/p"), fileName);
                    fImg.SaveAs(path);
                }
            }
            else
            {
                fileName = P_photo;
            }

            string editdate = DateTime.Now.ToString("yyyyMMddHHmmss");
            var pill = ga.Pill.Where(m => m.P_id == P_id).FirstOrDefault();
            pill.P_name = P_name;
            pill.P_code = P_code;
            pill.P_pharmaceutical_factory_Id = 藥廠編號;
           
            pill.P_product_name = P_product_name;
            pill.P_scientific_name = P_scientific_name;
            pill.P_remark = P_remark;
            pill.P_photo = fileName;
            pill.P_editor = User.Identity.Name;
            pill.P_create_date = editdate;
            pill.P_update_date = editdate;
            ga.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
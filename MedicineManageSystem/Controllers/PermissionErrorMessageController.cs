﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MedicineManageSystem.Controllers
{
    public class PermissionErrorMessageController : Controller
    {
        // GET: PermissionErrorMessage
        [Authorize]
        public ActionResult Index(string msg)
        {
            ViewBag.ErrorMsg = msg;
            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MedicineManageSystem.Models;

namespace MedicineManageSystem.Models
{
    public class PillCategoryViewModel
    {
        public List<Pill_Factory_Category> Category { get; set; }
        public List<Pill> Pill { get; set; }
    }
}
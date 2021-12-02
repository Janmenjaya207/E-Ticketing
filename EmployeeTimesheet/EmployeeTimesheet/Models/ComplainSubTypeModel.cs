using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeTimesheet.Models
{
    public class ComplainSubTypeModel
    {
        public int CompainsubtypeId { get; set; }
        public Nullable<int> ComplainTypeId { get; set; }
        public string SubtypeName { get; set; }
        public List<SelectListItem> lstCompalinType { get; set; }
        public List<vw_Complain_Sub_Type> vw_Complain_Sub_Types { get; set; }
    }
}
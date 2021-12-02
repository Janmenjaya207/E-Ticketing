using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeTimesheet.Models
{
    public class ComplainModel
    {
        public ComplainDetail ComplainDetail { get; set; }
        public List<SelectListItem> lstpriority { get; set; }
        public List<SelectListItem> lstAssignee { get; set; }
        public List<SelectListItem> lstcomplaintype { get; set; }
        public List<SelectListItem> lstcomplainsubtype { get; set; }
        public List<SelectListItem> lstulb { get; set; }
        public List<vw_Complain_Dtl> vw_Complain_Dtls { get; set; }
        public List<SelectListItem> Getemptype { get; set; }
    }
}
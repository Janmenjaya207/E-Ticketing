using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeTimesheet.Models
{
    public class AssignTaskModel
    {
        public int TaskId { get; set; }
        public int RegistrationId { get; set; }
        public int DistrictId { get; set; }
        public int ProjectId { get; set; }
        public int ModuleId { get; set; }
        public string TransactionCode { get; set; }
        public string TransactionName { get; set; }
        public DateTime TranCreationDate { get; set; }
        public string AssignedTaskDetail { get; set; }
        public List<SelectListItem> GetProject { get; set; }
        public List<SelectListItem> GetDistrict { get; set; }
        public List<SelectListItem> GetModule { get; set; }
        public List<SelectListItem> GetAllUser { get; set; }
    }
}
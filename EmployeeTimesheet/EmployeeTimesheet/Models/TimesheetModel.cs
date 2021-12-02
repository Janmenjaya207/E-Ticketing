using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeTimesheet.Models
{
    public class TimesheetModel
    {
        public int TimesheetId { get; set; }
        public int RegistrationId { get; set; }
        public int DistrictId { get; set; }
        public int ProjectId { get; set; }
        public int ModuleId { get; set; }
        public string TransactionCode { get; set; }
        public string TransactionName { get; set; }
        public string TransactionDesc { get; set; }
        public DateTime TranCreationDate { get; set; }
        public DateTime TranPostDate { get; set; }
        public string PendingReason { get; set; }
        public string Comments { get; set; }
        public string TaskDetail { get; set; }
        public List<SelectListItem> GetProject { get; set; }
        public List<SelectListItem> GetDistrict { get; set; }
        public List<SelectListItem> GetModule { get; set; }
        public List<SelectListItem> GetTaskList { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeTimesheet.Models
{
    public class ProjectModel
    {
        public int ClientId { get; set; }
        public int ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string Descriotion { get; set; }
        public List<SelectListItem> GetClients { get; set; }
    }
}
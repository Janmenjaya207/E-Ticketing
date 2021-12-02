using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeTimesheet.Models
{
    public class ChangePasswordModel
    {
        public int regid { get; set; }
        public string oldpwd { get; set; }
        public string newpwd { get; set; }
        public string confpwd { get; set; }

    }
}
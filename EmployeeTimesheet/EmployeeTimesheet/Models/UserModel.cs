using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeTimesheet.Models
{
    public class UserModel
    {
        public int RegistrationId { get; set; }
        public int DistrictId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Contactno { get; set; }
        public int UserTypeId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime Createdate { get; set; }
        public string Createby { get; set; }
        public List<SelectListItem> GetUserType { get; set; }
        public List<SelectListItem> GetDistrict { get; set; }
        public List<SelectListItem> lstcomplainTypes { get; set; }
        public int ComplainTypeId { get; set; }
        public List<SelectListItem> lstulb { get; set; }
        public int Ulb_Id { get; set; }
    }
}
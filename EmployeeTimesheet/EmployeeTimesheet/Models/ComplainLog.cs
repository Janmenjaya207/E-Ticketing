//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EmployeeTimesheet.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ComplainLog
    {
        public int LogId { get; set; }
        public Nullable<int> appid { get; set; }
        public Nullable<int> Regid { get; set; }
        public Nullable<int> usertypeid { get; set; }
        public string remark { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<System.DateTime> actiondate { get; set; }
    }
}
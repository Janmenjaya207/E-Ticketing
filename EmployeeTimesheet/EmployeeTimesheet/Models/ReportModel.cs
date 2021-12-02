using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeTimesheet.Models
{
    public class ReportModel
    {
        public int total { get; set; }
        public int assigned { get; set; }
        public int resolved { get; set; }
        public int rejected { get; set; }
        public int reopen { get; set; }
        public int close { get; set; }
        public int pending { get; set; }
        public List<vw_REPORT_COMPLAIN> vw_REPORT_COMPLAINs { get; set; }

        public int mtotal { get; set; }
        public int massigned { get; set; }
        public int mresolved { get; set; }
        public int mrejected { get; set; }
        public int mreopen { get; set; }
        public int mclose { get; set; }
        public int mpending { get; set; }
        public List<vw_REPORT_COMPLAIN_Month> vw_REPORT_COMPLAIN_Months { get; set; }
        public sp_REPORT_COMPLAIN_Year_Result sp_REPORT_COMPLAIN_Year_Result { get; set; }

        public string high { get; set; }
        public string medium { get; set; }
        public string low { get; set; }

        public string highavg { get; set; }
        public string mediumavg { get; set; }
        public string lowavg { get; set; }
    }
}
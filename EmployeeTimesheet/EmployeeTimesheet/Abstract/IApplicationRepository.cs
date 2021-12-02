using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using EmployeeTimesheet.Models;

namespace EmployeeTimesheet.Abstract
{
    public interface IApplicationRepository
    {
        User CheckLogin(string username, string password);
        #region--------------------Admin---------------------------
        bool manageclient(Client client);
        List<Client> lstclients();
        Client GetClient(int id);
        bool DeleteClient(int id);
        List<SelectListItem> clientlist();
        List<SelectListItem> districtlist();
        Project GetProject(int id);
        List<vw_Project_Details> GetProjectList();
        bool manageproject(Project client);
        bool DeleteProject(int id);
        List<SelectListItem> usertypelist();
        List<vw_user_detail> lstuserdtl();
        bool manageuser(UserModel um);
        bool DeleteUser(int id);
        bool CheckUserName(string uname);
        string Username(int regid);
        
        List<ComplainType> complainTypes();
        List<SelectListItem> lstcomplainTypes();
        List<SelectListItem> lstcomplainTypesss();
        List<SelectListItem> lstcomplainSubTypesss(int id);

        List<SelectListItem> lstcomplainpriority();
        #endregion

        #region--------------------District User---------------------------      
        List<SelectListItem> projectlist();
        List<SelectListItem> modulelist();
        List<SelectListItem> tasklist(int regid);
        int GetDistrictId(int regid);
        bool EntryTimeSheet(List<TimesheetModel> Timesheet,int regid,string username);
        List<vw_Timesheet_Dtl> TimesheetDetail(int regid);
        bool DeleteRecord(int id);
        #endregion

        #region--------------------Cluster Head---------------------------      
        List<vw_Timesheet_Dtl> TimesheetDetailforTL(int distid);
        bool EntryTimeSheetTL(List<TimesheetTLModel> Timesheet, int regid);
        List<SelectListItem> GetAllUser(int distid);
        bool AssignTask(List<AssignTaskModel> Timesheet, int regid,string uname);
        List<vw_Task_Dtl> vw_Task_Dtls();
        #endregion

        #region--------------------Project Manager--------------------------      
        bool ApproveRecord(int tid,string remark, int regid);
        bool RejectRecord(int tid,string remark, int regid);

        #endregion

        #region--------------------HR---------------------------      
        List<vw_Timesheet_Dtl> TimesheetDetailforHR();
        List<SelectListItem> userlist();
        List<SelectListItem> emptype();
        List<SelectListItem> userlistss();
        List<SelectListItem> userlistsss(int id,int ulb);
        #endregion
        int managecomplain(ComplainDetail cm);
        int managecomplaintype(ComplainType cm);
        string ComplainNo();
        bool DeleteComplain(int id);
        List<vw_Complain_Dtl> vw_Complain_Dtl();
        bool Resolve(int regid,int id, string remark);
        bool Reject(int regid, int id, string remark);
        Registration RegDetail(int id);
        SubCompainType SubCompainType(int id);
        List<vw_Complain_Sub_Type> vw_Complain_Sub_Types();
        int SaveComSubType(ComplainSubTypeModel cm);
        bool DeleteSubComplain(int id);
        List<SelectListItem> ulblist();
        List<vw_Complain_Dtl> BindComplainData(string mobile);
        ReportModel ReportModel();
        bool ReopenComplain(int regid, int id, string remark);
        bool CloseComplain(int regid, int id, string remark);
        sp_REPORT_COMPLAIN_Year_Result sp_REPORT_COMPLAIN_Year_Result(int year);
        List<sp_Log_Detail_Result> sp_Log_Detail_Result(int id);
        User UserCredntial(int id);
        int changepwd(ChangePasswordModel cpm);
        ComplainDetail GetComplainDetail_edit(int id);
    }
}

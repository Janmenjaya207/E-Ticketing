using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmployeeTimesheet.Models;
using System.Data;
using System.Data.Entity;
using EmployeeTimesheet.Abstract;
using Newtonsoft.Json;
using System.Configuration;

namespace EmployeeTimesheet.Controllers
{
    public class TransactionController : Controller
    {
        private readonly IApplicationRepository applicationRepository;
        public TransactionController(IApplicationRepository appRepository)
        {
            applicationRepository = appRepository;
        }
        public ActionResult Dashboard()
        {
            return View();
        }

        #region----------------------Admin---------------------------
        [HttpGet]
        public ActionResult ManageUser()
        {


            UserModel userModel = new UserModel();


            try
            {
                if (Session["username"] != null && Convert.ToInt32(Session["usertype"]) == 1)
                {
                    userModel.lstulb = applicationRepository.ulblist();

                    userModel.Username = "";
                    userModel.Password = "";
                    int userid = Convert.ToInt32(Session["userid"]);
                    userModel.GetUserType = applicationRepository.usertypelist();
                    userModel.GetDistrict = applicationRepository.districtlist();
                    userModel.lstcomplainTypes = applicationRepository.lstcomplainTypes();
                    ViewBag.data = applicationRepository.lstuserdtl().Where(x => x.UserTypeId != 1 && x.UserTypeId != 10).ToList();
                    return View(userModel);
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["WarningMessage"] = "Something went wrong";
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpPost]
        public ActionResult ManageUser(UserModel um)
        {
            try
            {
                if (Session["username"] != null)
                {
                    um.Createdate = DateTime.Now;
                    um.Createby = Session["username"].ToString();
                    bool data = applicationRepository.manageuser(um);
                    TempData["Message"] = "User created successfully";

                    return RedirectToAction("ManageUser", "Transaction");
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["WarningMessage"] = "Something went wrong";
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpPost]
        public ActionResult DeleteUser(int id)
        {
            var data = applicationRepository.DeleteUser(id);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult CheckUserName(string uname)
        {
            var data = applicationRepository.CheckUserName(uname);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ComplainType(int? id = 0)
        {
            try
            {
                if (Session["username"] != null && Convert.ToInt32(Session["usertype"]) == 1)
                {
                    if (id != 0)
                    {
                        int clientid = Convert.ToInt32(id);
                        int userid = Convert.ToInt32(Session["userid"]);
                        var data = applicationRepository.complainTypes().Where(x => x.ComplainTypeId == id).FirstOrDefault();
                        ViewBag.data = applicationRepository.complainTypes();
                        return View(data);
                    }
                    else
                    {
                        int userid = Convert.ToInt32(Session["userid"]);
                        ViewBag.data = applicationRepository.complainTypes();
                        return View();
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["WaringMessage"] = "Something went wrong";
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpPost]
        public ActionResult ComplainType(ComplainType pm)
        {
            try
            {
                if (Session["username"] != null)
                {
                    ComplainType ComplainType = new ComplainType();
                    int userid = Convert.ToInt32(Session["userid"]);
                    ComplainType.ComplainName = pm.ComplainName;
                    ComplainType.ComplainTypeId = pm.ComplainTypeId;
                    ComplainType.CreatedDate = DateTime.Now;
                    ComplainType.CreatedBy = Session["username"].ToString();
                    ComplainType.IsActive = true;
                    ComplainType.IsDeleted = false;
                    int data = applicationRepository.managecomplaintype(ComplainType);
                    if (data == 1)
                    {
                        TempData["Message"] = "Complain type inserted successfully";
                        return RedirectToAction("ComplainType", "Transaction");
                    }
                    else
                    {
                        TempData["Message"] = "Complain type updated successfully";
                        return RedirectToAction("ComplainType", "Transaction");
                    }

                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["WarningMessage"] = "Something went wrong";
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpPost]
        public ActionResult DeleteComplain(int id)
        {
            return Json(applicationRepository.DeleteComplain(id), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ComplainSubType(int? id = 0)
        {
            ComplainSubTypeModel cm = new ComplainSubTypeModel();
            try
            {
                if (Session["username"] != null && Convert.ToInt32(Session["usertype"]) == 1)
                {
                    if (id != 0)
                    {
                        int sid = Convert.ToInt32(id);
                        var data = applicationRepository.SubCompainType(sid);
                        cm.CompainsubtypeId = data.CompainsubtypeId;
                        cm.ComplainTypeId = data.ComplainTypeId;
                        cm.SubtypeName = data.SubtypeName;
                        cm.lstCompalinType = applicationRepository.lstcomplainTypes();
                        cm.vw_Complain_Sub_Types = applicationRepository.vw_Complain_Sub_Types();
                        return View(cm);
                    }
                    else
                    {
                        cm.lstCompalinType = applicationRepository.lstcomplainTypes();
                        cm.vw_Complain_Sub_Types = applicationRepository.vw_Complain_Sub_Types(); ViewBag.data = applicationRepository.complainTypes();
                        return View(cm);
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["WaringMessage"] = "Something went wrong";
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpPost]
        public ActionResult ComplainSubType(ComplainSubTypeModel cm)
        {
            try
            {
                if (Session["username"] != null)
                {

                    int data = applicationRepository.SaveComSubType(cm);
                    if (data == 1)
                    {
                        TempData["Message"] = "Complain sub type inserted successfully";
                        return RedirectToAction("ComplainSubType", "Transaction");
                    }
                    else
                    {
                        TempData["Message"] = "Complain sub type updated successfully";
                        return RedirectToAction("ComplainSubType", "Transaction");
                    }

                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["WarningMessage"] = "Something went wrong";
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpPost]
        public ActionResult DeleteSubComplain(int id)
        {
            return Json(applicationRepository.DeleteSubComplain(id), JsonRequestBehavior.AllowGet);
        }
        #endregion                           

        #region--------------------Transaction-----------------------
        [HttpGet]
        public ActionResult complain(int id = 0)
        {
            ComplainModel cm = new ComplainModel();

            try
            {
                if (Session["username"] != null && Convert.ToInt32(Session["usertype"]) == 2)
                {
                    if (id == 0)
                    {
                        cm.lstcomplaintype = applicationRepository.lstcomplainTypesss();
                        cm.lstpriority = applicationRepository.lstcomplainpriority();
                        cm.lstAssignee = applicationRepository.userlistsss(0,0);
                        cm.lstulb = applicationRepository.ulblist();
                        cm.lstcomplainsubtype = applicationRepository.lstcomplainSubTypesss(0);
                        cm.Getemptype = applicationRepository.emptype();
                        //ViewBag.data = applicationRepository.lstuserdtl();
                        return View(cm);
                    }
                    else
                    {
                        var data = applicationRepository.GetComplainDetail_edit(id);
                        int ulb = 0;
                        if(data.UlbID!=0&&data.UlbID!=null)
                        {
                            ulb = Convert.ToInt32(data.UlbID);
                        }
                        cm.ComplainDetail = data;
                        cm.lstcomplaintype = applicationRepository.lstcomplainTypesss();
                        cm.lstpriority = applicationRepository.lstcomplainpriority();
                        cm.Getemptype = applicationRepository.emptype();

                        int comptypeid = Convert.ToInt32(data.ComplainTypeId);
                        cm.lstAssignee = applicationRepository.userlistsss(comptypeid, ulb);
                        cm.lstulb = applicationRepository.ulblist();
                        cm.lstcomplainsubtype = applicationRepository.lstcomplainSubTypesss(comptypeid);
                        return View(cm);
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["WarningMessage"] = "Something went wrong";
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpPost]
        public ActionResult Bindassignto(int id,int ulb)
        {
            return Json(applicationRepository.userlistsss(id,ulb), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult complain(ComplainModel cm)
        {
            try
            {
                if (Session["username"] != null)
                {
                    if (cm.ComplainDetail.ComplainId != null && cm.ComplainDetail.ComplainId != 0)
                    {
                        ComplainDetail com = new ComplainDetail();
                        int Regid = Convert.ToInt32(Session["Regid"]);
                        com.ComplainId = cm.ComplainDetail.ComplainId;
                        com.Empid = cm.ComplainDetail.Empid;
                        com.Name = cm.ComplainDetail.Name;
                        com.Email = cm.ComplainDetail.Email;
                        com.Mobile = cm.ComplainDetail.Mobile;
                        com.Address = cm.ComplainDetail.Address;
                        com.Pin = cm.ComplainDetail.Pin;
                        com.Complain_Detail = cm.ComplainDetail.Complain_Detail;
                        com.ComplainTypeId = cm.ComplainDetail.ComplainTypeId;
                        com.PriorityId = cm.ComplainDetail.PriorityId;
                        if (cm.ComplainDetail.AssignedTo != 0 && cm.ComplainDetail.AssignedTo != null)
                        {
                            com.AssignedTo = cm.ComplainDetail.AssignedTo;
                            com.AssignedDate = DateTime.Now;
                            com.ComplainStatus = 2;
                        }
                        else
                        {
                            com.AssignedTo = null;
                            com.AssignedDate = null;
                            com.ComplainStatus = 7;
                        }
                        com.UlbID = cm.ComplainDetail.UlbID;
                        if (cm.ComplainDetail.ComplainSubTypeId != 0)
                        {
                            com.ComplainSubTypeId = cm.ComplainDetail.ComplainSubTypeId;
                        }
                        com.AadharNo = cm.ComplainDetail.AadharNo;                       
                        int data = applicationRepository.managecomplain(com);
                        if (data == 2)
                        {
                            TempData["Message"] = "Complain updated successfully";
                            return RedirectToAction("complain", "Transaction");
                        }
                        else
                        {
                            TempData["Message"] = "Complain registered successfully";
                            return RedirectToAction("complain", "Transaction");
                        }
                    }
                    else
                    {
                        ComplainDetail com = new ComplainDetail();
                        int Regid = Convert.ToInt32(Session["Regid"]);
                        com.Empid = cm.ComplainDetail.Empid;
                        com.CreatedBy = Session["username"].ToString();
                        com.Name = cm.ComplainDetail.Name;
                        com.Email = cm.ComplainDetail.Email;
                        com.Mobile = cm.ComplainDetail.Mobile;
                        com.Address = cm.ComplainDetail.Address;
                        com.Pin = cm.ComplainDetail.Pin;
                        com.Complain_Detail = cm.ComplainDetail.Complain_Detail;
                        com.ComplainTypeId = cm.ComplainDetail.ComplainTypeId;
                        com.PriorityId = cm.ComplainDetail.PriorityId;
                        if (cm.ComplainDetail.AssignedTo != 0 && cm.ComplainDetail.AssignedTo != null)
                        {
                            com.AssignedTo = cm.ComplainDetail.AssignedTo;
                            com.AssignedDate = DateTime.Now;
                            com.ComplainStatus = 2;
                        }
                        else
                        {
                            com.ComplainStatus = 7;
                        }
                        com.UlbID = cm.ComplainDetail.UlbID;
                        if (cm.ComplainDetail.ComplainSubTypeId != 0)
                        {
                            com.ComplainSubTypeId = cm.ComplainDetail.ComplainSubTypeId;
                        }
                        com.AadharNo = cm.ComplainDetail.AadharNo;

                        com.CreatedDate = DateTime.Now;
                        com.IsActive = true;
                        com.IsDeleted = false;
                        com.ReceivedBy = Regid;
                        com.ReceiveDate = DateTime.Now;
                        com.ComplainNo = applicationRepository.ComplainNo();
                        int data = applicationRepository.managecomplain(com);
                        if (data == 1)
                        {
                            TempData["Message"] = "Complain registered successfully";
                            return RedirectToAction("complain", "Transaction");
                        }
                        else
                        {
                            TempData["Message"] = "Complain updated successfully";
                            return RedirectToAction("complain", "Transaction");
                        }
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["WarningMessage"] = "Something went wrong";
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpPost]
        public ActionResult BindComplainData(string id)
        {
            return Json(applicationRepository.BindComplainData(id), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult BindSelectedData(int id)
        {
            var data = applicationRepository.vw_Complain_Dtl().Where(x => x.ComplainId == id).FirstOrDefault();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult complainlist()
        {
            ComplainModel cm = new ComplainModel();

            try
            {
                if (Session["username"] != null && Convert.ToInt32(Session["usertype"]) == 2)
                {
                    cm.lstcomplaintype = applicationRepository.lstcomplainTypesss();
                    cm.lstpriority = applicationRepository.lstcomplainpriority();
                    cm.lstAssignee = applicationRepository.userlist();
                    cm.lstcomplainsubtype = applicationRepository.lstcomplainSubTypesss(0);
                    int Regid = Convert.ToInt32(Session["Regid"]);

                    cm.vw_Complain_Dtls = applicationRepository.vw_Complain_Dtl().Where(x => x.ReceivedBy == Regid).ToList();
                    return View(cm);
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["WarningMessage"] = "Something went wrong";
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpPost]
        public ActionResult BindSubComplain(int id)
        {
            return Json(applicationRepository.lstcomplainSubTypesss(id), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult complainlist(ComplainModel cm)
        {
            try
            {
                if (Session["username"] != null && Convert.ToInt32(Session["usertype"]) == 2)
                {
                    cm.lstcomplaintype = applicationRepository.lstcomplainTypes();
                    cm.lstpriority = applicationRepository.lstcomplainpriority();
                    cm.lstAssignee = applicationRepository.userlist();
                    int Regid = Convert.ToInt32(Session["Regid"]);
                    int comptype = Convert.ToInt32(cm.ComplainDetail.ComplainTypeId);
                    int assign = Convert.ToInt32(cm.ComplainDetail.AssignedTo);
                    int prio = Convert.ToInt32(cm.ComplainDetail.PriorityId);

                    cm.vw_Complain_Dtls = applicationRepository.vw_Complain_Dtl().Where(x => x.ReceivedBy == Regid && x.ComplainTypeId == comptype && x.AssignedTo == assign && x.PriorityId == prio).ToList();

                    return View(cm);
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["WarningMessage"] = "Something went wrong";
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpPost]
        public ActionResult ViewComplain(int id)
        {
            var data = applicationRepository.vw_Complain_Dtl().Where(x => x.ComplainId == id).FirstOrDefault();
            return PartialView("_ComplainDetail", data);
        }
        [HttpGet]
        public ActionResult viewcomplain_exec()
        {
            ComplainModel cm = new ComplainModel();

            try
            {
                if (Session["username"] != null && Convert.ToInt32(Session["usertype"]) == 3)
                {
                    //cm.lstcomplaintype = applicationRepository.lstcomplainTypes();
                    cm.lstpriority = applicationRepository.lstcomplainpriority();
                    cm.lstAssignee = applicationRepository.userlist();
                    int Regid = Convert.ToInt32(Session["Regid"]);
                    int comptypeid = Convert.ToInt32(Session["comptype"]);
                    //cm.vw_Complain_Dtls = applicationRepository.vw_Complain_Dtl().Where(x => x.ComplainTypeId == comptypeid && (x.AssignedTo == Regid || x.AssignedTo == null)).ToList();
                    cm.vw_Complain_Dtls = applicationRepository.vw_Complain_Dtl().Where(x => x.ComplainTypeId == comptypeid && (x.AssignedTo == Regid)).ToList();

                    return View(cm);
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["WarningMessage"] = "Something went wrong";
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpPost]
        public ActionResult viewcomplain_exec(ComplainModel cm)
        {
            try
            {
                if (Session["username"] != null && Convert.ToInt32(Session["usertype"]) == 3)
                {
                    //cm.lstcomplaintype = applicationRepository.lstcomplainTypes();
                    cm.lstpriority = applicationRepository.lstcomplainpriority();
                    cm.lstAssignee = applicationRepository.userlist();
                    int Regid = Convert.ToInt32(Session["Regid"]);
                    int comptypeid = Convert.ToInt32(Session["comptype"]);
                    int prio = Convert.ToInt32(cm.ComplainDetail.PriorityId);

                    //cm.vw_Complain_Dtls = applicationRepository.vw_Complain_Dtl().Where(x => x.ComplainTypeId == comptypeid && (x.AssignedTo == Regid || x.AssignedTo == null) && x.PriorityId == prio).ToList();
                    cm.vw_Complain_Dtls = applicationRepository.vw_Complain_Dtl().Where(x => x.ComplainTypeId == comptypeid && (x.AssignedTo == Regid) && x.PriorityId == prio).ToList();

                    return View(cm);
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["WarningMessage"] = "Something went wrong";
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpGet]
        public ActionResult viewallcomplain_authority()
        {
            ComplainModel cm = new ComplainModel();

            try
            {
                if (Session["username"] != null && Convert.ToInt32(Session["usertype"]) == 4)
                {
                    cm.lstcomplaintype = applicationRepository.lstcomplainTypes();
                    cm.lstpriority = applicationRepository.lstcomplainpriority();
                    cm.lstulb = applicationRepository.ulblist();
                    int Regid = Convert.ToInt32(Session["Regid"]);

                    cm.vw_Complain_Dtls = applicationRepository.vw_Complain_Dtl().ToList();
                    return View(cm);
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["WarningMessage"] = "Something went wrong";
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpPost]
        public ActionResult viewallcomplain_authority(ComplainModel cm)
        {
            try
            {
                if (Session["username"] != null && Convert.ToInt32(Session["usertype"]) == 4)
                {
                    cm.lstcomplaintype = applicationRepository.lstcomplainTypes();
                    cm.lstpriority = applicationRepository.lstcomplainpriority();
                    cm.lstulb = applicationRepository.ulblist();
                    int Regid = Convert.ToInt32(Session["Regid"]);
                    int comptype = Convert.ToInt32(cm.ComplainDetail.ComplainTypeId);
                    int ulb = Convert.ToInt32(cm.ComplainDetail.UlbID);
                    int prio = Convert.ToInt32(cm.ComplainDetail.PriorityId);

                    cm.vw_Complain_Dtls = applicationRepository.vw_Complain_Dtl().Where(x => x.ComplainTypeId == comptype && x.UlbID == ulb && x.PriorityId == prio).ToList();

                    return View(cm);
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["WarningMessage"] = "Something went wrong";
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpGet]
        public ActionResult viewallcomplain_user()
        {
            ComplainModel cm = new ComplainModel();

            try
            {
                if (Session["username"] != null && Convert.ToInt32(Session["usertype"]) == 10)
                {
                    cm.lstcomplaintype = applicationRepository.lstcomplainTypes();
                    cm.lstpriority = applicationRepository.lstcomplainpriority();
                    cm.lstAssignee = applicationRepository.userlist();
                    int Regid = Convert.ToInt32(Session["Regid"]);

                    cm.vw_Complain_Dtls = applicationRepository.vw_Complain_Dtl().Where(x => x.RegId == Regid).ToList();
                    return View(cm);
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["WarningMessage"] = "Something went wrong";
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpPost]
        public ActionResult viewallcomplain_user(ComplainModel cm)
        {
            try
            {
                if (Session["username"] != null && Convert.ToInt32(Session["usertype"]) == 10)
                {
                    cm.lstcomplaintype = applicationRepository.lstcomplainTypes();
                    cm.lstpriority = applicationRepository.lstcomplainpriority();
                    cm.lstAssignee = applicationRepository.userlist();
                    int Regid = Convert.ToInt32(Session["Regid"]);
                    int comptype = Convert.ToInt32(cm.ComplainDetail.ComplainTypeId);
                    int prio = Convert.ToInt32(cm.ComplainDetail.PriorityId);

                    cm.vw_Complain_Dtls = applicationRepository.vw_Complain_Dtl().Where(x => x.ComplainTypeId == comptype && x.PriorityId == prio && x.RegId == Regid).ToList();

                    return View(cm);
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["WarningMessage"] = "Something went wrong";
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpPost]
        public ActionResult ResolveComplain(string id, string remark)
        {
            int Regid = Convert.ToInt32(Session["Regid"]);

            int compid = Convert.ToInt32(id);
            return Json(applicationRepository.Resolve(Regid, compid, remark), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult RejectComplain(string id, string remark)
        {
            int Regid = Convert.ToInt32(Session["Regid"]);

            int compid = Convert.ToInt32(id);
            return Json(applicationRepository.Reject(Regid, compid, remark), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult viewallcomplain_authority_dashboard()
        {
            try
            {
                if (Session["username"] != null && (Convert.ToInt32(Session["usertype"]) == 4 || Convert.ToInt32(Session["usertype"]) == 1))
                {
                    string month = DateTime.Now.ToString("MMMM");
                    int year = DateTime.Now.Year;
                    ViewBag.formonth = month + "-" + year;
                    var data = applicationRepository.ReportModel();
                    return View(data);
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["WarningMessage"] = "Something went wrong";
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpPost]
        public ActionResult ReopenComplain(string id, string remark)
        {
            int Regid = Convert.ToInt32(Session["Regid"]);

            int compid = Convert.ToInt32(id);
            return Json(applicationRepository.ReopenComplain(Regid, compid, remark), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CloseComplain(string id, string remark)
        {
            int Regid = Convert.ToInt32(Session["Regid"]);

            int compid = Convert.ToInt32(id);
            return Json(applicationRepository.CloseComplain(Regid, compid, remark), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult sp_Log_Detail_Result(int id)
        {
            LogModel lm = new LogModel();
            lm.sp_Log_Detail_Results = applicationRepository.sp_Log_Detail_Result(id);

            return PartialView("_LogPartial", lm);
        }
        [HttpGet]
        public ActionResult changepassword()
        {
            try
            {
                if (Session["username"] != null)
                {
                    ChangePasswordModel cpm = new ChangePasswordModel();
                    int Regid = Convert.ToInt32(Session["Regid"]);
                    cpm.regid = Regid;
                    return View(cpm);
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["WarningMessage"] = "Something went wrong";
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpPost]
        public ActionResult changepassword(ChangePasswordModel cpm)
        {
            try
            {
                if (Session["username"] != null)
                {
                    int Regid = Convert.ToInt32(Session["Regid"]);
                    cpm.regid = Regid;
                    if (cpm.newpwd != cpm.confpwd)
                    {
                        TempData["WarningMessage"] = "New password and confirm password not matching";
                        return View(cpm);
                    }
                    else
                    {
                        var data = applicationRepository.UserCredntial(Regid);
                        if (data.Password != cpm.oldpwd)
                        {
                            TempData["WarningMessage"] = "Incorrect old password";
                            return View(cpm);
                        }
                        else if (data.Password == cpm.newpwd)
                        {
                            TempData["WarningMessage"] = "New password should be different than old password";
                            return View(cpm);
                        }
                        else
                        {
                            var cpw = applicationRepository.changepwd(cpm);
                            if (cpw == 1)
                            {
                                TempData["Message"] = "Password changed successfully";
                                return View(cpm);
                            }
                            else
                            {
                                TempData["WarningMessage"] = "Something went wrong";
                                return View(cpm);
                            }
                        }
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["WarningMessage"] = "Something went wrong";
                return RedirectToAction("Login", "Home");
            }
        }
        #endregion
    }
}
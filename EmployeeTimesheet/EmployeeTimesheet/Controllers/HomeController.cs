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
    public class HomeController : Controller
    {
        private readonly IApplicationRepository applicationRepository;

        public HomeController(IApplicationRepository appRepository)
        {
            applicationRepository = appRepository;
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            try
            {
                var data = applicationRepository.CheckLogin(username, password);

                if (data != null&&data.IsDeleted==false)
                {
                    if (data.RegistrationId != null)
                    {
                        int regid = Convert.ToInt32(data.RegistrationId);
                        Session["distid"] = applicationRepository.GetDistrictId(regid);
                        Session["User_name"] = applicationRepository.Username(regid);

                        var regdetail = applicationRepository.RegDetail(regid);
                        if(regdetail!=null)
                        {
                            Session["comptype"] = regdetail.ComplainTypeId;
                        }

                    }
                    else
                    {
                        Session["User_name"] = applicationRepository.Username(0);
                    }
                    Session["userid"] = data.UserId;
                    Session["Regid"] = data.RegistrationId;
                    Session["username"] = data.Username;
                    Session["usertype"] = data.UserTypeId;
                    if (data.UserTypeId == 1 || data.UserTypeId == 4)
                    {
                        return RedirectToAction("viewallcomplain_authority_dashboard", "Transaction");
                    }
                    else
                    {
                        return RedirectToAction("Dashboard", "Transaction");
                    }
                }
                else if (data != null && data.IsDeleted == true)
                {
                    TempData["Message"] = "User does not exist";
                    return View();
                }
                else
                {
                    TempData["Message"] = "Username or password is incorrect";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Something went wrong";
                return RedirectToAction("Login", "Admin");
            }
        }
        [HttpGet]
        public ActionResult index()
        {
            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mail;
using EmployeeTimesheet.Abstract;
using EmployeeTimesheet.Models;
using System.Text;
using System.Net;
using System.IO;
using System.Web.Mvc;

namespace EmployeeTimesheet.Concrete
{
    public class ApplicationRepository : IApplicationRepository
    {
        TimeSheetEntities con = new TimeSheetEntities();

        public User CheckLogin(string username, string password)
        {
            var data = con.Users.Where(x => x.Username == username && x.Password == password).FirstOrDefault();

            return data;
        }
        #region--------------------Admin---------------------------
        public bool manageclient(Client client)
        {
            if (client.ClientId != 0)
            {
                var clientdtl = con.Clients.Where(x => x.ClientId == client.ClientId).FirstOrDefault();
                clientdtl.ClientName = client.ClientName;
                con.SaveChanges();
            }
            else
            {
                con.Clients.Add(client);
                con.SaveChanges();
            }

            return true;
        }
        public List<Client> lstclients()
        {
            var data = con.Clients.Where(x => x.IsActive == true && x.IsDeleted == false).OrderByDescending(x => x.ClientId).ToList();
            return data;
        }
        public Client GetClient(int id)
        {
            var data = con.Clients.Where(x => x.ClientId == id).FirstOrDefault();
            return data;
        }
        public bool DeleteClient(int id)
        {
            var data = con.Clients.Where(x => x.ClientId == id).FirstOrDefault();
            data.IsActive = false;
            data.IsDeleted = true;
            con.SaveChanges();
            return true;
        }
        public List<SelectListItem> clientlist()
        {
            var data = con.Clients.ToList();
            List<SelectListItem> li = new List<SelectListItem>();

            foreach (var item in data)
            {
                li.Add(new SelectListItem
                {
                    Text = item.ClientName,
                    Value = item.ClientId.ToString(),
                });
            }
            return li;
        }
        public List<SelectListItem> usertypelist()
        {
            var data = con.UserTypes.Where(x => x.UserTypeId != 1 && x.UserTypeId != 10).ToList();
            List<SelectListItem> li = new List<SelectListItem>();

            foreach (var item in data)
            {
                li.Add(new SelectListItem
                {
                    Text = item.UserTypeName,
                    Value = item.UserTypeId.ToString(),
                });
            }
            return li;
        }
        public List<SelectListItem> districtlist()
        {
            var data = con.Districts.ToList();
            List<SelectListItem> li = new List<SelectListItem>();

            foreach (var item in data)
            {
                li.Add(new SelectListItem
                {
                    Text = item.DistrictName,
                    Value = item.DistrictId.ToString(),
                });
            }
            return li;
        }
        public List<SelectListItem> lstcomplainTypes()
        {
            var data = con.ComplainTypes.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
            List<SelectListItem> li = new List<SelectListItem>();

            foreach (var item in data)
            {
                li.Add(new SelectListItem
                {
                    Text = item.ComplainName,
                    Value = item.ComplainTypeId.ToString(),
                });
            }
            return li;
        }
        public List<SelectListItem> lstcomplainTypesss()
        {
            var data = con.ComplainTypes.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "Select", Value = "0", });
            foreach (var item in data)
            {
                li.Add(new SelectListItem
                {
                    Text = item.ComplainName,
                    Value = item.ComplainTypeId.ToString(),
                });
            }
            return li;
        }
        public List<SelectListItem> lstcomplainSubTypesss(int id)
        {
            var data = con.SubCompainTypes.Where(x => x.ComplainTypeId == id && x.IsActive == true && x.IsDelete == false).ToList();
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "Select", Value = "0", });
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    li.Add(new SelectListItem
                    {
                        Text = item.SubtypeName,
                        Value = item.CompainsubtypeId.ToString(),
                    });
                }
            }
            return li;
        }
        public List<SelectListItem> lstcomplainpriority()
        {
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "High", Value = "1", });
            li.Add(new SelectListItem { Text = "Medium", Value = "2", });
            li.Add(new SelectListItem { Text = "Low", Value = "3", });
            return li;
        }
        public Project GetProject(int id)
        {
            var data = con.Projects.Where(x => x.ProjectId == id).FirstOrDefault();
            return data;
        }
        public List<vw_Project_Details> GetProjectList()
        {
            var data = con.vw_Project_Details.OrderByDescending(x => x.ProjectId).ToList();
            return data;
        }
        public bool manageproject(Project project)
        {
            if (project.ProjectId != 0)
            {
                var projectdtl = con.Projects.Where(x => x.ProjectId == project.ProjectId).FirstOrDefault();
                projectdtl.ProjectName = project.ProjectName;
                projectdtl.ClientId = project.ClientId;
                projectdtl.Descriotion = project.Descriotion;
                projectdtl.ProjectCode = project.ProjectCode;

                con.SaveChanges();
            }
            else
            {
                con.Projects.Add(project);
                con.SaveChanges();
            }

            return true;
        }
        public bool DeleteProject(int id)
        {
            var data = con.Projects.Where(x => x.ProjectId == id).FirstOrDefault();
            data.IsActive = false;
            data.IsDeleted = true;
            con.SaveChanges();
            return true;
        }
        public bool manageuser(UserModel um)
        {
            Registration reg = new Registration();
            reg.FirstName = um.FirstName;
            reg.LastName = um.LastName;
            reg.DistrictId = um.DistrictId;
            reg.Email = um.Email;
            reg.Contactno = um.Contactno;
            if (um.UserTypeId == 3)
            {
                reg.ComplainTypeId = um.ComplainTypeId;
                reg.Ulb_Id = um.Ulb_Id;
            }
            reg.IsActive = true;
            reg.IsDeleted = false;
            reg.CreatedBy = um.Createby;
            reg.CreatedOn = um.Createdate;
            con.Registrations.Add(reg);
            con.SaveChanges();

            User usr = new User();
            usr.Username = um.Username;
            usr.Password = um.Password;
            usr.RegistrationId = reg.RegistrationId;
            usr.UserTypeId = um.UserTypeId;
            usr.IsActive = true;
            usr.IsDeleted = false;
            usr.CreatedBy = um.Createby;
            usr.CreatedOn = um.Createdate;
            con.Users.Add(usr);
            con.SaveChanges();

            string sub = "Credential detail";
            var body = new StringBuilder();
            body.AppendFormat("Hello, {0}\n", um.FirstName + " " + um.LastName);
            body.AppendLine(@"Please find the below creadential detail for login");
            body.AppendLine("<br/>");
            body.AppendLine("<br/>");
            body.AppendLine(@"Username: " + um.Username);
            body.AppendLine("<br/>");
            body.AppendLine(@"Password: " + um.Password);

            string mailbody = body.ToString();
            SendMail(reg.Email, sub, body.ToString());

            return true;
        }
        public bool DeleteUser(int id)
        {
            var data = con.Registrations.Where(x => x.RegistrationId == id).FirstOrDefault();
            data.IsActive = false;
            data.IsDeleted = true;
            con.SaveChanges();

            var data1 = con.Users.Where(x => x.RegistrationId == id).FirstOrDefault();
            data1.IsActive = false;
            data1.IsDeleted = true;
            con.SaveChanges();

            return true;
        }
        public bool CheckUserName(string uname)
        {
            var data = con.Users.Where(x => x.Username == uname).FirstOrDefault();
            if (data != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string Username(int regid)
        {
            var data = "ADMIN";
            var user = con.Registrations.Where(x => x.RegistrationId == regid).FirstOrDefault();
            if (user != null)
            {
                data = user.FirstName + " " + user.LastName;
            }
            return data;
        }
        public int managecomplaintype(ComplainType ct)
        {
            int data = 0;
            if (ct.ComplainTypeId != 0)
            {
                var clientdtl = con.ComplainTypes.Where(x => x.ComplainTypeId == ct.ComplainTypeId).FirstOrDefault();
                clientdtl.ComplainName = ct.ComplainName;
                con.SaveChanges();
                data = 2;
            }
            else
            {
                con.ComplainTypes.Add(ct);
                con.SaveChanges();
                data = 1;
            }

            return data;
        }
        public List<ComplainType> complainTypes()
        {
            var data = con.ComplainTypes.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
            return data;
        }
        #endregion

        #region-------------District User-------------------------
        public List<SelectListItem> projectlist()
        {
            var data = con.Projects.ToList();
            List<SelectListItem> li = new List<SelectListItem>();

            foreach (var item in data)
            {
                li.Add(new SelectListItem
                {
                    Text = item.ProjectName,
                    Value = item.ProjectId.ToString(),
                });
            }
            return li;
        }
        public List<SelectListItem> modulelist()
        {
            var data = con.ProjectModules.ToList();
            List<SelectListItem> li = new List<SelectListItem>();

            foreach (var item in data)
            {
                li.Add(new SelectListItem
                {
                    Text = item.Module,
                    Value = item.ModuleId.ToString(),
                });
            }
            return li;
        }
        public int GetDistrictId(int regid)
        {
            var data = con.Registrations.Where(x => x.RegistrationId == regid).FirstOrDefault();
            int d = Convert.ToInt32(data.DistrictId);
            return d;
        }
        public bool EntryTimeSheet(List<TimesheetModel> tm, int regid, string username)
        {
            foreach (var item in tm)
            {
                TimeSheetDtl timeSheet = new TimeSheetDtl();
                timeSheet.RegistrationId = regid;
                timeSheet.DistrictId = item.DistrictId;
                timeSheet.ProjectId = item.ProjectId;
                timeSheet.ModuleId = item.ModuleId;
                timeSheet.TransactionCode = item.TransactionCode;
                timeSheet.TransactionName = item.TransactionName;
                timeSheet.TransactionDesc = item.TransactionDesc;
                timeSheet.TranCreationDate = item.TranCreationDate;
                timeSheet.TranPostDate = item.TranPostDate;
                timeSheet.PendingReason = item.PendingReason;
                timeSheet.TaskDetail = item.TaskDetail;
                timeSheet.Comments = item.Comments;
                timeSheet.CreatedBy = username;
                timeSheet.CreatedOn = DateTime.Now;
                timeSheet.IsActive = true;
                timeSheet.IsDeleted = false;

                con.TimeSheetDtls.Add(timeSheet);
                con.SaveChanges();

                //var data = con.TaskDetails.Where(x => x.TransactionCode == item.TransactionCode).FirstOrDefault();
                //data.IsApprived = true;
                //con.SaveChanges();
            }
            return true;
        }
        public List<vw_Timesheet_Dtl> TimesheetDetail(int regid)
        {
            var data = con.vw_Timesheet_Dtl.Where(x => x.RegistrationId == regid).ToList();

            return data;
        }
        public bool DeleteRecord(int id)
        {
            var data = con.TimeSheetDtls.Where(x => x.TimesheetId == id).FirstOrDefault();
            data.IsActive = false;
            data.IsDeleted = true;
            con.SaveChanges();
            return true;
        }
        public List<SelectListItem> tasklist(int regid)
        {
            var data = con.TaskDetails.Where(x => x.RegistrationId == regid && x.IsApprived == null).ToList();
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "Select Task", Value = "0" });
            foreach (var item in data)
            {
                li.Add(new SelectListItem
                {
                    Text = item.TransactionCode,
                    Value = item.TransactionCode.ToString(),
                });
            }
            return li;
        }
        #endregion

        #region--------------------Cluster Head---------------------------      
        public List<vw_Timesheet_Dtl> TimesheetDetailforTL(int distid)
        {
            var data = con.vw_Timesheet_Dtl.Where(x => x.DistrictId == distid).ToList();

            return data;
        }
        public bool EntryTimeSheetTL(List<TimesheetTLModel> Timesheet, int regid)
        {
            foreach (var item in Timesheet)
            {
                TimeSheetDtl timeSheet = con.TimeSheetDtls.Where(x => x.TimesheetId == item.TimesheetId).FirstOrDefault();
                timeSheet.TLId = regid;
                timeSheet.TLRemark = item.Remark;

                con.SaveChanges();
            }

            return true;
        }
        public bool AssignTask(List<AssignTaskModel> Timesheet, int regid, string uname)
        {
            foreach (var item in Timesheet)
            {
                TaskDetail timeSheet = new TaskDetail();
                timeSheet.AssigneeId = regid;
                timeSheet.RegistrationId = item.RegistrationId;
                timeSheet.DistrictId = item.DistrictId;
                timeSheet.ProjectId = item.ProjectId;
                timeSheet.ModuleId = item.ModuleId;
                int data = con.TaskDetails.ToList().Count;
                if (data == 0)
                {
                    timeSheet.TransactionCode = "T00" + 1;
                }
                else
                {
                    int data1 = data + 1;
                    timeSheet.TransactionCode = "T00" + data1;
                }
                timeSheet.TransactionName = item.TransactionName;
                timeSheet.AssignedTaskDetail = item.AssignedTaskDetail;
                timeSheet.TranCreationDate = item.TranCreationDate;
                timeSheet.CreatedBy = uname;
                timeSheet.CreatedOn = DateTime.Now;
                timeSheet.IsActive = true;
                timeSheet.IsDeleted = false;

                con.TaskDetails.Add(timeSheet);
                con.SaveChanges();
            }

            return true;
        }
        public List<vw_Task_Dtl> vw_Task_Dtls()
        {
            var data = con.vw_Task_Dtl.ToList();
            return data;
        }
        #endregion

        #region--------------------Project Manager--------------------------      
        

        #endregion

        #region--------------------HR---------------------------      
        public List<vw_Timesheet_Dtl> TimesheetDetailforHR()
        {
            var data = con.vw_Timesheet_Dtl.ToList();
            return data;
        }
        #endregion

        #region-------------------Mail--------------------------
        public bool SendMail(string to, string subject, string body)
        {
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add(to);
            mail.From = new MailAddress("cimsdev2020@gmail.com");
            mail.Subject = subject;
            string Body = body;
            mail.Body = Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = new System.Net.NetworkCredential("cimsdev2020@gmail.com", "India@12345"); // Enter seders User name and password       
            smtp.EnableSsl = true;
            smtp.Send(mail);

            return true;
        }
        public List<vw_user_detail> lstuserdtl()
        {
            var data = con.vw_user_detail.ToList();
            return data;
        }
        public List<SelectListItem> userlist()
        {
            var data = (from a in con.Registrations.Where(x => x.IsActive == true && x.IsDeleted == false)
                        join b in con.Users.Where(x => x.UserTypeId == 3 && x.IsActive == true && x.IsDeleted == false)
                        on a.RegistrationId equals b.RegistrationId
                        select new
                        {
                            a.RegistrationId,
                            a.FirstName,
                            a.LastName
                        }).ToList();


            List<SelectListItem> li = new List<SelectListItem>();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    li.Add(new SelectListItem
                    {
                        Text = item.FirstName + " " + item.LastName,
                        Value = item.RegistrationId.ToString(),
                    });
                }
            }
            return li;
        }
        public List<SelectListItem> userlistss()
        {
            var data = (from a in con.Registrations.Where(x => x.IsActive == true && x.IsDeleted == false)
                        join b in con.Users.Where(x => x.UserTypeId == 3 && x.IsActive == true && x.IsDeleted == false)
                        on a.RegistrationId equals b.RegistrationId
                        select new
                        {
                            a.RegistrationId,
                            a.FirstName,
                            a.LastName
                        }).ToList();


            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem
            {
                Text = "Select",
                Value = "0",
            });
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    li.Add(new SelectListItem
                    {
                        Text = item.FirstName + " " + item.LastName,
                        Value = item.RegistrationId.ToString(),
                    });
                }
            }
            return li;
        }
        public List<SelectListItem> userlistsss(int id,int ulb)
        {
            var data = (from a in con.Registrations.Where(x => x.ComplainTypeId == id&&x.Ulb_Id==ulb && x.IsActive == true && x.IsDeleted == false)
                        join b in con.Users.Where(x => x.UserTypeId == 3 && x.IsActive == true && x.IsDeleted == false)
                        on a.RegistrationId equals b.RegistrationId
                        select new
                        {
                            a.RegistrationId,
                            a.FirstName,
                            a.LastName
                        }).ToList();

            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem
            {
                Text = "Select",
                Value = "0",
            });
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    li.Add(new SelectListItem
                    {
                        Text = item.FirstName + " " + item.LastName,
                        Value = item.RegistrationId.ToString(),
                    });
                }
            }
            return li;
        }
        public List<SelectListItem> GetAllUser(int distid)
        {
            var data = (from a in con.Registrations.Where(x => x.DistrictId == distid && x.IsActive == true && x.IsDeleted == false)
                        join b in con.Users.Where(x => x.UserTypeId == 2 && x.IsActive == true && x.IsDeleted == false)
                        on a.RegistrationId equals b.RegistrationId
                        select new
                        {
                            a.RegistrationId,
                            a.FirstName,
                            a.LastName
                        }).ToList();


            List<SelectListItem> li = new List<SelectListItem>();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    li.Add(new SelectListItem
                    {
                        Text = item.FirstName + " " + item.LastName,
                        Value = item.RegistrationId.ToString(),
                    });
                }
            }
            return li;
        }
        #endregion
        public int managecomplain(ComplainDetail cm)
        {
            if (cm.ComplainId != 0 && cm.ComplainId != null)
            {
                var com = con.ComplainDetails.Where(x => x.ComplainId == cm.ComplainId).FirstOrDefault();
                com.Name = cm.Name;
                com.Email = cm.Email;
                com.Mobile = cm.Mobile;
                com.Address = cm.Address;
                com.Pin = cm.Pin;
                com.Complain_Detail = cm.Complain_Detail;
                com.ComplainTypeId = cm.ComplainTypeId;
                com.PriorityId = cm.PriorityId;
                com.Empid = cm.Empid;
                com.AssignedTo = cm.AssignedTo;
                com.AssignedDate = cm.AssignedDate;
                com.ComplainStatus = 2;

                com.UlbID = cm.UlbID;
                if (cm.ComplainSubTypeId != 0)
                {
                    com.ComplainSubTypeId = cm.ComplainSubTypeId;
                }
                com.AadharNo = cm.AadharNo;
                con.SaveChanges();

                if (cm.AssignedTo != 0 && cm.AssignedTo != null)
                {
                    var comp = con.ComplainDetails.Where(x => x.ComplainId == cm.ComplainId).FirstOrDefault();
                    if (comp.AssignedTo != cm.AssignedTo)
                    {
                        var reg = con.Registrations.Where(x => x.RegistrationId == cm.AssignedTo).FirstOrDefault();
                        string sub1 = "New ticket raised";
                        var body1 = new StringBuilder();
                        body1.AppendFormat("Dear , {0}\n", reg.FirstName);
                        body1.AppendLine("<br/>");
                        body1.AppendLine("<br/>");
                        body1.AppendLine(@"A ticket has been raised with ticket no: " + cm.ComplainNo);
                        body1.AppendLine("<br/>");

                        string mailbody1 = body1.ToString();
                        SendMail(reg.Email, sub1, body1.ToString());
                    }
                }
                return 2;
            }
            else
            {
                var datasss = con.ComplainDetails.Add(cm);
                con.SaveChanges();

                //var data = con.Users.Where(x => x.Username == cm.Email && x.UserTypeId == 10 && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();

                if (cm.AssignedTo != 0 && cm.AssignedTo != null)
                {
                    var reg = con.Registrations.Where(x => x.RegistrationId == cm.AssignedTo).FirstOrDefault();
                    string sub1 = "New ticket raised";
                    var body1 = new StringBuilder();
                    body1.AppendFormat("Dear , {0}\n", reg.FirstName);
                    body1.AppendLine("<br/>");
                    body1.AppendLine("<br/>");
                    body1.AppendLine(@"A ticket has been raised with ticket no: " + cm.ComplainNo);
                    body1.AppendLine("<br/>");

                    string mailbody1 = body1.ToString();
                    SendMail(reg.Email, sub1, body1.ToString());

                    string sub = "Ticket detail";
                    var body = new StringBuilder();
                    body.AppendFormat("Dear , {0}\n", cm.Name);
                    body.AppendLine("<br/>");
                    body.AppendLine(@"Your ticket has been registred with ticket no: " + cm.ComplainNo);
                    body.AppendLine("<br/>");
                    body.AppendLine("<br/>");

                    string mailbody = body.ToString();
                    SendMail(cm.Email, sub, body.ToString());
                }
                //if (data != null)
                //{
                //    datasss.RegId = data.RegistrationId;
                //    con.SaveChanges();

                //    string sub = "Complain detail with login Credential";
                //    var body = new StringBuilder();
                //    body.AppendFormat("Dear , {0}\n", cm.Name);
                //    body.AppendLine("<br/>");
                //    body.AppendLine(@"Your complain has been registred with Complain no: " + cm.ComplainNo);
                //    body.AppendLine("<br/>");
                //    body.AppendLine(@"Please find the below creadential detail for login and status update");
                //    body.AppendLine("<br/>");
                //    body.AppendLine("<br/>");
                //    //body.AppendLine(@"Username: " + cm.Email);
                //    //body.AppendLine("<br/>");
                //    //body.AppendLine(@"Password: " + data.Password);

                //    string mailbody = body.ToString();
                //    SendMail(cm.Email, sub, body.ToString());

                //    if (cm.AssignedTo != 0 && cm.AssignedTo != null)
                //    {
                //        var reg = con.Registrations.Where(x => x.RegistrationId == cm.AssignedTo).FirstOrDefault();
                //        string sub1 = "New complain raised";
                //        var body1 = new StringBuilder();
                //        body1.AppendFormat("Dear , {0}\n", reg.FirstName);
                //        body1.AppendLine("<br/>");
                //        body1.AppendLine("<br/>");
                //        body1.AppendLine(@"A complain has been raised with Complain no: " + cm.ComplainNo);
                //        body1.AppendLine("<br/>");

                //        string mailbody1 = body1.ToString();
                //        SendMail(reg.Email, sub1, body1.ToString());
                //    }
                //}
                else
                {
                    //Registration reg = new Registration();
                    //reg.FirstName = cm.Name;
                    //reg.Email = cm.Email;
                    //reg.Contactno = cm.Mobile;
                    //reg.IsActive = true;
                    //reg.IsDeleted = false;
                    //reg.CreatedBy = cm.CreatedBy;
                    //reg.CreatedOn = cm.CreatedDate;
                    //con.Registrations.Add(reg);
                    //con.SaveChanges();

                    //User usr = new User();
                    //usr.Username = cm.Email;
                    //usr.Password = GenerateString(6);
                    //usr.RegistrationId = reg.RegistrationId;
                    //usr.UserTypeId = 10;
                    //usr.IsActive = true;
                    //usr.IsDeleted = false;
                    //usr.CreatedBy = cm.CreatedBy;
                    //usr.CreatedOn = cm.CreatedDate;
                    //con.Users.Add(usr);
                    //con.SaveChanges();

                    //datasss.RegId = reg.RegistrationId;
                    //con.SaveChanges();

                    string sub = "Complain detail";
                    var body = new StringBuilder();
                    body.AppendFormat("Dear , {0}\n", cm.Name);
                    body.AppendLine("<br/>");
                    body.AppendLine(@"Your ticket has been registred with ticket no: " + cm.ComplainNo);
                    body.AppendLine("<br/>");
                    body.AppendLine("<br/>");
                    //body.AppendLine(@"Username: " + cm.Email);
                    //body.AppendLine("<br/>");
                    //body.AppendLine(@"Password: " + usr.Password);

                    string mailbody = body.ToString();
                    SendMail(cm.Email, sub, body.ToString());

                    if (cm.AssignedTo != 0 && cm.AssignedTo != null)
                    {
                        var reg1 = con.Registrations.Where(x => x.RegistrationId == cm.AssignedTo).FirstOrDefault();
                        string sub1 = "New ticket raised";
                        var body1 = new StringBuilder();
                        body1.AppendFormat("Dear , {0}\n", reg1.FirstName);
                        body1.AppendLine("<br/>");
                        body1.AppendLine("<br/>");
                        body1.AppendLine(@"A ticket has been raised with ticket no: " + cm.ComplainNo);
                        body1.AppendLine("<br/>");

                        string mailbody1 = body1.ToString();
                        SendMail(reg1.Email, sub1, body1.ToString());
                    }
                }
                return 1;
            }
        }
        public string ComplainNo()
        {
            string data = "";
            var count = con.ComplainDetails.ToList().Count;
            if (count == 0)
            {
                data = "COMP/" + DateTime.Now.Year + "/" + 1;
            }
            else
            {
                count = count + 1;
                data = "COMP/" + DateTime.Now.Year + "/" + count;
            }
            return data;
        }
        public bool DeleteComplain(int id)
        {
            var data = con.ComplainTypes.Where(x => x.ComplainTypeId == id).FirstOrDefault();
            data.IsActive = false;
            data.IsDeleted = true;
            con.SaveChanges();
            return true;
        }
        public List<vw_Complain_Dtl> vw_Complain_Dtl()
        {
            var data = con.vw_Complain_Dtl.ToList();
            return data;
        }        
        public Registration RegDetail(int id)
        {
            var data = con.Registrations.Where(x => x.RegistrationId == id && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
            return data;
        }
        public SubCompainType SubCompainType(int id)
        {
            var data = con.SubCompainTypes.Where(x => x.CompainsubtypeId == id && x.IsActive == true && x.IsDelete == false).FirstOrDefault();
            return data;
        }
        public List<vw_Complain_Sub_Type> vw_Complain_Sub_Types()
        {
            var data = con.vw_Complain_Sub_Type.ToList();
            return data;
        }
        public int SaveComSubType(ComplainSubTypeModel cm)
        {
            var data = con.SubCompainTypes.Where(x => x.CompainsubtypeId == cm.CompainsubtypeId).FirstOrDefault();
            if (data != null)
            {
                data.ComplainTypeId = cm.ComplainTypeId;
                data.SubtypeName = cm.SubtypeName;
                con.SaveChanges();
                return 2;
            }
            else
            {
                SubCompainType st = new SubCompainType();
                st.ComplainTypeId = cm.ComplainTypeId;
                st.SubtypeName = cm.SubtypeName;
                st.IsActive = true;
                st.IsDelete = false;
                con.SubCompainTypes.Add(st);
                con.SaveChanges();
                return 1;
            }
        }
        public bool DeleteSubComplain(int id)
        {
            var data = con.SubCompainTypes.Where(x => x.CompainsubtypeId == id).FirstOrDefault();
            data.IsActive = false;
            data.IsDelete = true;
            con.SaveChanges();
            return true;
        }
        public List<SelectListItem> ulblist()
        {
            var data = con.ULBs.Where(x => x.IsActive == true && x.IsDelete == false).ToList();
            List<SelectListItem> li = new List<SelectListItem>();

            foreach (var item in data)
            {
                li.Add(new SelectListItem
                {
                    Text = item.ULBName,
                    Value = item.UlbID.ToString(),
                });
            }
            return li;
        }
        public List<SelectListItem> emptype()
        {
            var data = con.Emp_Type.ToList();
            List<SelectListItem> li = new List<SelectListItem>();

            foreach (var item in data)
            {
                li.Add(new SelectListItem
                {
                    Text = item.EmpType,
                    Value = item.Empid.ToString(),
                });
            }
            return li;
        }
        public List<vw_Complain_Dtl> BindComplainData(string mobile)
        {
            var data = con.vw_Complain_Dtl.Where(x => x.Mobile == mobile).ToList();
            return data;
        }
        public ReportModel ReportModel()
        {
            ReportModel rm = new ReportModel();
            rm.vw_REPORT_COMPLAINs = con.vw_REPORT_COMPLAIN.ToList();
            var data = con.vw_REPORT_COMPLAIN.ToList();
            rm.total = Convert.ToInt32(data.Sum(x => x.Total));
            rm.resolved = Convert.ToInt32(data.Sum(x => x.Resolved));
            rm.assigned = Convert.ToInt32(data.Sum(x => x.Assigned));
            rm.rejected = Convert.ToInt32(data.Sum(x => x.Rejected));
            rm.reopen = Convert.ToInt32(data.Sum(x => x.Reopen));
            rm.close = Convert.ToInt32(data.Sum(x => x.Close));
            rm.pending = Convert.ToInt32(data.Sum(x => x.Pending));

            rm.vw_REPORT_COMPLAIN_Months = con.vw_REPORT_COMPLAIN_Month.ToList();
            var data1 = con.vw_REPORT_COMPLAIN_Month.ToList();
            rm.mtotal = Convert.ToInt32(data1.Sum(x => x.mTotal));
            rm.mresolved = Convert.ToInt32(data1.Sum(x => x.mResolved));
            rm.massigned = Convert.ToInt32(data1.Sum(x => x.mAssigned));
            rm.mrejected = Convert.ToInt32(data1.Sum(x => x.mRejected));
            rm.mreopen = Convert.ToInt32(data1.Sum(x => x.mReopen));
            rm.mclose = Convert.ToInt32(data1.Sum(x => x.mClose));
            rm.mpending = Convert.ToInt32(data1.Sum(x => x.mPending));
            int year = DateTime.Now.Year;
            rm.sp_REPORT_COMPLAIN_Year_Result = sp_REPORT_COMPLAIN_Year_Result(year);


            var data2 = con.vw_complain_avg_time_calculator.FirstOrDefault();
            rm.high = data2.PriorityHigh;
            rm.medium = data2.PriorityMedium;
            rm.low = data2.PriorityLow;
            rm.highavg = "0";
            rm.mediumavg = "0";
            rm.lowavg = "0";
            if (data2.avgtime_high != null)
            {
                double dh = Convert.ToDouble(data2.avgtime_high) / 60;
                rm.highavg = String.Format("{0:0.00}", dh);
            }
            if (data2.avgtime_med != null)
            {
                double dm = Convert.ToDouble(data2.avgtime_med) / 60;
                rm.mediumavg = String.Format("{0:0.00}", dm);
            }
            if (data2.avgtime_low != null)
            {
                double dl = Convert.ToDouble(data2.avgtime_low) / 60;
                rm.lowavg = String.Format("{0:0.00}", dl);
            }
            return rm;
        }
        public bool ApproveRecord(int tid, string remark, int regid)
        {
            TimeSheetDtl timeSheet = con.TimeSheetDtls.Where(x => x.TimesheetId == tid).FirstOrDefault();
            timeSheet.PMId = regid;
            timeSheet.PMRemark = remark;
            timeSheet.IsApprived = true;

            con.SaveChanges();

            return true;
        }
        public bool RejectRecord(int tid, string remark, int regid)
        {
            TimeSheetDtl timeSheet = con.TimeSheetDtls.Where(x => x.TimesheetId == tid).FirstOrDefault();
            timeSheet.PMId = regid;
            timeSheet.PMRemark = remark;
            timeSheet.IsApprived = false;

            con.SaveChanges();

            return true;
        }
        public bool Resolve(int regid, int id, string remark)
        {
            var data = con.ComplainDetails.Where(x => x.ComplainId == id).FirstOrDefault();
            if (data.AssignedTo == null)
            {
                data.AssignedTo = regid;
                data.AssignedDate = DateTime.Now;
            }
            data.ComplainStatus = 3;
            data.ExecRemark = remark;
            data.ActionDate = DateTime.Now;
            con.SaveChanges();

            ComplainLog cl = new ComplainLog();
            cl.appid = data.ComplainId;
            cl.Regid = regid;
            cl.status = 3;
            cl.remark = remark;
            var reg = con.Users.Where(x => x.RegistrationId == regid).FirstOrDefault();
            if (reg.UserTypeId == 2)
            {
                cl.usertypeid = 2;
            }
            else
            {
                cl.usertypeid = 3;
            }
            cl.actiondate = DateTime.Now;
            con.ComplainLogs.Add(cl);
            con.SaveChanges();

            return true;
        }
        public bool Reject(int regid, int id, string remark)
        {
            var data = con.ComplainDetails.Where(x => x.ComplainId == id).FirstOrDefault();
            if (data.AssignedTo == null)
            {
                data.AssignedTo = regid;
                data.AssignedDate = DateTime.Now;
            }
            data.ComplainStatus = 4;
            data.ExecRemark = remark;
            data.ActionDate = DateTime.Now;
            con.SaveChanges();

            ComplainLog cl = new ComplainLog();
            cl.appid = data.ComplainId;
            cl.Regid = regid;
            cl.status = 4;
            cl.remark = remark;
            var reg = con.Users.Where(x => x.RegistrationId == regid).FirstOrDefault();
            if (reg.UserTypeId == 2)
            {
                cl.usertypeid = 2;
            }
            else
            {
                cl.usertypeid = 3;
            }
            cl.actiondate = DateTime.Now;
            con.ComplainLogs.Add(cl);
            con.SaveChanges();
            return true;
        }
        public bool ReopenComplain(int regid, int id, string remark)
        {
            var data = con.ComplainDetails.Where(x => x.ComplainId == id).FirstOrDefault();
            if (data.AssignedTo == null)
            {
                data.AssignedTo = regid;
                data.AssignedDate = DateTime.Now;
            }
            data.ComplainStatus = 5;
            data.UserRemark = remark;
            data.ActionDate = DateTime.Now;
            con.SaveChanges();

            ComplainLog cl = new ComplainLog();
            cl.appid = data.ComplainId;
            cl.Regid = regid;
            cl.status = 5;
            cl.remark = remark;
            var reg = con.Users.Where(x => x.RegistrationId == regid).FirstOrDefault();
            if (reg.UserTypeId == 2)
            {
                cl.usertypeid = 2;
            }
            else
            {
                cl.usertypeid = 3;
            }
            cl.actiondate = DateTime.Now;
            con.ComplainLogs.Add(cl);
            con.SaveChanges();
            return true;
        }
        public bool CloseComplain(int regid, int id, string remark)
        {
            var data = con.ComplainDetails.Where(x => x.ComplainId == id).FirstOrDefault();
            if (data.AssignedTo == null)
            {
                data.AssignedTo = regid;
                data.AssignedDate = DateTime.Now;
            }
            data.ComplainStatus = 6;
            data.UserRemark = remark;
            data.ActionDate = DateTime.Now;
            con.SaveChanges();

            var regis = con.Registrations.Where(x => x.RegistrationId == regid).FirstOrDefault();
            string sub1 = "Ticket closed";
            var body1 = new StringBuilder();
            body1.AppendFormat("Dear , {0}\n", regis.FirstName);
            body1.AppendLine("<br/>");
            body1.AppendLine("<br/>");
            body1.AppendLine(@"Your ticket no -"+ data.ComplainNo + " has been closed with below remark" );
            body1.AppendLine("<br/>");
            body1.AppendLine("<br/>");
            body1.AppendLine(@"Remark -" + data.ExecRemark + "");

            string mailbody1 = body1.ToString();
            SendMail(regis.Email, sub1, body1.ToString());


            ComplainLog cl = new ComplainLog();
            cl.appid = data.ComplainId;
            cl.Regid = regid;
            cl.status = 6;
            cl.remark = remark;
            var reg = con.Users.Where(x => x.RegistrationId == regid).FirstOrDefault();
            if (reg.UserTypeId == 2)
            {
                cl.usertypeid = 2;
            }
            else
            {
                cl.usertypeid = 3;
            }
            cl.actiondate = DateTime.Now;
            con.ComplainLogs.Add(cl);
            con.SaveChanges();
            return true;
        }
        public sp_REPORT_COMPLAIN_Year_Result sp_REPORT_COMPLAIN_Year_Result(int year)
        {
            var data = con.sp_REPORT_COMPLAIN_Year(year).FirstOrDefault();
            return data;
        }
        public List<sp_Log_Detail_Result> sp_Log_Detail_Result(int id)
        {
            var data = con.sp_Log_Detail(id).ToList();
            return data;
        }
        public string GenerateString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var random = new Random();
            var randomString = new string(Enumerable.Repeat(chars, length)
                                                    .Select(s => s[random.Next(s.Length)]).ToArray());
            return randomString;
        }
        public User UserCredntial(int id)
        {
            var data = con.Users.Where(x => x.RegistrationId == id && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
            return data;
        }
        public int changepwd(ChangePasswordModel cpm)
        {
            var data = con.Users.Where(x => x.RegistrationId == cpm.regid && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
            data.Password = cpm.newpwd;
            con.SaveChanges();
            return 1;
        }
        public ComplainDetail GetComplainDetail_edit(int id)
        {
            var data = con.ComplainDetails.Where(x => x.ComplainId == id && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
            return data;
        }
    }
}
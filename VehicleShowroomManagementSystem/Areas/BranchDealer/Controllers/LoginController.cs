using Common;
using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleShowroomManagementSystem.Models;

namespace VehicleShowroomManagementSystem.Areas.BranchDealer.Controllers
{
    public class LoginController : Controller
    {
        // GET: BranchDealer/Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new DealerDAO();
                var result = dao.Login(model.UserName, Encryptor.MD5Hash(model.Password), true);
                if (result == 1)
                {
                    var employee = dao.GetByUserName(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = employee.UserName;
                    userSession.UserID = employee.ID;
                    userSession.RoleID = employee.RoleID;
                    userSession.Avatar = employee.Avatar;
                    userSession.Name = employee.DealerName;
                  
                    Session.Add(Constants.DEALER_SESSION, userSession);


                    if (model.RememberMe == true)
                    {
                        if (Request.Cookies[model.UserName] != null)
                        {
                            Response.Cookies[model.UserName].Expires = DateTime.Now.AddDays(-1);
                        }

                        HttpCookie rememberAccount = new HttpCookie(model.UserName);
                        rememberAccount["UserName"] = model.UserName;
                        rememberAccount["Password"] = model.Password;
                        rememberAccount.Expires = DateTime.Now.AddDays(30);
                        Response.Cookies.Add(rememberAccount);
                    }

                    return RedirectToAction("Index", "Home");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Not Exist Account...");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Locked Account ...");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Incorrect Password...");
                }
                else if (result == -3)
                {
                    ModelState.AddModelError("", "Not Credential...");
                }
            }
            return View("Index");
        }

        public ActionResult Logout()
        {
            Session[Constants.DEALER_SESSION] = null;           
            return Redirect("/BranchDealer/Login/Index");
        }

        [HttpPost]
        public JsonResult ForgotPassword(string email)
        {
            var model = new EmployeeDAO().GetEmployeeByEmail(email);
            if (model != null)
            {
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Common/template/ForgotPassword.html"));

                content = content.Replace("{{Name}}", model.Name);
                content = content.Replace("{{UserName}}", model.UserName);
                content = content.Replace("{{Password}}", model.Password);
                content = content.Replace("{{Email}}", model.Email);

                new MailHelper().SendMail(model.Email, "[SENSITIVE] Xbox Vehicle Showroom", content);
            }
            else
            {
                return Json(new
                {
                    Status = false
                });
            }

            return Json(new
            {
                Status = true
            });
        }

        public JsonResult FillPassword(string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {

                HttpCookie fillPassword = Request.Cookies[userName];
                if (fillPassword != null)
                {
                    var data = fillPassword["Password"].ToString();
                    return Json(new
                    {
                        Status = true,
                        Data = data
                    }, JsonRequestBehavior.AllowGet);

                }
            }
            return Json(new
            {
                Status = false
            });

        }
    }
}
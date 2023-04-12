using BotDetect.Web.Mvc;
using Common;
using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleShowroomManagementSystem.Models;

namespace VehicleShowroomManagementSystem.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new CustomerDAO();
                var result = dao.Login(model.UserName, Encryptor.MD5Hash(model.Password));
                if (result == 1)
                {
                    Session[Common.Constants.USER_SESSION] = null;
                    var customer = dao.GetCustomerByUserName(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = customer.UserName;
                    userSession.UserID = customer.ID;
                    userSession.Avatar = customer.Avatar;
                    userSession.Name = customer.Name;
                    Session.Add(Constants.USER_SESSION, userSession);

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

                    return Redirect("/");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Not Exist Account...");
                    return View();
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Locked Account...");
                    return View();
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Wrong Password...");
                    return View();
                }
                else if (result == -3)
                {
                    ModelState.AddModelError("", "Not Credential...");
                    return View();
                }
            }
            else
            {
                return View();
            }
            return Redirect("/");
        }

        public ActionResult Logout()
        {
            Session[Common.Constants.USER_SESSION] = null;
            return Redirect("/");
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

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [CaptchaValidationActionFilter("CaptchaCode", "registerCaptcha", "Wrong Confirmation Code !")]
        public ActionResult Register(RegisterModel model,HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                var dao = new CustomerDAO();
                if (dao.CheckUserNameExist(model.UserName))
                {
                    ModelState.AddModelError("", "Existed UserName...");
                    return View();
                }
                else if (dao.CheckEmailExist(model.Email))
                {
                    ModelState.AddModelError("", "Existed Email...");
                    return View();
                }
                else
                {
                    Customer customer = new Customer();
                    if (uploadImage != null)
                    {
                        var fileName = Path.GetFileName(uploadImage.FileName); //getting only file name(ex-ganesh.jpg)  
                        var ext = Path.GetExtension(uploadImage.FileName); //getting the extension(ex-.jpg)  
                        var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };
                        if (allowedExtensions.Contains(ext)) //check what type of extension  
                        {
                            string avatar = "customer" + (dao.GetIDRecent() + 1).ToString() + ext; //appending the name with id                                                                 // store the file inside ~/project folder(Img)  
                            uploadImage.SaveAs(Path.Combine(Server.MapPath("~/Assets/showroom/img/customer/" + avatar)));
                            customer.Avatar = avatar;
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "OOPS Please choose only Image file");
                        return View();
                    }
                    customer.UserName = model.UserName;
                    customer.Password = Common.Encryptor.MD5Hash(model.Password);
                    customer.Name = model.Name;
                    customer.Email = model.Email;
                    customer.Phone = model.Phone;
                    customer.DateOfBirth = model.DateOfBirth;
                    customer.RoleID = Common.Constants.CUSTOMER_GROUP;
                    customer.Status = true;
                    var result = dao.Insert(customer);
                    if (result > 0)
                    {
                        Session[Common.Constants.USER_SESSION] = null;
                        var userSession = new UserLogin();
                        userSession.UserName = customer.UserName;
                        userSession.UserID = customer.ID;
                        userSession.Avatar = customer.Avatar;
                        Session.Add(Constants.USER_SESSION, userSession);
                        TempData["msg"] = "<script>alert('Succesfully');</script>";
                        return RedirectToAction("Login", "Customer");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "OOPS Something went wrong...");
                return View();
            }
            return Redirect("/");
        }

        public ActionResult DashBoard(int pageIndex = 1, int pageSize = 5)
        {
            int totalRecord = 0;
            var customerSession = (Models.UserLogin)Session[Common.Constants.USER_SESSION];
            ViewBag.Order = new OrderDAO().ListOrderByCustomer(customerSession.UserID, ref totalRecord, pageIndex, pageSize);
            ViewBag.Customer = new CustomerDAO().Detail(customerSession.UserID);
            ViewBag.Total = totalRecord;
            ViewBag.Page = pageIndex;
            ViewBag.PageSize = pageSize;
            int maxPage = 5;
            int totalPage = 0;
            totalPage = (int)(Math.Ceiling((decimal)totalRecord / (decimal)pageSize));
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            return View();
        }

        new public ActionResult Profile()
        {
            var customerSession = (Models.UserLogin)Session[Common.Constants.USER_SESSION];
            var model = new CustomerDAO().Detail(customerSession.UserID);
            return View(model);
        }

        [HttpPost]
        new public ActionResult Profile(Customer customer,HttpPostedFileBase uploadImage)
        {
            var customerSession = (Models.UserLogin)Session[Common.Constants.USER_SESSION];
            var model = new CustomerDAO().Detail(customerSession.UserID);
            if (model.UserName != customer.UserName)
            {
                if (new CustomerDAO().CheckUserNameExist(customer.UserName))
                {
                    ModelState.AddModelError("", "OOPS UserName not valid");
                    return View(customer);
                }
            }
            if(model.Email != customer.Email)
            {
                if (new CustomerDAO().CheckEmailExist(customer.Email))
                {
                    ModelState.AddModelError("", "OOPS Email not valid");
                    return View(customer);
                }
            }
                
            if (ModelState.IsValid)
            {
                var dao = new CustomerDAO();
                if (uploadImage != null)
                {
                    var fileName = Path.GetFileName(uploadImage.FileName); //getting only file name(ex-ganesh.jpg)  
                    var ext = Path.GetExtension(uploadImage.FileName); //getting the extension(ex-.jpg)  
                    var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };
                    if (allowedExtensions.Contains(ext)) //check what type of extension  
                    {
                        var filePath = Server.MapPath("~/Assets/showroom/img/customer/" + customer.Avatar);
                        FileInfo fileImage = new FileInfo(filePath);
                        if (fileImage.Exists)
                        {
                            fileImage.Delete();
                        }
                        string avatar = "employee" + customer.ID.ToString() + ext; //appending the name with id                                                                 // store the file inside ~/project folder(Img)  
                        uploadImage.SaveAs(Path.Combine(Server.MapPath("~/Assets/showroom/img/customer/" + avatar)));
                        customer.Avatar = avatar;
                    }
                    else
                    {
                        ModelState.AddModelError("", "OOPS Please choose only Image file");
                        return View(customer);
                    }

                }
                else
                {
                    customer.Avatar = dao.Detail(customer.ID).Avatar;
                }


                var result = dao.Update(customer);

                if (result)
                {                   
                    return RedirectToAction("Profile", "Customer");
                }
                else
                {
                    ModelState.AddModelError("", "OOPS Something went wrong...");
                    return View(customer);
                }


            }
            else
            {
                ModelState.AddModelError("", "OOPS Something went wrong...");
                return View(customer);
            }            
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            var customerSession = (Models.UserLogin)Session[Common.Constants.USER_SESSION];
            if (ModelState.IsValid)
            {
                var customer = new CustomerDAO().Detail(customerSession.UserID);             
                    customer.Password = Common.Encryptor.MD5Hash(model.NewPassword);

                    var rs = new CustomerDAO().Update(customer);
                    if (rs)
                    {                       
                        return RedirectToAction("Profile", "Customer");
                    }
                    else
                    {
                        ModelState.AddModelError("", "OOPS Something went wrong...");
                        return View();
                    }                
            }
            else
            {
                ModelState.AddModelError("", "OOPS Something went wrong...");
                return View();
            }          
        }
    }
}
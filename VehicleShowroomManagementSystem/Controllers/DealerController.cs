using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BotDetect.Web.Mvc;
using Model.DAO;
using Model.EF;
using VehicleShowroomManagementSystem.Models;

namespace VehicleShowroomManagementSystem.Controllers
{
    public class DealerController : Controller
    {
        // GET: Dealer
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [CaptchaValidationActionFilter("CaptchaCode", "registerCaptcha", "Wrong Confirmation Code !")]
        public ActionResult Register(RegisterDealerModel model, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                var dao = new DealerDAO();
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
                    Dealer dealer = new Dealer();
                    if (uploadImage != null)
                    {
                        var fileName = Path.GetFileName(uploadImage.FileName); //getting only file name(ex-ganesh.jpg)  
                        var ext = Path.GetExtension(uploadImage.FileName); //getting the extension(ex-.jpg)  
                        var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };
                        if (allowedExtensions.Contains(ext)) //check what type of extension  
                        {
                            string avatar = "dealer" + (dao.GetIDRecent() + 1).ToString() + ext; //appending the name with id                                                                 // store the file inside ~/project folder(Img)  
                            uploadImage.SaveAs(Path.Combine(Server.MapPath("~/Assets/showroom/img/dealer/" + avatar)));
                            dealer.Avatar = avatar;
                        }
                        else
                        {
                            ModelState.AddModelError("", "OOPS Please choose only Image file...");
                            return View();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Image cannot be blank...");
                        return View();
                    }
                    dealer.UserName = model.UserName;
                    dealer.Password = Common.Encryptor.MD5Hash(model.Password);
                    dealer.DealerName = model.DealerName;
                    dealer.Email = model.Email;
                    dealer.Phone = model.Phone;
                    dealer.RoleID = Common.Constants.DEALER_GROUP;
                    dealer.Status = 0;
                    var result = dao.Insert(dealer);
                    if (result > 0)
                    {
                        TempData["msg"] = "<script>alert('Succesfully');</script>";
                        return Redirect("Register");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "OOPS Something went wrong...");
                return View();
            }                         
                return View();           
        }
    }
}
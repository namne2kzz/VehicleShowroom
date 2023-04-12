using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleShowroomManagementSystem.Models;
using Model.DAO;
using Model.EF;
using System.IO;
using Model.ViewModel;

namespace VehicleShowroomManagementSystem.Areas.BranchDealer.Controllers
{
    public class HomeController : BaseController
    {
        // GET: BranchDealer/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Report_BrandVehicleSell(int month)
        {
            var dealerSession = (Models.UserLogin)Session[Common.Constants.DEALER_SESSION];
            List<BrandVehicleReport> result = new BrandDAO().ReportBrandVehicleSellByDealer(month,dealerSession.UserID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Report_BrandVehicleAvailable()
        {
            var dealerSession = (Models.UserLogin)Session[Common.Constants.DEALER_SESSION];
            List<BrandVehicleReport> result = new BrandDAO().ReportBrandVehicleAvailableByDealer(dealerSession.UserID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public new ActionResult Profile()
        {
            var dealerSession = (Models.UserLogin)Session[Common.Constants.DEALER_SESSION];
            var model = new DealerDAO().Detail(dealerSession.UserID);
            return View(model);
        }

        [HttpPost]
        public new  ActionResult Profile(Dealer dealer, HttpPostedFileBase uploadImage)
        {
            var model = new DealerDAO().Detail(dealer.ID);
            if (model.UserName != dealer.UserName)
            {
                if (new DealerDAO().CheckUserNameExist(dealer.UserName))
                {
                    ModelState.AddModelError("", "OOPS UserName cannot be duplicate...");
                    return View(dealer);
                }
            }
            if (model.Email != dealer.Email)
            {
                if (new DealerDAO().CheckEmailExist(dealer.Email))
                {
                    ModelState.AddModelError("", "OOPS Email cannot be duplicate...");
                    return View(dealer);
                }
            }
            if (ModelState.IsValid)
            {
                var dao = new DealerDAO();
                if (uploadImage != null)
                {
                    var fileName = Path.GetFileName(uploadImage.FileName); //getting only file name(ex-ganesh.jpg)  
                    var ext = Path.GetExtension(uploadImage.FileName); //getting the extension(ex-.jpg)  
                    var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };
                    if (allowedExtensions.Contains(ext)) //check what type of extension  
                    {
                        var filePath = Server.MapPath("~/Assets/showroom/img/dealer/" + dealer.Avatar);
                        FileInfo fileImage = new FileInfo(filePath);
                        if (fileImage.Exists)
                        {
                            fileImage.Delete();
                        }
                        string avatar = "dealer" + dealer.ID.ToString() + ext; //appending the name with id                                                                 // store the file inside ~/project folder(Img)  
                        uploadImage.SaveAs(Path.Combine(Server.MapPath("~/Assets/showroom/img/dealer/" + avatar)));
                        dealer.Avatar = avatar;
                    }
                    else
                    {
                        ModelState.AddModelError("", "OOPS Please choose only Image file...");
                        return View(dealer);
                    }

                }
                else
                {
                    dealer.Avatar = dao.Detail(dealer.ID).Avatar;
                }

                var result = dao.Update(dealer);

                if (result)
                {
                    SetAlert("Update Successfully...", "success");
                    return RedirectToAction("Profile", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "OOPS Something went wrong...");
                    return View(dealer);
                }
            }
            return View("Profile");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            var dealerSession = (Models.UserLogin)Session[Common.Constants.DEALER_SESSION];
            if (ModelState.IsValid)
            {
                var dealer = new DealerDAO().Detail(dealerSession.UserID);
                dealer.Password = Common.Encryptor.MD5Hash(model.NewPassword);
                var rs = new DealerDAO().Update(dealer);
                if (rs)
                {
                    SetAlert("Change Password Successfully...", "success");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "OOPS Something went wrong...");
                    return View();
                }
            }
            return View("ChangePassword");
        }
    }
}
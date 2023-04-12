using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using System.IO;
using Common;

namespace VehicleShowroomManagementSystem.Areas.Showroom.Controllers
{
    public class DealerController : BaseController
    {
        // GET: Showroom/Dealer
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new DealerDAO();
            var model = dao.ListAll(searchString, page, pageSize);
            ViewBag.SearchKeyword = searchString;
            SelectedListStatus();
            return View(model);
        }

        public ActionResult Confirm(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new DealerDAO();
            var model = dao.ListConfirmDealer(searchString, page, pageSize);
            ViewBag.SearchKeyword = searchString;
            SelectedListConfirm();
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {           
            return View();
        }

        [HttpPost]
        public ActionResult Create(Dealer dealer, HttpPostedFileBase uploadImage)
        {
            if (new DealerDAO().CheckUserNameExist(dealer.UserName))
            {
                ModelState.AddModelError("", "OOPS UserName cannot be duplicate...");
               return View();
            }
            if (new DealerDAO().CheckEmailExist(dealer.Email))
            {
                ModelState.AddModelError("", "OOPS Email cannot be duplicate...");
                return View();
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
                        string avatar = "dealer" + (dao.GetIDRecent() + 1).ToString() + ext; //appending the name with id                                                                 // store the file inside ~/project folder(Img)  
                        uploadImage.SaveAs(Path.Combine(Server.MapPath("~/Assets/showroom/img/dealer/" + avatar)));
                        dealer.Avatar = avatar;
                    }
                    else
                    {
                        ModelState.AddModelError("", "OOPS Please choose only Image file");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "OOPS Image cannot be blank...");
                    return View();
                }

                var encryptorMd5Pass = Encryptor.MD5Hash(dealer.Password);
                dealer.Password = encryptorMd5Pass;
                dealer.RoleID = Constants.DEALER_GROUP;
                dealer.Status = 0;
                long id = dao.Insert(dealer);
                if (id > 0)
                {
                    SetAlert("Create Successfully...", "success");
                    return RedirectToAction("Confirm", "Dealer");
                }
                else
                {
                    ModelState.AddModelError("", "OOPS Something went wrong...");
                    return View();
                }


            }           
            return View("Index");

        }

        public ActionResult Edit(long id)
        {
            var dealer = new DealerDAO().Detail(id);          
            return View(dealer);
        }

        [HttpPost]
        public ActionResult Edit(Dealer dealer, HttpPostedFileBase uploadImage)
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
                    return RedirectToAction("Index", "Dealer");
                }
                else
                {
                    ModelState.AddModelError("", "OOPS Something went wrong...");
                    return View(dealer);
                }
            }          
            return View("Index");

        }

        public JsonResult Delete(int id)
        {
            new DealerDAO().Delete(id);
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public ActionResult ChangeStatus(long id, FormCollection confirm)
        {
            var status = int.Parse(confirm["Status"]);
            if (status == -2)
            {
                new DealerDAO().Delete(id);
                SetAlert("Delete Successfully...", "success");
                return Redirect("Confirm");
            }
            var result = new DealerDAO().ChangeStatus(id, status);
            return Redirect("Index");
        }     

        public void SelectedListStatus()
        {
            List<SelectListItem> listStatus = new List<SelectListItem>();
            listStatus.Add(new SelectListItem { Text = "ACTIVE", Value = "1" });
            listStatus.Add(new SelectListItem { Text = "LOCK", Value = "-1" });
            ViewBag.ListStatus = listStatus;
        }

        public void SelectedListConfirm()
        {
            List<SelectListItem> listConfirm = new List<SelectListItem>();
            listConfirm.Add(new SelectListItem { Text = "WAITING CONFIRM", Value = "0" });
            listConfirm.Add(new SelectListItem { Text = "ACTIVE", Value = "1" });
            listConfirm.Add(new SelectListItem { Text = "DENY", Value = "-2" });
            ViewBag.ListConfirm = listConfirm;
        }
    }
}
using Common;
using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VehicleShowroomManagementSystem.Areas.Showroom.Controllers
{
    public class CustomerController : BaseController
    {
        // GET: Showroom/Customer
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new CustomerDAO();
            var model = dao.ListAll(searchString, page, pageSize);
            ViewBag.SearchKeyword = searchString;
            return View(model);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Customer customer, HttpPostedFileBase uploadImage)
        {

            if (new CustomerDAO().CheckUserNameExist(customer.UserName))
            {
                ModelState.AddModelError("", "OOPS UserName cannot be duplicate...");
                return View();
            }
            if (new CustomerDAO().CheckEmailExist(customer.Email))
            {
                ModelState.AddModelError("", "OOPS Email cannot be duplicate...");
                return View();
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
                        string avatar = "customer" + (dao.GetIDRecent() + 1).ToString() + ext; //appending the name with id                                                                 // store the file inside ~/project folder(Img)  
                        uploadImage.SaveAs(Path.Combine(Server.MapPath("~/Assets/showroom/img/customer/" + avatar)));
                        customer.Avatar = avatar;
                    }
                    else
                    {
                        ModelState.AddModelError("", "OOPS Please choose only Image file");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Image cannot be blank...");
                    return View();
                }

                var encryptorMd5Pass = Encryptor.MD5Hash(customer.Password);
                customer.Password = encryptorMd5Pass;
                customer.RoleID = Constants.CUSTOMER_GROUP;
                customer.Status = true;
                long id = dao.Insert(customer);
                if (id > 0)
                {
                    SetAlert("Create Successfully...", "success");
                    return RedirectToAction("Index", "Customer");
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
            var customer = new CustomerDAO().Detail(id);          
            return View(customer);
        }

        [HttpPost]
        public ActionResult Edit(Customer customer, HttpPostedFileBase uploadImage)
        {
            var model = new CustomerDAO().Detail(customer.ID);
            if (model.UserName != customer.UserName)
            {
                if (new CustomerDAO().CheckUserNameExist(customer.UserName))
                {
                    ModelState.AddModelError("", "OOPS UserName cannot be duplicate...");
                    return View(customer);
                }
            }
            if (model.Email != customer.Email)
            {
                if (new CustomerDAO().CheckEmailExist(customer.Email))
                {
                    ModelState.AddModelError("", "OOPS Email cannot be duplicate...");
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
                    SetAlert("Update Successfully...", "success");
                    return RedirectToAction("Index", "Customer");
                }
                else
                {
                    ModelState.AddModelError("", "OOPS Something went wrong...");
                    return View(customer);
                }
            }          
            return View("Index");

        }

        public JsonResult Delete(int id)
        {
            new CustomerDAO().Delete(id);
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new CustomerDAO().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }      
    }
}
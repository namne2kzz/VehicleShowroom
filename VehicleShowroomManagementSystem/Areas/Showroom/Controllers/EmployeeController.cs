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
    public class EmployeeController : BaseController
    {
        // GET: Admin/User

        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new EmployeeDAO();
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
        public ActionResult Create(Employee employee, HttpPostedFileBase uploadImage)
        {           
            if (new EmployeeDAO().CheckUserNameExist(employee.UserName))
            {
                ModelState.AddModelError("", "OOPS UserName cannot be duplicate...");
                return View();
            }
            if (new EmployeeDAO().CheckEmailExist(employee.Email))
            {
                ModelState.AddModelError("", "OOPS Email cannot be duplicate...");
                return View();
            }
            if (ModelState.IsValid)
            {
                var dao = new EmployeeDAO();
                if (uploadImage != null)
                {
                    var fileName = Path.GetFileName(uploadImage.FileName); //getting only file name(ex-ganesh.jpg)  
                    var ext = Path.GetExtension(uploadImage.FileName); //getting the extension(ex-.jpg)  
                    var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };
                    if (allowedExtensions.Contains(ext)) //check what type of extension  
                    {
                        string avatar = "employee" + (dao.GetIDRecent() + 1).ToString() + ext; //appending the name with id                                                                 // store the file inside ~/project folder(Img)  
                        uploadImage.SaveAs(Path.Combine(Server.MapPath("~/Assets/showroom/img/employee/" + avatar)));
                        employee.Avatar = avatar;
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

                var encryptorMd5Pass = Encryptor.MD5Hash(employee.Password);
                employee.Password = encryptorMd5Pass;
                employee.RoleID = Constants.SHOWROOM_GROUP;
                employee.Status = true;
                long id = dao.Insert(employee);
                if (id > 0)
                {
                    SetAlert("Create Successfully...", "success");
                    return Redirect("Index");
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
            var employee = new EmployeeDAO().Detail(id);                
            return View(employee);
        }

        [HttpPost]
        public ActionResult Edit(Employee employee, HttpPostedFileBase uploadImage)
        {
            var model = new EmployeeDAO().Detail(employee.ID);
            if (model.UserName != employee.UserName)
            {
                if (new EmployeeDAO().CheckUserNameExist(employee.UserName))
                {
                    ModelState.AddModelError("", "OOPS UserName cannot be duplicate...");
                    return View(employee);
                }
            }
            if (model.Email != employee.Email)
            {
                if (new EmployeeDAO().CheckEmailExist(employee.Email))
                {
                    ModelState.AddModelError("", "OOPS Email cannot be duplicate...");
                    return View(employee);
                }
            }          
            if (ModelState.IsValid)
            {
                var dao = new EmployeeDAO();
                if (uploadImage != null)
                {
                    var fileName = Path.GetFileName(uploadImage.FileName); //getting only file name(ex-ganesh.jpg)  
                    var ext = Path.GetExtension(uploadImage.FileName); //getting the extension(ex-.jpg)  
                    var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };
                    if (allowedExtensions.Contains(ext)) //check what type of extension  
                    {
                        var filePath = Server.MapPath("~/Assets/showroom/img/employee/" + employee.Avatar);
                        FileInfo fileImage = new FileInfo(filePath);
                        if (fileImage.Exists)
                        {
                            fileImage.Delete();
                        }
                        string avatar = "employee" + employee.ID.ToString() + ext; //appending the name with id                                                                 // store the file inside ~/project folder(Img)  
                        uploadImage.SaveAs(Path.Combine(Server.MapPath("~/Assets/showroom/img/employee/" + avatar)));
                        employee.Avatar = avatar;
                    }
                    else
                    {
                        ModelState.AddModelError("", "OOPS Please choose only Image file");
                        return View(employee);
                    }

                }
                else
                {
                    employee.Avatar = dao.Detail(employee.ID).Avatar;
                }
              

                var result = dao.Update(employee);

                if (result)
                {
                    SetAlert("Update Successfully...", "success");
                    return RedirectToAction("Index", "Employee");
                }
                else
                {
                    ModelState.AddModelError("", "OOPS Something went wrong...");
                    return View(employee);
                }
            }
           
            return View("Index");

        }

        public JsonResult Delete(int id)
        {
            new EmployeeDAO().Delete(id);


            return Json(new
            {
                status = true
            });
        }

        [HttpPost]

        public JsonResult ChangeStatus(long id)
        {
            var result = new EmployeeDAO().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

      
             
    }
}
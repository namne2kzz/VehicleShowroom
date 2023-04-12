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
    public class BrandController : BaseController
    {
        // GET: Showroom/Brand
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new BrandDAO();
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
        public ActionResult Create(Brand brand, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                var dao = new BrandDAO();
                if (uploadImage != null)
                {
                    var fileName = Path.GetFileName(uploadImage.FileName); //getting only file name(ex-ganesh.jpg)  
                    var ext = Path.GetExtension(uploadImage.FileName); //getting the extension(ex-.jpg)  
                    var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };
                    if (allowedExtensions.Contains(ext)) //check what type of extension  
                    {
                        string image = "brand" + (dao.GetIDRecent() + 1).ToString() + ext; //appending the name with id                                                                 // store the file inside ~/project folder(Img)  
                        uploadImage.SaveAs(Path.Combine(Server.MapPath("~/Assets/showroom/img/brand/" + image)));
                        brand.Image = image;
                    }
                }
                else
                {
                    ModelState.AddModelError("", "OOPS Please choose only Image file...");
                    return View();
                }
                brand.Status = true;
                long id = dao.Insert(brand);
                if (id > 0)
                {
                    SetAlert("Create Successfully...", "success");
                    return RedirectToAction("Index", "Brand");
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
            var brand = new BrandDAO().Detail(id);          
            return View(brand);
        }

        [HttpPost]
        public ActionResult Edit(Brand brand, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                var dao = new BrandDAO();
                if (uploadImage != null)
                {
                    var fileName = Path.GetFileName(uploadImage.FileName); //getting only file name(ex-ganesh.jpg)  
                    var ext = Path.GetExtension(uploadImage.FileName); //getting the extension(ex-.jpg)  
                    var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };
                    if (allowedExtensions.Contains(ext)) //check what type of extension  
                    {
                        var filePath = Server.MapPath("~/Assets/showroom/img/brand/" + brand.Image);
                        FileInfo fileImage = new FileInfo(filePath);
                        if (fileImage.Exists)
                        {
                            fileImage.Delete();
                        }
                        string image = "brand" + brand.ID.ToString() + ext; //appending the name with id              
                        uploadImage.SaveAs(Path.Combine(Server.MapPath("~/Assets/showroom/img/brand/" + image)));
                        brand.Image = image;
                    }
                    else
                    {
                        ModelState.AddModelError("", "OOPS Please choose only Image file...");
                        return View(brand);
                    }
                }
                else
                {
                    brand.Image = dao.Detail(brand.ID).Image;
                }
                var result = dao.Update(brand);

                if (result)
                {
                    SetAlert("Update Successfully...", "success");
                    return RedirectToAction("Index", "Brand");
                }
                else
                {
                    ModelState.AddModelError("", "OOPS Something went wrong...");
                    return View(brand);
                }
            }
          
            return View("Index");
        }

        public JsonResult Delete(int id)
        {
            new BrandDAO().Delete(id);
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new BrandDAO().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
       
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DAO;
using Model.EF;

namespace VehicleShowroomManagementSystem.Areas.Showroom.Controllers
{
    public class SlideController : BaseController
    {
        // GET: Showroom/Slide
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new SlideDAO();
            var model = dao.ListAll(searchString, page, pageSize);
            ViewBag.SearchKeyword = searchString;
            return View(model);
        }
        [HttpGet]
        public ActionResult Create()
        {
            SelectListType();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Slide slide, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                var dao = new SlideDAO();
                if (uploadImage != null)
                {
                    var fileName = Path.GetFileName(uploadImage.FileName); //getting only file name(ex-ganesh.jpg)  
                    var ext = Path.GetExtension(uploadImage.FileName); //getting the extension(ex-.jpg)  
                    var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };
                    if (allowedExtensions.Contains(ext)) //check what type of extension  
                    {
                        string slideImage = "slide" + (dao.GetIDRecent() + 1).ToString() + ext; //appending the name with id                                                                 // store the file inside ~/project folder(Img)  
                        uploadImage.SaveAs(Path.Combine(Server.MapPath("~/Assets/showroom/img/slide/" + slideImage)));
                        slide.Image = slideImage;
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
                slide.Status = true;
                long id = dao.Insert(slide);
                if (id > 0)
                {
                    SetAlert("Create Successfully...", "success");
                    return RedirectToAction("Index", "Slide");
                }
                else
                {
                    ModelState.AddModelError("", "OOPS Something went wrong...");
                    return View();
                }
            }
            SelectListType();
            return View("Index");

        }

        public ActionResult Edit(long id)
        {
            var slide = new SlideDAO().Detail(id);
            SelectListType(slide.Type);
            return View(slide);
        }

        [HttpPost]
        public ActionResult Edit(Slide slide, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                var dao = new SlideDAO();
                if (uploadImage != null)
                {
                    var fileName = Path.GetFileName(uploadImage.FileName); //getting only file name(ex-ganesh.jpg)  
                    var ext = Path.GetExtension(uploadImage.FileName); //getting the extension(ex-.jpg)  
                    var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };
                    if (allowedExtensions.Contains(ext)) //check what type of extension  
                    {
                        var filePath = Server.MapPath("~/Assets/showroom/img/slide/" + slide.Image);
                        FileInfo fileImage = new FileInfo(filePath);
                        if (fileImage.Exists)
                        {
                            fileImage.Delete();
                        }
                        string slideImage = "slide" + slide.ID.ToString() + ext; //appending the name with id                                                                 // store the file inside ~/project folder(Img)  
                        uploadImage.SaveAs(Path.Combine(Server.MapPath("~/Assets/showroom/img/slide/" + slideImage)));
                        slide.Image = slideImage;
                    }
                    else
                    {
                        ModelState.AddModelError("", "OOPS Please choose only Image file...");
                        return View(slide);
                    }
                }
                else
                {
                    slide.Image = dao.Detail(slide.ID).Image;
                }             

                var result = dao.Update(slide);

                if (result)
                {
                    SetAlert("Update Successfully...", "success");
                    return RedirectToAction("Index", "Slide");
                }
                else
                {
                    ModelState.AddModelError("", "OOPS Something went wrong...");
                    return View(slide);
                }
            }
            SelectListType(slide.Type);
            return View("Index");

        }

        public JsonResult Delete(int id)
        {
            new SlideDAO().Delete(id);
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new SlideDAO().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        public void SelectListType(string sltedType = null)
        {
            List<SelectListItem> listType = new List<SelectListItem>();
            listType.Add(new SelectListItem { Text = "MAIN", Value = "MAIN" });
            listType.Add(new SelectListItem { Text = "EXTRA", Value = "EXTRA" });          
            ViewBag.ListType = new SelectList(listType, "Value", "Text", sltedType);
        }

       
    }
}
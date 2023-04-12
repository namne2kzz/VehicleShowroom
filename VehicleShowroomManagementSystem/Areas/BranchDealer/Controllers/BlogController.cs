using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VehicleShowroomManagementSystem.Areas.BranchDealer.Controllers
{
    public class BlogController : BaseController
    {
        // GET: BranchDealer/Blog
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dealerSession = (Models.UserLogin)Session[Common.Constants.DEALER_SESSION];
            var dao = new BlogDAO();
            var model = dao.ListAll(dealerSession.UserID, searchString, page, pageSize);
            ViewBag.SearchKeyword = searchString;                     
            return View(model);
        }      

        public ActionResult Confirm(string searchString, int page = 1, int pageSize = 10)
        {
            var dealerSession = (Models.UserLogin)Session[Common.Constants.DEALER_SESSION];
            var dao = new BlogDAO();
            var model = dao.ListConfirmBlog(dealerSession.UserID, searchString, page, pageSize);
            ViewBag.SearchKeyword = searchString;                   
            return View(model);
        }       

        [HttpGet]
        public ActionResult Create()
        {             
            return View();
        }

        [HttpPost]
        public ActionResult Create(Blog blog, HttpPostedFileBase uploadImage)
        {
            var dealerSession = (Models.UserLogin)Session[Common.Constants.DEALER_SESSION];
            if (ModelState.IsValid)
            {
                var dao = new BlogDAO();
                if (uploadImage != null)
                {
                    var fileName = Path.GetFileName(uploadImage.FileName); //getting only file name(ex-ganesh.jpg)  
                    var ext = Path.GetExtension(uploadImage.FileName); //getting the extension(ex-.jpg)  
                    var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };
                    if (allowedExtensions.Contains(ext)) //check what type of extension  
                    {
                        string image = "blog" + (dao.GetIDRecent() + 1).ToString() + ext; //appending the name with id                                                                 // store the file inside ~/project folder(Img)  
                        uploadImage.SaveAs(Path.Combine(Server.MapPath("~/Assets/showroom/img/blog/" + image)));
                        blog.Image = image;
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
                blog.DealerID = dealerSession.UserID;
                blog.CreatedDate = DateTime.Now;
                blog.Status = 0;
                long id = dao.Insert(blog);
                if (id > 0)
                {
                    SetAlert("Create Successfully...", "success");
                    return RedirectToAction("Confirm", "Blog");
                }
                else
                {
                    ModelState.AddModelError("", "OOPS Something went wrong...");
                    return View();
                }
            }
            else
            {              
                return View();
            }

          
        }

        public ActionResult Edit(long id)
        {
            var blog = new BlogDAO().Detail(id);           
            return View(blog);
        }

        [HttpPost]
        public ActionResult Edit(Blog blog, HttpPostedFileBase uploadImage)
        {
            var dealerSession = (Models.UserLogin)Session[Common.Constants.DEALER_SESSION];
            if (ModelState.IsValid)
            {
                var dao = new BlogDAO();
                if (uploadImage != null)
                {
                    var fileName = Path.GetFileName(uploadImage.FileName); //getting only file name(ex-ganesh.jpg)  
                    var ext = Path.GetExtension(uploadImage.FileName); //getting the extension(ex-.jpg)  
                    var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };
                    if (allowedExtensions.Contains(ext)) //check what type of extension  
                    {
                        var filePath = Server.MapPath("~/Assets/showroom/img/blog/" + blog.Image);
                        FileInfo fileImage = new FileInfo(filePath);
                        if (fileImage.Exists)
                        {
                            fileImage.Delete();
                        }
                        string image = "blog" + blog.ID.ToString() + ext; //appending the name with id              
                        uploadImage.SaveAs(Path.Combine(Server.MapPath("~/Assets/showroom/img/blog/" + image)));
                        blog.Image = image;
                    }
                    else
                    {
                        ModelState.AddModelError("", "OOPS Please choose only Image file...");
                        return View(blog);
                    }

                }
                else
                {
                    blog.Image = dao.Detail(blog.ID).Image;
                }
                var result = dao.Update(blog);

                if (result)
                {
                    SetAlert("Update Successfully...", "success");
                    return RedirectToAction("Index", "Blog");
                }
                else
                {
                    ModelState.AddModelError("", "OOPS Something went wrong...");
                    return View(blog);
                }
            }
            else
            {               
                return View(blog);
            }
        }

        public JsonResult Delete(int id)
        {
            new BlogDAO().Delete(id);
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
                new BlogDAO().Delete(id);
                SetAlert("Delete Successfully...", "success");
                return Redirect("Confirm");
            }
            var result = new BlogDAO().ChangeStatus(id, status);
            return Redirect("Index");
        }    
      
    }
}
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
    public class BlogController : BaseController
    {
        // GET: Showroom/Blog
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new BlogDAO();
            var model = dao.ListAll(null, searchString, page, pageSize);
            ViewBag.SearchKeyword = searchString;
            SelectedListDealer();
            SelectedListStatus();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(FormCollection form, string searchString, int page = 1, int pageSize = 10)
        {
            string dealerID = form["DealerID"];
            var dao = new BlogDAO();
            if(!dealerID.Equals(""))
            {
                IEnumerable<Blog> model = dao.ListAll(long.Parse(dealerID), searchString, page, pageSize);
                ViewBag.SearchKeyword = searchString;
                SelectedListDealer(long.Parse(dealerID));
                SelectedListStatus();
                return View(model);
            }
            else
            {
                IEnumerable<Blog> model = dao.ListAll(null,searchString, page, pageSize);
                ViewBag.SearchKeyword = searchString;
                SelectedListDealer();
                SelectedListStatus();
                return View(model);
            }
           
        }

        public ActionResult Confirm(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new BlogDAO();
            IEnumerable<Blog> model = dao.ListConfirmBlog(null, searchString, page, pageSize);
            ViewBag.SearchKeyword = searchString;
            var dealer = new DealerDAO().ListAll();
            ViewBag.Dealer = new SelectList(dealer, "ID", "DealerName");
            SelectedListDealer();
            SelectedListConfirm();
            return View(model);
        }

        [HttpPost]
        public ActionResult Confirm(FormCollection form, string searchString, int page = 1, int pageSize = 10)
        {
            string dealerID = form["DealerID"];
            var dao = new BlogDAO();
            if (!dealerID.Equals(""))
            {
                IEnumerable<Blog> model = dao.ListConfirmBlog(long.Parse(dealerID), searchString, page, pageSize);
                ViewBag.SearchKeyword = searchString;
                SelectedListDealer(long.Parse(dealerID));
                SelectedListConfirm();
                return View(model);
            }
            else
            {
                IEnumerable<Blog> model = dao.ListConfirmBlog(null, searchString, page, pageSize);
                ViewBag.SearchKeyword = searchString;
                SelectedListDealer(long.Parse(dealerID));
                SelectedListConfirm();
                return View(model);
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
                        ModelState.AddModelError("", "OOPS Please choose only Image file");
                        return View();
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
                    return View();
                }
            }          
            return View("Index");

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

        public void SelectedListDealer(long? sltedDealer=null)
        {
            var dealer = new DealerDAO().ListAll();
            ViewBag.Dealer = new SelectList(dealer, "ID", "DealerName",sltedDealer);
        }
    }
}
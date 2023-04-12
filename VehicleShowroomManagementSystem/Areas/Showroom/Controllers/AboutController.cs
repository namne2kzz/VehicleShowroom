using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DAO;
using Model.EF;

namespace VehicleShowroomManagementSystem.Areas.Showroom.Controllers
{
    public class AboutController : BaseController
    {
        // GET: Showroom/About
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new AboutDAO();
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
        public ActionResult Create(About about)
        {
            if (ModelState.IsValid)
            {
                var dao = new AboutDAO();             
                about.Status = true;
                long id = dao.Insert(about);
                if (id > 0)
                {
                    SetAlert("Create Successfully...", "success");
                    return RedirectToAction("Index", "About");
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
            var about = new AboutDAO().Detail(id);         
            return View(about);
        }

        [HttpPost]
        public ActionResult Edit(About about)
        {
            if (ModelState.IsValid)
            {
                var dao = new AboutDAO();

                var result = dao.Update(about);

                if (result)
                {
                    SetAlert("Update Successfully...", "success");
                    return RedirectToAction("Index", "About");
                }
                else
                {
                    ModelState.AddModelError("", "OOPS Something went wrong...");
                    return View(about);
                }
            }      
            return View("Index");

        }

        public JsonResult Delete(int id)
        {
            new AboutDAO().Delete(id);
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new AboutDAO().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }      
    }
}
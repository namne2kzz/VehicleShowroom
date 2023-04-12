using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VehicleShowroomManagementSystem.Areas.Showroom.Controllers
{
    public class MandatoryCostController : BaseController
    {
        // GET: Showroom/MandatoryCost
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new MandatoryCostDAO();
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
        public ActionResult Create(MandatoryCost mandatoryCost)
        {
            if (ModelState.IsValid)
            {
                var dao = new MandatoryCostDAO();              
                mandatoryCost.Status = true;
                long id = dao.Insert(mandatoryCost);
                if (id > 0)
                {
                    SetAlert("Create Successfully...", "success");
                    return RedirectToAction("Index", "MandatoryCost");
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
            var mandatoryCost = new MandatoryCostDAO().Detail(id);          
            return View(mandatoryCost);
        }

        [HttpPost]
        public ActionResult Edit(MandatoryCost mandatoryCost)
        {
            if (ModelState.IsValid)
            {
                var dao = new MandatoryCostDAO();

                var result = dao.Update(mandatoryCost);

                if (result)
                {
                    SetAlert("Update Successfully...", "success");
                    return RedirectToAction("Index", "MandatoryCost");
                }
                else
                {
                    ModelState.AddModelError("", "OOPS Something went wrong...");
                    return View(mandatoryCost);
                }
            }            
            return View("Index");

        }

        public JsonResult Delete(int id)
        {
            new MandatoryCostDAO().Delete(id);
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new MandatoryCostDAO().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

       
    }
}
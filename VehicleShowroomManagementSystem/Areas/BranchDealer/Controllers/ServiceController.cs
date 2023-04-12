using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DAO;
using Model.EF;

namespace VehicleShowroomManagementSystem.Areas.BranchDealer.Controllers
{
    public class ServiceController : BaseController
    {
        // GET: BranchDealer/Service
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dealerSession = (Models.UserLogin)Session[Common.Constants.DEALER_SESSION];
            var dao = new ServiceDAO();
            var model = dao.ListAll(dealerSession.UserID,searchString, page, pageSize);
            ViewBag.SearchKeyword = searchString;
            return View(model);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Service service)
        {
            var dealerSession = (Models.UserLogin)Session[Common.Constants.DEALER_SESSION];
            if (ModelState.IsValid)
            {
                var dao = new ServiceDAO();
                service.DealerID = dealerSession.UserID;
                service.Status = true;
                long id = dao.Insert(service);
                if (id > 0)
                {
                    SetAlert("Create Successfully...", "success");
                    return RedirectToAction("Index", "Service");
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
            var service = new ServiceDAO().Detail(id);
            return View(service);
        }

        [HttpPost]
        public ActionResult Edit(Service service)
        {
            if (ModelState.IsValid)
            {
                var dao = new ServiceDAO();

                var result = dao.Update(service);

                if (result)
                {
                    SetAlert("Update Successfully...", "success");
                    return RedirectToAction("Index", "Service");
                }
                else
                {
                    ModelState.AddModelError("", "OOPS Something went wrong...");
                    return View(service);
                }
            }
            else
            {             
                return View(service);
            }
        }

        public JsonResult Delete(int id)
        {
            new ServiceDAO().Delete(id);
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new ServiceDAO().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}
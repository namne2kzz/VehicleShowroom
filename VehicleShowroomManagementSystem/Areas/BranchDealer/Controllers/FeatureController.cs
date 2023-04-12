using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DAO;
using Model.EF;

namespace VehicleShowroomManagementSystem.Areas.BranchDealer.Controllers
{
    public class FeatureController : BaseController
    {
        // GET: BranchDealer/Feature
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dealerSession = (Models.UserLogin)Session[Common.Constants.DEALER_SESSION];
            var dao = new FeatureDAO();
            var model = dao.ListAll(dealerSession.UserID, searchString, page, pageSize);
            ViewBag.SearchKeyword = searchString;
            return View(model);
        }
        [HttpGet]
        public ActionResult Create()
        {
            SelectlistVehicle();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Feature feature)
        {
            if (ModelState.IsValid)
            {
                
                var dao = new FeatureDAO();
                if (dao.CheckFeatureVehicle(feature.VehicleID))
                {
                    ModelState.AddModelError("", "OOPS Vehicle cannot add Feature");
                    SelectlistVehicle();
                    return View();
                }
                if (feature.ExpiredDate < feature.StartedDate)
                {
                    ModelState.AddModelError("", "OOPS Time is not valid");
                    SelectlistVehicle();
                    return View();
                }
                feature.Status = true;
                long id = dao.Insert(feature);
                if (id > 0)
                {
                    SetAlert("Create Successfully...", "success");
                    return RedirectToAction("Index", "Feature");
                }
                else
                {
                    ModelState.AddModelError("", "OOPS Something went wrong...");
                    SelectlistVehicle();
                    return View();
                }
            }
            else
            {
                SelectlistVehicle();
                return View();
            }
            
           

        }

        public ActionResult Edit(long id)
        {
            var feature = new FeatureDAO().Detail(id);
            SelectlistVehicle(feature.VehicleID);
            return View(feature);
        }

        [HttpPost]
        public ActionResult Edit(Feature feature)
        {

            if (ModelState.IsValid)
            {
                var dao = new FeatureDAO();
                if (feature.ExpiredDate < feature.StartedDate)
                {
                    ModelState.AddModelError("", "OOPS Time is not valid");
                    SelectlistVehicle(feature.VehicleID);
                    return View(feature);
                }
                var result = dao.Update(feature);

                if (result)
                {
                    SetAlert("Update Successfully...", "success");
                    return RedirectToAction("Index", "Feature");
                }
                else
                {
                    ModelState.AddModelError("", "OOPS Something went wrong...");
                    SelectlistVehicle(feature.VehicleID);
                    return View(feature);
                }
            }
            else
            {
                SelectlistVehicle(feature.VehicleID);
                return View(feature);
            }
          
           
        }

        public JsonResult Delete(int id)
        {
            new FeatureDAO().Delete(id);
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new FeatureDAO().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        public void SelectlistVehicle(long? sltedVehicle = null)
        {
            var dealerSession = (Models.UserLogin)Session[Common.Constants.DEALER_SESSION];
            var dao = new VehicleDAO().ListAll(dealerSession.UserID);
            ViewBag.Vehicle = new SelectList(dao, "ID", "Name", sltedVehicle);
        }
    }
}
using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;

namespace VehicleShowroomManagementSystem.Areas.BranchDealer.Controllers
{
    public class VehicleController : BaseController
    {
        // GET: BranchDealer/Vehicle
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dealerSession = (Models.UserLogin)Session[Common.Constants.DEALER_SESSION];
            var dao = new VehicleDAO();
            var model = dao.ListAll(null,dealerSession.UserID, searchString, page, pageSize);
            ViewBag.SearchKeyword = searchString;
            SelectListBrand();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(FormCollection form, string searchString, int page = 1, int pageSize = 10)
        {
            var dealerSession = (Models.UserLogin)Session[Common.Constants.DEALER_SESSION];
            string brandId = form["BrandID"];
            var dao = new VehicleDAO();
            if (!brandId.Equals(""))
            {
                IEnumerable<Vehicle> model = dao.ListAll(long.Parse(brandId), dealerSession.UserID, searchString, page, pageSize);
                SelectListBrand(long.Parse(brandId));
                ViewBag.SearchKeyword = searchString;
                return View(model);
            }
            else
            {
                IEnumerable<Vehicle> model = dao.ListAll(null, dealerSession.UserID, searchString, page, pageSize);
                SelectListBrand();
                ViewBag.SearchKeyword = searchString;
                return View(model);
            }
           
        }

        [HttpGet]
        public ActionResult Create()
        {
            SelectListBrand();
            SelectListPurchase();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Vehicle vehicle)
        {
            var dealerSession = (Models.UserLogin)Session[Common.Constants.DEALER_SESSION];
            if(new VehicleDAO().CheckModelNumberExist(vehicle.ModelNumber))
            {
                ModelState.AddModelError("", "Model Number is not valid");
                SelectListBrand();
                SelectListPurchase();
                return View();
            }
            if (ModelState.IsValid)
            {
                var dao = new VehicleDAO();
                vehicle.DealerID = dealerSession.UserID;              
                vehicle.Status = true;
                long id = dao.Insert(vehicle);
                if (id > 0)
                {
                    new PurchaseDAO().Update(vehicle.PurchaseID, vehicle.HistoricalCost);
                    SetAlert("Create Successfully...", "success");
                    return RedirectToAction("Index", "Vehicle");
                }
                else
                {
                    ModelState.AddModelError("", "OOPS Something went wrong...");
                    SelectListBrand();
                    SelectListPurchase();
                    return View();
                }

            }
            else
            {              
                SelectListBrand();
                SelectListPurchase();
                return View();
            }                 

        }

        public ActionResult Edit(long id)
        {
            var vehicle = new VehicleDAO().Detail(id);
            SelectListBrand(vehicle.BrandID);          
            return View(vehicle);
        }

        [HttpPost]
        public ActionResult Edit(Vehicle vehicle)
        {
            var model = new VehicleDAO().Detail(vehicle.ID);
            if(model.ModelNumber != vehicle.ModelNumber)
            {
                if (new VehicleDAO().CheckModelNumberExist(vehicle.ModelNumber))
                {
                    ModelState.AddModelError("", "Model Number is not valid");
                    SelectListBrand(vehicle.BrandID);
                    return View(vehicle);
                }
            }
            if (ModelState.IsValid)
            {
                var dao = new VehicleDAO();              
                var result = dao.Update(vehicle);

                if (result)
                {
                    SetAlert("Update Successfully...", "success");
                    return RedirectToAction("Index", "Vehicle");
                }
                else
                {
                    ModelState.AddModelError("", "OOPS Something went wrong...");
                    SelectListBrand(vehicle.BrandID);
                    return View(vehicle);
                }
            }
            else
            {
                SelectListBrand(vehicle.BrandID);
                return View(vehicle);
            }
           

        }

        public JsonResult Delete(long id)
        {
            new VehicleDAO().Delete(id);
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new VehicleDAO().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        public void SelectListBrand(long? sltedBrand = null)
        {
            var dao = new BrandDAO();
            ViewBag.Brand = new SelectList(dao.ListAll(), "ID", "Name", sltedBrand);
        }

        public void SelectListPurchase(long? sltedPurchase = null)
        {
            var dealerSession = (Models.UserLogin)Session[Common.Constants.DEALER_SESSION];
            var dao = new PurchaseDAO();
            ViewBag.Purchase = new SelectList(dao.ListWaiting(dealerSession.UserID), "ID", "ID", sltedPurchase);
        }
    }
}
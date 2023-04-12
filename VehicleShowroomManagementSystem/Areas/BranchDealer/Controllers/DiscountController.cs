using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DAO;
using Model.EF;

namespace VehicleShowroomManagementSystem.Areas.BranchDealer.Controllers
{
    public class DiscountController : BaseController
    {       
        // GET: BranchDealer/Discount
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dealerSession = (Models.UserLogin)Session[Common.Constants.DEALER_SESSION];
            var dao = new DiscountDAO();
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
        public ActionResult Create(Discount discount)
        {
            if (ModelState.IsValid)
            {
                var dao = new DiscountDAO();
                if (dao.CheckDiscountVehicle(discount.VehicleID))
                {
                    ModelState.AddModelError("", "OOPS Vehicle cannot be duplicate Discount Value");
                    SelectlistVehicle();
                    return View();
                }
                if(discount.ExpiredDate < discount.StartedDate)
                {
                    ModelState.AddModelError("", "OOPS Time is not valid");
                    SelectlistVehicle();
                    return View();
                }              
                discount.Status = true;
                long id = dao.Insert(discount);
                if (id > 0)
                {
                    SetAlert("Create Successfully...", "success");
                    return RedirectToAction("Index", "Discount");
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
            var discount = new DiscountDAO().Detail(id);
            SelectlistVehicle(discount.VehicleID);
            return View(discount);
        }

        [HttpPost]
        public ActionResult Edit(Discount discount)
        {
            if (ModelState.IsValid)
            {
                var dao = new DiscountDAO();
                if (discount.ExpiredDate < discount.StartedDate)
                {
                    ModelState.AddModelError("", "OOPS Time is not valid");
                    SelectlistVehicle(discount.VehicleID);
                    return View(discount);
                }
                var result = dao.Update(discount);

                if (result)
                {
                    SetAlert("Update Successfully...", "success");
                    return RedirectToAction("Index", "Discount");
                }
                else
                {
                    ModelState.AddModelError("", "OOPS Something went wrong...");
                    SelectlistVehicle(discount.VehicleID);
                    return View(discount);
                }
            }
            else
            {
                SelectlistVehicle(discount.VehicleID);
                return View(discount);
            }
          
           
        }

        public JsonResult Delete(int id)
        {
            new DiscountDAO().Delete(id);
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new DiscountDAO().ChangeStatus(id);
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
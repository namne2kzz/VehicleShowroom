using Model.DAO;
using Model.EF;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleShowroomManagementSystem.Models;

namespace VehicleShowroomManagementSystem.Areas.Showroom.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Showroom/Home     
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Report_BrandVehicleSell(int month)
        {
            List<BrandVehicleReport> result = new BrandDAO().ReportBrandVehicleSell(month);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Report_BrandVehicleAvailable()
        {
            List<BrandVehicleReport> result = new BrandDAO().ReportBrandVehicleAvailable();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Report_DealerVehicleSell(int month)
        {
            List<DealerVehicleReport> result = new DealerDAO().ReportDealerVehicleSell(month);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Report_AllotmentVehicleRegistration()
        {
            IEnumerable<AllotmentVehicleRegistrationReport> result = new VehicleRegistrationDAO().ListCountVehicleRegistrationByProvince();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Report_AllotmentContact(int month)
        {
            IEnumerable<AllotmentContactReport> result = new ContactDAO().ListCountContactByProvince(month);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Report_CustomerStatus()
        {
            var result = new CustomerDAO().ListCountCustomerByStatus();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Report_DealerStatus()
        {
            var result = new DealerDAO().ListCountDealerByStatus();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public new ActionResult Profile()
        {
            var showroomSession = (Models.UserLogin)Session[Common.Constants.SHOWROOM_SESSION];
            var model = new EmployeeDAO().Detail(showroomSession.UserID);
            return View(model);
        }      

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            var showroomSession = (Models.UserLogin)Session[Common.Constants.SHOWROOM_SESSION];
            if (ModelState.IsValid)
            {
                var employee = new EmployeeDAO().Detail(showroomSession.UserID);

                employee.Password = Common.Encryptor.MD5Hash(model.NewPassword);

                var rs = new EmployeeDAO().Update(employee);
                if (rs)
                {
                    SetAlert("Change Password Successfully...", "success");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "OOPS Something went wrong...");
                    return View();
                }
            }
            return View("ChangePassword");
        }
    }
}
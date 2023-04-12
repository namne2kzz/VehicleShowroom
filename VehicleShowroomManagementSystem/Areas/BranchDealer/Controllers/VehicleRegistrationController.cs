using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.DAO;
using System.Xml.Linq;
using VehicleShowroomManagementSystem.Models;

namespace VehicleShowroomManagementSystem.Areas.BranchDealer.Controllers
{
    public class VehicleRegistrationController : BaseController
    {
        // GET: BranchDealer/VehicleRegistration
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dealerSession = (Models.UserLogin)Session[Common.Constants.DEALER_SESSION];
           
            var dao = new VehicleRegistrationDAO();
            var model = dao.ListAll(dealerSession.UserID, searchString, page, pageSize);
            ViewBag.SearchKeyword = searchString;
           
            return View(model);
        }
        [HttpGet]
        public ActionResult Create()
        {
            var dealerSession = (Models.UserLogin)Session[Common.Constants.DEALER_SESSION];
            var vehicle = new VehicleDAO().ListVehicleRegistration(dealerSession.UserID);
            ViewBag.ListVehicleRegistration = new SelectList(vehicle, "ID", "Name");
            LoadProvince();
            return View();
        }

        [HttpPost]
        public ActionResult Create(VehicleRegistrationData vehicleRegistration)
        {
            if (ModelState.IsValid)
            {
                var dao = new VehicleRegistrationDAO();
                var vehicle = new VehicleDAO().Detail(vehicleRegistration.VehicleID);
                vehicleRegistration.CustomerID = vehicle.OwnerID.Value;
                vehicleRegistration.Status = true;
                long id = dao.Insert(vehicleRegistration);
                if (id > 0)
                {                   
                    SetAlert("Create Successfully...", "success");
                    return RedirectToAction("Index", "VehicleRegistration");
                }
                else
                {
                    ModelState.AddModelError("", "OOPS Something went wrong...");
                    LoadProvince();
                    return View();
                }
            }
            else
            {              
                LoadProvince();
                return View();
            }
           
           

        }

        public ActionResult Edit(long id)
        {
            var vehicleRegistration = new VehicleRegistrationDAO().Detail(id);
            LoadProvince(vehicleRegistration.RegistrationPlace);
            return View(vehicleRegistration);
        }

        [HttpPost]
        public ActionResult Edit(VehicleRegistrationData vehicleRegistration)
        {

            if (ModelState.IsValid)
            {
                var dao = new VehicleRegistrationDAO();

                var result = dao.Update(vehicleRegistration);

                if (result)
                {
                    SetAlert("Update Successfully...", "success");
                    return RedirectToAction("Index", "VehicleRegistration");
                }
                else
                {
                    ModelState.AddModelError("", "OOPS Something went wrong...");
                    LoadProvince(vehicleRegistration.RegistrationPlace);
                    return View(vehicleRegistration);
                }
            }
            else
            {
                LoadProvince(vehicleRegistration.RegistrationPlace);
                return View(vehicleRegistration);
            }
            
           

        }

        public JsonResult Delete(int id)
        {
            new VehicleRegistrationDAO().Delete(id);
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new VehicleRegistrationDAO().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        public void LoadProvince(string sltProvince=null)
        {
            var xmlDoc = XDocument.Load(Server.MapPath(@"~/App_Data/Provinces_Data.xml"));

            var xElement = xmlDoc.Element("Root").Elements("Item").Where(x => x.Attribute("type").Value == "province");

            var list = new List<ProvinceModel>();
            ProvinceModel province = null;
            foreach (var item in xElement)
            {
                province = new ProvinceModel();
                province.ID = int.Parse(item.Attribute("id").Value);
                province.Name = item.Attribute("value").Value;
                list.Add(province);
            }

            ViewBag.ListProvince = new SelectList(list, "Name", "Name",sltProvince);
        }

       
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.DAO;
using System.Xml.Linq;
using VehicleShowroomManagementSystem.Models;

namespace VehicleShowroomManagementSystem.Controllers
{
    public class AddressController : Controller
    {
        // GET: Address
        public ActionResult Index()
        {
            var customerSession = (Models.UserLogin)Session[Common.Constants.USER_SESSION];
            var model = new AddressDAO().ListAll(customerSession.UserID);
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Address address)
        {
            var customerSession = (Models.UserLogin)Session[Common.Constants.USER_SESSION];            
            if (ModelState.IsValid)
            {
                address.CustomerID = customerSession.UserID;
                if (new AddressDAO().CountAddress(customerSession.UserID) == 0)
                {
                    address.Status = true;
                }
                else
                {
                    address.Status = false;
                }
                var result = new AddressDAO().Insert(address);
                if (result > 0)
                {
                    return Redirect("Index");
                }
            }
            else
            {
                ModelState.AddModelError("", "OOPS Something went wrong...");
                return View(address);
            }
            return View();
        }

        [HttpPost]
        public ActionResult ChangeStatus(long id)
        {
            var result = new AddressDAO().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        public JsonResult Delete(long id)
        {          
                      
             new AddressDAO().Delete(id);
            
            return Json(new
            {
                status = true,
               
            });
        }

        public JsonResult LoadProvince()
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

            return Json(new
            {
                data = list,
                status = true
            });

        }

        public JsonResult LoadDistrict(string provinceId)
        {
            var xmlDoc = XDocument.Load(Server.MapPath(@"~/App_Data/Provinces_Data.xml"));

            var xElement = xmlDoc.Element("Root").Elements("Item").Single(x => x.Attribute("type").Value == "province" && x.Attribute("value").Value == provinceId);

            var list = new List<DistrictModel>();
            DistrictModel district = null;
            foreach (var item in xElement.Elements("Item").Where(x => x.Attribute("type").Value == "district"))
            {
                district = new DistrictModel();
                district.ID = int.Parse(item.Attribute("id").Value);
                district.Name = item.Attribute("value").Value;
                district.ProvinceID = int.Parse(xElement.Attribute("id").Value);
                list.Add(district);
            }

            return Json(new
            {
                data = list,
                status = true
            });

        }
    }
}
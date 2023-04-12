using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Model.DAO;
using Model.EF;
using VehicleShowroomManagementSystem.Models;

namespace VehicleShowroomManagementSystem.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            ViewBag.About = new AboutDAO().ListAll();

            return View();
        }

        [HttpPost]
        public ActionResult Index(Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.CreatedDate = DateTime.Now;
                contact.Status = true;
                var result = new ContactDAO().Insert(contact);
                if (result > 0)
                {
                    TempData["msg"] = "<script>alert('Succesfully');</script>";
                    return RedirectToAction("Index","Contact");
                }
            }
            else
            {
                ModelState.AddModelError("", "OOPS Something went wrong...");
                ViewBag.About = new AboutDAO().ListAll();
                return View();
            }

            return View();
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

    }
}
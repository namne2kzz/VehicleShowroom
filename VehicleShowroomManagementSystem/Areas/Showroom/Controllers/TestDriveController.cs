using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DAO;
using Model.EF;

namespace VehicleShowroomManagementSystem.Areas.Showroom.Controllers
{
    public class TestDriveController : BaseController
    {
        // GET: Showroom/TestDrive
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new TestDriveDAO();
            var model = dao.ListAll(searchString, page, pageSize);
            ViewBag.SearchKeyword = searchString;          
            SelectedListStatus();
            return View(model);
        } 

        public ActionResult Confirm(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new TestDriveDAO();
            var model = dao.ListConfirmTestDrive(searchString, page, pageSize);
            ViewBag.SearchKeyword = searchString;           
            SelectedListConfirm();
            return View(model);
        }                       

        public JsonResult Delete(int id)
        {
            new TestDriveDAO().Delete(id);


            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public ActionResult ChangeStatus(long id, FormCollection confirm)
        {
            var status = int.Parse(confirm["Status"]);
            if (status == -2)
            {
                new TestDriveDAO().Delete(id);
                SetAlert("Delete Successfully...", "success");
                return Redirect("Confirm");
            }
            var result = new TestDriveDAO().ChangeStatus(id, status);
            return Redirect("Index");
        }

        public void SelectedListStatus()
        {
            List<SelectListItem> listStatus = new List<SelectListItem>();

            listStatus.Add(new SelectListItem { Text = "ALLOW", Value = "1" });
            listStatus.Add(new SelectListItem { Text = "FINISHED", Value = "2" });
            ViewBag.ListStatus = listStatus;
        }

        public void SelectedListConfirm()
        {
            List<SelectListItem> listConfirm = new List<SelectListItem>();
            listConfirm.Add(new SelectListItem { Text = "WAITING CONFIRM", Value = "0" });
            listConfirm.Add(new SelectListItem { Text = "ALLOW", Value = "1" });
            listConfirm.Add(new SelectListItem { Text = "DENY", Value = "-2" });
            ViewBag.ListConfirm = listConfirm;
        }
       
    }
}
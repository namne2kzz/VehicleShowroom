using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DAO;
using Model.EF;

namespace VehicleShowroomManagementSystem.Areas.Showroom.Controllers
{
    public class ContactController : BaseController
    {
        // GET: Showroom/Contact
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ContactDAO();
            var model = dao.ListAll(searchString, page, pageSize);
            ViewBag.SearchKeyword = searchString;
            return View(model);
        }                

        public JsonResult Delete(int id)
        {
            new ContactDAO().Delete(id);


            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new ContactDAO().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}
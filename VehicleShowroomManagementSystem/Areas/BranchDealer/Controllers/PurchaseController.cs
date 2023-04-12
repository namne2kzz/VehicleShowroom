using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Rotativa;
using System.Web.UI.WebControls;

namespace VehicleShowroomManagementSystem.Areas.BranchDealer.Controllers
{
    public class PurchaseController : BaseController
    {
        // GET: BranchDealer/Purchase
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dealerSession = (Models.UserLogin)Session[Common.Constants.DEALER_SESSION];
            var dao = new PurchaseDAO();
            var model = dao.ListAll(dealerSession.UserID,searchString, page, pageSize);
            ViewBag.SearchKeyword = searchString;
          
            return View(model);
        }

        public ActionResult Waiting(string searchString, int page = 1, int pageSize = 10)
        {
            var dealerSession = (Models.UserLogin)Session[Common.Constants.DEALER_SESSION];
            var dao = new PurchaseDAO();
            var model = dao.ListWaiting(dealerSession.UserID,searchString, page, pageSize);
            ViewBag.SearchKeyword = searchString;
            SelectedListWaiting();
            return View(model);
        }

       
        public ActionResult Billing(long id)
        {
            var model = new VehicleDAO().ListBill(id);
            var billInfo = new PurchaseDAO().Detail(id);
            ViewBag.BillInfo = billInfo;
            ViewBag.DealerId = id;
            return View(model);
        }      

        
        public PartialViewAsPdf PrintPDF(long id)
        {
            var model = new VehicleDAO().ListBill(id);
            var billInfo = new PurchaseDAO().Detail(id);
            ViewBag.BillInfo = billInfo;
            return new PartialViewAsPdf("PrintPDF", model)
            {
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                FileName = "PurchaseDocument" + id.ToString() + ".pdf"
            };
        }

        [HttpGet]
        public ActionResult Create()
        {                       
            return View();
        }

        [HttpPost]
        public ActionResult Create(Purchase purchase)
        {
            var dealerSession = (Models.UserLogin)Session[Common.Constants.DEALER_SESSION];
            if (ModelState.IsValid)
            {
                var dao = new PurchaseDAO();
                purchase.DealerID = dealerSession.UserID;
                purchase.CreatedDate = DateTime.Now;
                purchase.ReceivedDate = DateTime.Now;
                purchase.Quantity = 0;
                purchase.TotalCost = 0;
                purchase.Status = 0;
                long id = dao.Insert(purchase);
                if (id > 0)
                {
                    SetAlert("Create Successfully...", "success");
                    return RedirectToAction("Waiting", "Purchase");
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
            var purchase = new PurchaseDAO().Detail(id);                  
            return View(purchase);
        }

        [HttpPost]
        public ActionResult Edit(Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                var dao = new PurchaseDAO();

                var result = dao.Update(purchase);

                if (result)
                {
                    SetAlert("Update Successfully...", "success");
                    return RedirectToAction("Index", "Purchase");
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

        public JsonResult Delete(int id)
        {
            new PurchaseDAO().Delete(id);
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public ActionResult ChangeStatus(long id, FormCollection confirm)
        {
            var status = int.Parse(confirm["Status"]);
            if(status==1 )
            {
                var dao = new PurchaseDAO().Update(id, DateTime.Now);
            }            
            var result = new PurchaseDAO().ChangeStatus(id, status);
            return Redirect("Index");
        }
      

        public void SelectedListWaiting()
        {
            List<SelectListItem> listWaiting = new List<SelectListItem>();
            listWaiting.Add(new SelectListItem { Text = "WAITING GOODS RECEIVE", Value = "0" });
            listWaiting.Add(new SelectListItem { Text = "GOODS RECEIVED SUCCESSFULLY", Value = "1" });         
            ViewBag.ListWaiting = listWaiting;
        }

    }
}
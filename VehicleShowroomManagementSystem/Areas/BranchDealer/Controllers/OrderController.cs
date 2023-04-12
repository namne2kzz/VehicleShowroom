using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DAO;
using Model.EF;
using Rotativa;

namespace VehicleShowroomManagementSystem.Areas.BranchDealer.Controllers
{
    public class OrderController : BaseController
    {
        // GET: BranchDealer/Order
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dealerSession = (Models.UserLogin)Session[Common.Constants.DEALER_SESSION];
            var dao = new OrderDAO();
            var model = dao.ListAll(dealerSession.UserID, searchString, page, pageSize);
            ViewBag.SearchKeyword = searchString;
            SelectedListStatus();
            return View(model);
        }

        public ActionResult Confirm(string searchString, int page = 1, int pageSize = 10)
        {
            var dealerSession = (Models.UserLogin)Session[Common.Constants.DEALER_SESSION];
            var dao = new OrderDAO();
            var model = dao.ListConfirmBlog(dealerSession.UserID, searchString, page, pageSize);
            ViewBag.SearchKeyword = searchString;
            SelectedListConfirm();
            return View(model);
        }       

        [HttpGet]
        public ActionResult Create()
        {           
            return View();
        }

        [HttpPost]
        public ActionResult Create(Order order)
        {
            var dealerSession = (Models.UserLogin)Session[Common.Constants.DEALER_SESSION];
            if (ModelState.IsValid)
            {
                var dao = new OrderDAO();                              
                order.OrderedDate = DateTime.Now;
                order.Status = 0;
                long id = dao.Insert(order);
                if (id > 0)
                {
                    new OrderDAO().Update(id);
                    SetAlert("Create Successfully...", "success");
                    return Redirect("/");
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
            
        public ActionResult Detail(long id)
        {
            var dao = new OrderDAO().Detail(id);
            ViewBag.Order = dao;
            ViewBag.Dealer = new DealerDAO().Detail(dao.DealerID);
            ViewBag.Customer = new CustomerDAO().Detail(dao.CustomerID);
            var listIdService = new ServiceOrderDetailDAO().ListAll(id);
            var listService = new List<Service>();
            foreach (var item in listIdService)
            {
                listService.Add(new ServiceDAO().Detail(item.ServiceID));
            }
            ViewBag.Service = listService;
            var model = new OrderDAO().ListAllByOrder(id);
            return View(model);
        }

        public PartialViewAsPdf PrintPDF(long id)
        {
            var model = new OrderDAO().ListAllByOrder(id);
            var dao = new OrderDAO().Detail(id);
            ViewBag.Order = dao;
            ViewBag.Dealer = new DealerDAO().Detail(dao.DealerID);
            ViewBag.Customer = new CustomerDAO().Detail(dao.CustomerID);
            var listIdService = new ServiceOrderDetailDAO().ListAll(id);
            var listService =new  List<Service>();
            foreach (var item in listIdService)
            {
                listService.Add(new ServiceDAO().Detail(item.ServiceID));
            }
            ViewBag.Service = listService;
            return new PartialViewAsPdf("PrintPDF", model)
            {
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                FileName = "SalesOrderDocument" + id.ToString() + ".pdf"
            };
        }

        public JsonResult Delete(int id)
        {
            new OrderDAO().Delete(id);


            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public ActionResult ChangeStatus(long id, FormCollection confirm)
        {
            var status = int.Parse(confirm["Status"]);
            if (status == -1)
            {
                new OrderDAO().Delete(id);
                SetAlert("Delete Successfully...", "success");
                return Redirect("Confirm");
            }
            var order = new OrderDAO().Detail(id);
            var result = new OrderDAO().ChangeStatus(id, status);
            if(result == 3)
            {
                var orderDetail = new OrderDAO().ListAllByOrder(id);
                foreach (var item in orderDetail)
                {
                    new VehicleDAO().UpdateOwnerID(item.VehicleID, order.CustomerID);                   
                }
            }
            return Redirect("Index");
        }

        public void SelectedListStatus()
        {
            List<SelectListItem> listStatus = new List<SelectListItem>();

            listStatus.Add(new SelectListItem { Text = "PREPARING GOODS", Value = "1" });
            listStatus.Add(new SelectListItem { Text = "DELIVERING", Value = "2" });
            listStatus.Add(new SelectListItem { Text = "SUCCESSFULLY", Value = "3" });
            listStatus.Add(new SelectListItem { Text = "FAIL DEVIVERY", Value = "-2" });
            ViewBag.ListStatus = listStatus;
        }

        public void SelectedListConfirm()
        {
            List<SelectListItem> listConfirm = new List<SelectListItem>();
            listConfirm.Add(new SelectListItem { Text = "WAITING CONFIRM", Value = "0" });
            listConfirm.Add(new SelectListItem { Text = "ACCEPT ORDER", Value = "1" });
            listConfirm.Add(new SelectListItem { Text = "CANCLE", Value = "-3" });
            ViewBag.ListConfirm = listConfirm;
        }
    }
}
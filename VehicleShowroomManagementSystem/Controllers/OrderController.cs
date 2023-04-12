using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleShowroomManagementSystem.Models;
using Model.DAO;
using Model.EF;

namespace VehicleShowroomManagementSystem.Controllers
{
    public class OrderController : BaseController
    {
        // GET: Order
        public ActionResult Index()
        {
            var customerSession = (Models.UserLogin)Session[Common.Constants.USER_SESSION];
            decimal totalDiscount = 0;
            decimal totalCost = 0;
            var listCheckout = (List<CartItem>)Session[Common.Constants.CART_SESSION];

            foreach (CartItem item in listCheckout)
            {
                totalCost += item.Vehicle.Price;
                totalDiscount += (decimal)item.SaleOff * item.Vehicle.Price;
            }
            ViewBag.Address = new SelectList(new AddressDAO().ListByCustomer(customerSession.UserID),"AddressDetail","AddressDetail");
            ViewBag.TotalDiscount = totalDiscount.ToString("N0");
            ViewBag.TotalCost = totalCost.ToString("N0");
            ViewBag.TotalPayment = (totalCost - totalDiscount).ToString("N0");
            ViewBag.Checkout = listCheckout;
            ViewBag.Service = (List<Service>)Session[Common.Constants.SERVICE_SESSION];
            return View();
        }

        [HttpPost]
        public ActionResult Index(Order order)
        {
            var customerSession = (Models.UserLogin)Session[Common.Constants.USER_SESSION];
            if (ModelState.IsValid)
            {
                order.DealerID = long.Parse((Session[Common.Constants.DEALER_SESSION].ToString()));
                order.CustomerID = customerSession.UserID;
                order.OrderedDate = DateTime.Now;
                order.Status = 0;
                var result = new OrderDAO().Insert(order);
                var listOrderDetail = (List<CartItem>)Session[Common.Constants.CART_SESSION];
                var listService = (List<Service>)Session[Common.Constants.SERVICE_SESSION];
                var listMandatoryCost = new MandatoryCostDAO().ListAll();
                if (result > 0)
                {                  
                    foreach (var item in listOrderDetail)
                    {
                        var orderDetail = new OrderDetail();
                        orderDetail.OrderID = result;
                        orderDetail.VehicleID = item.Vehicle.ID;                        
                        foreach (var sub in listMandatoryCost)
                        {
                            if (sub.ID == 1)
                            {
                                orderDetail.RegistrationFee = (sub.Cost / 100) * item.Vehicle.Price;
                            }
                            else if (sub.ID == 2)
                            {
                                orderDetail.RoadMaintenanceFee = sub.Cost;
                            }
                            else if (sub.ID == 3)
                            {
                                orderDetail.CivilLiabilityInsuranceFee = sub.Cost;
                            }
                            else if (sub.ID == 4)
                            {
                                orderDetail.TestingFee = sub.Cost;
                            }
                            else if (sub.ID == 5)
                            {
                                orderDetail.RegistrationPlateFee = sub.Cost;
                            }
                        }
                        var rs = new OrderDetailDAO().Insert(orderDetail);
                        if (rs>0)
                        {
                            var listCart = new CartDAO().ListAll();
                            foreach (var c in listCart)
                            {
                                if (c.VehicleID == item.Vehicle.ID)
                                {
                                    var cart = new CartDAO().Detail(c.VehicleID);
                                    new CartDAO().Delete(cart.ID);
                                }
                               
                            }
                        }

                    }

                    new OrderDAO().Update(result);

                    foreach (var item in listService)
                    {
                        var serviceOrderDetail = new ServiceOrderDetail();
                        serviceOrderDetail.OrderID = result;
                        serviceOrderDetail.ServiceID = item.ID;
                        new ServiceOrderDetailDAO().Insert(serviceOrderDetail);
                    }

                }
                return RedirectToAction("DashBoard", "Customer");
            }
            else
            {
                ModelState.AddModelError("", "OOPS Something went wrong...");
                decimal totalDiscount = 0;
                decimal totalCost = 0;
                var listCheckout = (List<CartItem>)Session[Common.Constants.CART_SESSION];

                foreach (CartItem item in listCheckout)
                {
                    totalCost += item.Vehicle.Price;
                    totalDiscount += (decimal)item.SaleOff * item.Vehicle.Price;
                }
                ViewBag.Address = new SelectList(new AddressDAO().ListByCustomer(customerSession.UserID), "AddressDetail", "AddressDetail");
                ViewBag.TotalDiscount = totalDiscount;
                ViewBag.TotalCost = totalCost;
                ViewBag.TotalPayment = totalCost - totalDiscount;
                ViewBag.Checkout = listCheckout;
                ViewBag.Service = (List<Service>)Session[Common.Constants.SERVICE_SESSION];
                return View();
            }
           
        }

        public ActionResult OrderDetail(long orderId)
        {

            var order = new OrderDAO().Detail(orderId);
            ViewBag.Order = order;          
            ViewBag.Customer = new CustomerDAO().Detail(order.CustomerID);
            var listIdService = new ServiceOrderDetailDAO().ListAll(orderId);
            var listService = new List<Service>();
            foreach (var item in listIdService)
            {
                listService.Add(new ServiceDAO().Detail(item.ServiceID));
            }
            ViewBag.Service = listService;
            var model = new OrderDAO().ListAllByOrder(orderId);
            return View(model);
        }

    }
}
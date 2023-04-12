using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using VehicleShowroomManagementSystem.Models;

namespace VehicleShowroomManagementSystem.Controllers
{
    public class CartController : BaseController
    {

        // GET: Cart
        public ActionResult Index()
        {
            var customerSession = (Models.UserLogin)Session[Common.Constants.USER_SESSION];
            if (customerSession == null)
            {
                return View();
            }
            var model = new CartDAO().ListCartByCustomer(customerSession.UserID);
            var listCart = new List<CartItem>();

            foreach (Model.EF.Cart item in model)
            {
                var cart = new CartItem();
                cart.Vehicle = new VehicleDAO().Detail(item.VehicleID);
                cart.SaleOff = new DiscountDAO().GetDiscountVehicle(item.VehicleID);
                cart.Image = new ImageDAO().GetMainImage(item.VehicleID).Image1;
                listCart.Add(cart);
            }
            SelectedListDealer();
            return View(listCart);
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            var customerSession = (Models.UserLogin)Session[Common.Constants.USER_SESSION];
            Session[Common.Constants.CART_SESSION] = null;
            Session[Common.Constants.SERVICE_SESSION] = null;
            Session[Common.Constants.DEALER_SESSION] = null;
            if (customerSession == null)
            {
                return RedirectToAction("Login", "Customer");
            }
            string dealerID = form["DealerID"];
            Session[Common.Constants.DEALER_SESSION] = long.Parse(dealerID);

            if (!dealerID.ToString().Equals(""))
            {
                ViewBag.dealerID = dealerID;
                ViewBag.Service = new ServiceDAO().ListAll(long.Parse(dealerID));
                var model = new CartDAO().ListCartByCustomerByDealer(customerSession.UserID, long.Parse(dealerID));
                var listCart = new List<CartItem>();

                foreach (Model.EF.Cart item in model)
                {
                    var cart = new CartItem();
                    cart.Vehicle = new VehicleDAO().Detail(item.VehicleID);
                    cart.SaleOff = new DiscountDAO().GetDiscountVehicle(item.VehicleID);
                    cart.Image = new ImageDAO().GetMainImage(item.VehicleID).Image1;
                    listCart.Add(cart);
                }
                SelectedListDealer(Convert.ToInt64(form["DealerID"]));
                return View(listCart);
            }
            else
            {
                var model = new CartDAO().ListCartByCustomer(customerSession.UserID);
                var listCart = new List<CartItem>();

                foreach (Model.EF.Cart item in model)
                {
                    var cart = new CartItem();
                    cart.Vehicle = new VehicleDAO().Detail(item.VehicleID);
                    cart.SaleOff = new DiscountDAO().GetDiscountVehicle(item.VehicleID);
                    cart.Image = new ImageDAO().GetMainImage(item.VehicleID).Image1;
                    listCart.Add(cart);
                }
                SelectedListDealer();
                return View(listCart);
            }
        }

        public ActionResult AddItem(long vehicleId)
        {

            var customerSession = (Models.UserLogin)Session[Common.Constants.USER_SESSION];
            if (new CartDAO().CheckVehicleExistCart(customerSession.UserID, vehicleId))
            {
                RedirectToAction("Index");
            }
            else
            {
                var vehicle = new VehicleDAO().Detail(vehicleId);
                var discount = new DiscountDAO().GetDiscountVehicle(vehicleId);
                var image = new ImageDAO().GetMainImage(vehicleId);

                Cart cartItem = new Cart();
                cartItem.CustomerID = customerSession.UserID;
                cartItem.VehicleID = vehicleId;
                cartItem.CreatedDate = DateTime.Now;
                cartItem.Status = true;
                new CartDAO().Insert(cartItem);

            }

            return RedirectToAction("Index");

        }

        [HttpPost]
        public JsonResult PrepareCheckout(List<string> vehicle, List<string> service)
        {
           if(vehicle==null || service == null)
            {
                return Json(new
                {
                    status = false
                }) ;
            }
            
            var list = new List<CartItem>();
            foreach (var item in vehicle)
            {
                string[] str = item.Split('-');
                var cartItem = new CartItem();
                var model = new VehicleDAO().Detail(long.Parse(str[0]));
                cartItem.Vehicle = model;
                cartItem.Image = str[1];
                cartItem.SaleOff = double.Parse(str[2]);
                list.Add(cartItem);
            }
                              
            Session[Common.Constants.CART_SESSION] = list;

            var listServive = new List<Service>();
            foreach (var sub in service)
            {
                var serviceItem = new ServiceDAO().Detail(long.Parse(sub));
                listServive.Add(serviceItem);               
            }
            Session[Common.Constants.SERVICE_SESSION] = listServive;

            return Json(new
            {
                status = true
            });
        }               

        public JsonResult Remove(long id)
        {

            var model = new CartDAO().Detail(id);
            new CartDAO().Delete(model.ID);
            return Json(new
            {
                status = true
            });
        }

        public JsonResult ClearAll()
        {
            var customerSession = (Models.UserLogin)Session[Common.Constants.USER_SESSION];
            for (int i = 0; i <= new CartDAO().CountCartByCustomer(customerSession.UserID); i++)
            {
                new CartDAO().Delete(new CartDAO().CartByCustomer(customerSession.UserID).ID);
            }
            return Json(new
            {
                status = true
            });
        }

        public void SelectedListDealer(long? sltedDealer = null)
        {
            var dealer = new DealerDAO().ListAll();
            ViewBag.Dealer = new SelectList(dealer, "ID", "DealerName", sltedDealer);
        }
    }
}
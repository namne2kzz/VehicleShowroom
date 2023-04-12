using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleShowroomManagementSystem.Models;

namespace VehicleShowroomManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.MainSlide = new SlideDAO().MainSlide();
            ViewBag.ExtraSlide = new SlideDAO().ExtraSlide();
            ViewBag.ListDiscount = new DiscountDAO().ListDiscount();
            ViewBag.ListFeature = new FeatureDAO().ListFeature();
            ViewBag.ListBlog = new BlogDAO().ListAll();
            ViewBag.ListBrand = new BrandDAO().ListAll();
            ViewBag.ListDealer = new DealerDAO().ListAll();
            //new Common.MailHelper().SendMail("huynhphuongnam2000@gmail.com", "ergvr", "fjhdvjh");
            return View();
          
        }

        [ChildActionOnly]
        public PartialViewResult P_Menu()
        {

            return PartialView();
        }

        [ChildActionOnly]
        public PartialViewResult P_Footer()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public PartialViewResult P_Cart()
        {
          
            var customerSession = (Models.UserLogin)Session[Common.Constants.USER_SESSION];
            var model = new List<Cart>();
            var listCart = new List<CartItem>();
            if (customerSession != null)
            {
                model = new CartDAO().ListCartByCustomer(customerSession.UserID);
            }
            else
            {
                model = null;
                listCart = null;
                return PartialView(listCart);
            }                   
            foreach (Model.EF.Cart item in model)
            {
                var cart = new CartItem();
                cart.Vehicle = new VehicleDAO().Detail(item.VehicleID);
                cart.SaleOff = new DiscountDAO().GetDiscountVehicle(item.VehicleID);
                cart.Image = new ImageDAO().GetMainImage(item.VehicleID).Image1;
                listCart.Add(cart);
               
            }
            return PartialView(listCart);
        }
    }
}
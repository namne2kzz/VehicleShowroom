using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DAO;
using Model.EF;
using Model.ViewModel;

namespace VehicleShowroomManagementSystem.Controllers
{
    public class VehicleController : Controller
    {
        // GET: Vehicle
        public ActionResult ListVehicleByDealer(long dealerId, int pageIndex = 1, int pageSize = 5)
        {            
            int totalRecord = 0;
            var model = new VehicleDAO().ListVehicleByDealer(dealerId, ref totalRecord, pageIndex, pageSize);
            ViewBag.Total = totalRecord;
            ViewBag.Page = pageIndex;
            ViewBag.PageSize = pageSize;
            int maxPage = 5;
            int totalPage = 0;
            totalPage = (int)(Math.Ceiling((decimal)totalRecord / (decimal)pageSize));
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.DealerId = dealerId;
            ViewBag.Blog = new BlogDAO().ListBlogByDealer(dealerId);
            ViewBag.Dealer = new DealerDAO().Detail(dealerId);
            var compare = Session[Common.Constants.COMPARE_SESSION];
            var list = new List<CompareViewModel>();
            if (compare != null)
            {
                list = (List<CompareViewModel>)compare;
            }
            ViewBag.Compare = list;
            return View(model);
        }
      

        public ActionResult Compare()
        {
            var compare = Session[Common.Constants.COMPARE_SESSION];
            var list = new List<CompareViewModel>();
            if (compare != null)
            {
                list = (List<CompareViewModel>)compare;
            }
            if (list.Count == 1)
            {
                ViewBag.Vehicle1 = list[0];
                ViewBag.Vehicle2 = null;
                ViewBag.Vehicle3 = null;
            }
            else if (list.Count == 2)
            {
                ViewBag.Vehicle1 = list[0];
                ViewBag.Vehicle2 = list[1];
                ViewBag.Vehicle3 = null;
            }
            else if(list.Count==3)
            {
                ViewBag.Vehicle1 = list[0];
                ViewBag.Vehicle2 = list[1];
                ViewBag.Vehicle3 = list[2];
            }
            else if (list.Count == 0)
            {
                ViewBag.Vehicle1 = null;
                ViewBag.Vehicle2 = null;
                ViewBag.Vehicle3 = null;
            }
           
          
            return View();
        }

        public ActionResult AddCompare(long vehicleId)
        {          
            var compare = Session[Common.Constants.COMPARE_SESSION];
            if (compare != null)
            {
                var list = (List<CompareViewModel>)compare;
                if (list.Count == 3)
                {
                    if (list.Exists(x => x.ID != vehicleId))
                    {
                        var vehicle = new VehicleDAO().GetCompareVehicle(vehicleId);
                        list.Add(vehicle);
                        list.RemoveAt(0);
                    }                   
                }
                else
                {
                    if (list.Exists(x => x.ID == vehicleId)==false)
                    {
                        var vehicle = new VehicleDAO().GetCompareVehicle(vehicleId);
                        list.Add(vehicle);
                       
                    }
                }
                                           
            Session[Common.Constants.COMPARE_SESSION] = list;

            }
            else
            {
                var list =new List<CompareViewModel>();                              
                list.Add(new VehicleDAO().GetCompareVehicle(vehicleId));               
                Session[Common.Constants.COMPARE_SESSION] = list;
            }
            return Redirect("Compare");
        }

        public JsonResult RemoveCompare(long vehicleId)
        {
            var compare = Session[Common.Constants.COMPARE_SESSION];
            var list = new List<CompareViewModel>();
            if (compare != null)
            {
                list = (List<CompareViewModel>)compare;
            }
            foreach (var item in list)
            {
                if (item.ID == vehicleId)
                {
                    list.Remove(item);
                    return Json(new
                    {
                        status = true
                    });
                }
            }
            Session[Common.Constants.COMPARE_SESSION] = list;
            return Json(new
            {
                status=true
            });
        }

        public ActionResult ClearCompare()
        {
            Session[Common.Constants.COMPARE_SESSION] = null;
            return Redirect("Compare");
        }

        public ActionResult FinanceCalculator()
        {
            return View();
        }

        public JsonResult LoadBrand()
        {
            var model = new BrandDAO().ListAll();
            return Json(new
            {
                data = model,
                status = true,
            });
        }

        public JsonResult LoadVehicle(long brandId)
        {
            var model = new VehicleDAO().ListByBrand(brandId);
            return Json(new
            {
                data=model,
                status=true,
            });
        }

        public JsonResult Finance(long vehicleId)
        {
            var model = new VehicleDAO().Detail(vehicleId);
            var image = (new ImageDAO().ListAll(vehicleId)).Where(x=>x.Status==true).FirstOrDefault();
            var mandatoryCost = new MandatoryCostDAO().ListAll();
            return Json(new
            {
                image = image.Image1,
                price = string.Format("{0:c}", model.Price),
                registrationFee = string.Format("{0:c}", mandatoryCost[0].Cost/100 * model.Price),
                roadMaintenanceFee = string.Format("{0:c}", mandatoryCost[1].Cost),
                civilLiabilityInsuranceFee = string.Format("{0:c}", mandatoryCost[2].Cost),
                testingFee = string.Format("{0:c}", mandatoryCost[3].Cost),
                registrationPlateFee = string.Format("{0:c}", mandatoryCost[4].Cost),
                total = string.Format("{0:c}", model.Price + mandatoryCost[0].Cost * model.Price + mandatoryCost[1].Cost + mandatoryCost[2].Cost + mandatoryCost[3].Cost + mandatoryCost[4].Cost),
                status = true,
            }); 
        }

        public ActionResult TestDrive()
        {
            var model = new VehicleDAO().ListToTestDrive();
            ViewBag.Vehicle = new SelectList(model, "ID", "Name");

            List<SelectListItem> listDriverLicense = new List<SelectListItem>();

            listDriverLicense.Add(new SelectListItem { Text = "B1", Value = "B1" });
            listDriverLicense.Add(new SelectListItem { Text = "B2", Value = "B2" });
            listDriverLicense.Add(new SelectListItem { Text = "C", Value = "C" });
            listDriverLicense.Add(new SelectListItem { Text = "D", Value = "D" });
            listDriverLicense.Add(new SelectListItem { Text = "E", Value = "E" });
            listDriverLicense.Add(new SelectListItem { Text = "F", Value = "F" });
            ViewBag.DriverLicense = listDriverLicense;

            return View();
        }

        [HttpPost]
        public ActionResult TestDrive(TestDrive model)
        {
            var customerSession = (Models.UserLogin)Session[Common.Constants.USER_SESSION];
            if (ModelState.IsValid)
            {
                model.CustomerID = customerSession.UserID;
                model.Status = 0;
                var result = new TestDriveDAO().Insert(model);
                if (result > 0)
                {
                    TempData["msg"] = "<script>alert('Succesfully');</script>";
                    return Redirect("TestDrive");
                }
            }
            else
            {
                ModelState.AddModelError("", "OOPS Something went wrong...");
                return View();
            }
            return Redirect("TestDrive");
        }

        public ActionResult VehicleDetail(long vehicleId)
        {
            
            var model = new VehicleDAO().GetAllDetail(vehicleId);
            ViewBag.Service = new ServiceDAO().ListAll(model.DealerID);
            ViewBag.Image = new ImageDAO().ListImageFalse(vehicleId);
            ViewBag.Review = new ReviewDAO().ListByVehicle(vehicleId);
            ViewBag.Related = new VehicleDAO().ListRelated(vehicleId);
            return View(model);
        }

        public ActionResult AddReview(long vehicleId, string content)
        {
            var customerSession = (Models.UserLogin)Session[Common.Constants.USER_SESSION];
            var review = new Review();
            review.CustomerID = customerSession.UserID;
            review.VehicleID = vehicleId;
            review.Content = content;           
            review.CreatedDate = DateTime.Now.Date;
            review.Status = true;
            var result = new ReviewDAO().Insert(review);
            if (result > 0)
            {
                return RedirectToAction("VehicleDetail", new { vehicleId= vehicleId });
            }
            return RedirectToAction("VehicleDetail", new { vehicleId = vehicleId });
        }

        public ActionResult Search(string keyword, int pageIndex = 1, int pageSize = 5)
        {
            int totalRecord = 0;
            var model = new VehicleDAO().ListVehicleBySearch(keyword, ref totalRecord, pageIndex, pageSize);
            ViewBag.Total = totalRecord;
            ViewBag.Page = pageIndex;
            ViewBag.PageSize = pageSize;
            int maxPage = 5;
            int totalPage = 0;
            totalPage = (int)(Math.Ceiling((decimal)totalRecord / (decimal)pageSize));
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Keyword = keyword;
            ViewBag.Brand = new BrandDAO().ListAll();           
            var compare = Session[Common.Constants.COMPARE_SESSION];
            var list = new List<CompareViewModel>();
            if (compare != null)
            {
                list = (List<CompareViewModel>)compare;
            }
            ViewBag.Compare = list;
            return View(model);
        }

        public JsonResult ListName(string q)
        {
            var data = new VehicleDAO().ListName(q);
            return Json(new
            {
                data = data,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DAO;
using Model.EF;
using VehicleShowroomManagementSystem.Models;

namespace VehicleShowroomManagementSystem.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        public ActionResult Index(string searchString, int pageIndex = 1, int pageSize = 5)
        {
            var brand = new BrandDAO().ListAll();
            var listCountVehicleByBrand = new List<CountVehicleByBrand>();
            foreach (var item in brand)
            {
                var subModel = new CountVehicleByBrand();
                subModel.Name = item.Name;
                subModel.Quantity = new VehicleDAO().CountVehicleByBrand(null,item.ID);
                listCountVehicleByBrand.Add(subModel);
            }
            ViewBag.CountVehicleByBrand = listCountVehicleByBrand;
            ViewBag.NewDealer = new DealerDAO().ListAll().Take<Dealer>(4);
            ViewBag.SearchKeyword = searchString;
            int totalRecord = 0;
            var model = new BlogDAO().ListAll(searchString, ref totalRecord, pageIndex, pageSize);
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
            return View(model);
           
        }
    }
}
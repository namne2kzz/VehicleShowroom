using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using System.IO;

namespace VehicleShowroomManagementSystem.Areas.BranchDealer.Controllers
{
    public class ImageController : BaseController
    {
        
        // GET: BranchDealer/Image        
        public ActionResult Index(long vehicleId)
        {           
            var dao = new ImageDAO();
            var model = dao.ListAll(vehicleId);
            ViewBag.VehicleId = vehicleId;
            return View(model);
        }        

        [HttpPost]
        public ActionResult Create(long vehicleId, HttpPostedFileBase uploadImage)
        {
            var dealerSession = (Models.UserLogin)Session[Common.Constants.DEALER_SESSION];
            if (ModelState.IsValid)
            {
                var dao = new ImageDAO();
                var picture = new Image();
                if (uploadImage != null)
                {
                    var fileName = Path.GetFileName(uploadImage.FileName); //getting only file name(ex-ganesh.jpg)  
                    var ext = Path.GetExtension(uploadImage.FileName); //getting the extension(ex-.jpg)  
                    var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };
                    if (allowedExtensions.Contains(ext)) //check what type of extension  
                    {
                        string image = "vehicle" + vehicleId.ToString()+"_" + (dao.CountImage(vehicleId)+1).ToString() + ext; //appending the name with id                                                                 // store the file inside ~/project folder(Img)  
                        uploadImage.SaveAs(Path.Combine(Server.MapPath("~/Assets/dealer/img/vehicle/" + image)));
                       
                        picture.Image1 = image;
                    }
                    else
                    {
                        ModelState.AddModelError("", "OOPS Please choose only Image file");
                        return Redirect("?vehicleId=" + vehicleId);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "OOPS Image cannot be blank...");
                    return Redirect("?vehicleId=" + vehicleId );
                }
                picture.VehicleID = vehicleId;
                if (dao.CountImage(vehicleId) == 0)
                {
                    picture.Status = true;
                }
                else
                {
                    picture.Status = false;
                }
              
                long id = dao.Insert(picture);
                if (id > 0)
                {
                    SetAlert("Create Successfully...", "success");
                    return Redirect("?vehicleId=" + vehicleId);
                }
                else
                {
                    ModelState.AddModelError("", "OOPS Something went wrong...");
                    return Redirect("?vehicleId=" + vehicleId);
                }
            }
            else
            {              
                return Redirect("?vehicleId=" + vehicleId);
            }           

        }

        [HttpPost]
        public ActionResult ChangeStatus(long id)
        {
            var result = new ImageDAO().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        public JsonResult Delete(long id)
        {          
            var model = new ImageDAO().Detail(id);
            var filePath = Server.MapPath("~/Assets/dealer/img/vehicle/" + model.Image1);
            FileInfo fileImage = new FileInfo(filePath);
            if (fileImage.Exists)
            {
                fileImage.Delete();
                new ImageDAO().Delete(id);
            }
            return Json(new
            {
                status = true,
                vehicleId = model.VehicleID,
            }); 
        }

    }
}
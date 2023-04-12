using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using VehicleShowroomManagementSystem.Models;

namespace VehicleShowroomManagementSystem.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = (UserLogin)Session[Constants.USER_SESSION];
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(

                    new RouteValueDictionary(new { controller = "Customer", action = "Login" }));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
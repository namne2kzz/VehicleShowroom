using System.Web.Mvc;

namespace VehicleShowroomManagementSystem.Areas.Showroom
{
    public class ShowroomAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Showroom";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Showroom_default",
                "Showroom/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
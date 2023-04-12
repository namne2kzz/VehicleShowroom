using System.Web.Mvc;

namespace VehicleShowroomManagementSystem.Areas.BranchDealer
{
    public class BranchDealerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "BranchDealer";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "BranchDealer_default",
                "BranchDealer/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
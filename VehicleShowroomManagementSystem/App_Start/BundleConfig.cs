using System.Web;
using System.Web.Optimization;

namespace VehicleShowroomManagementSystem
{
    public class BundleConfig
    {
        
        public static void RegisterBundles(BundleCollection bundles)
        {
            //js
            bundles.Add(new ScriptBundle("~/bundles/clientjs").Include(
                        "~/Assets/client/js/jquery.min.js",
                        "~/Assets/client/js/bootstrap.min.js",
                        "~/Assets/client/js/bootstrap-slider.min.js",
                        "~/Assets/client/js/bootstrap-select.min.js",
                        "~/Assets/client/js/parallax.js",
                        "~/Assets/client/js/revslider.js",
                        "~/Assets/client/js/common.js",
                        "~/Assets/client/js/jquery.bxslider.min.js",
                        "~/Assets/client/js/owl.carousel.min.js",
                        "~/Assets/client/js/jquery.mobile-menu.min.js",
                        "~/Assets/client/js/countdown.js",
                        "~/Assets/client/js/jquery-ui.js",
                        "~/Assets/client/js/controller/SearchController.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/showroomjs").Include(
                "~/Assets/showroom/vendor/jquery/jquery.min.js",
                "~/Scripts/jquery.unobtrusive-ajax.min.js",
                "~/Assets/showroom/vendor/bootstrap/js/bootstrap.bundle.min.js",
                "~/Assets/showroom/vendor/jquery-easing/jquery.easing.min.js",
                "~/Assets/showroom/js/sb-admin-2.min.js",
                "~/Assets/showroom/vendor/chart.js/Chart.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/dealerjs").Include(
                "~/Assets/dealer/vendor/jquery/jquery.min.js",
                "~/Scripts/jquery.unobtrusive-ajax.min.js",
                "~/Assets/dealer/vendor/bootstrap/js/bootstrap.bundle.min.js",
                "~/Assets/dealer/vendor/jquery-easing/jquery.easing.min.js",
                "~/Assets/dealer/js/sb-admin-2.min.js",              
                "~/Assets/dealer/vendor/chart.js/Chart.min.js"
                ));

         //css
          bundles.Add(new StyleBundle("~/bundles/clientcss").Include(
                        "~/Assets/client/css/bootstrap.min.css",
                        "~/Assets/client/css/font-awesome.css",
                        "~/Assets/client/css/bootstrap-select.css",
                        "~/Assets/client/css/revslider.css",
                        "~/Assets/client/css/owl.carousel.css",
                        "~/Assets/client/css/owl.theme.css",
                        "~/Assets/client/css/jquery.bxslider.css",
                        "~/Assets/client/css/jquery.mobile-menu.css",
                        "~/Assets/client/css/style.css",
                        "~/Assets/client/css/responsive.css",
                        "~/Assets/client/css/bootstrap-social.css",
                        "~/Assets/client/css/jquery-ui.css"
                        ));

            bundles.Add(new StyleBundle("~/bundles/showroomcss").Include(
                "~/Assets/showroom/vendor/fontawesome-free/css/all.min.css",
                "~/Assets/showroom/css/sb-admin-2.min.css",
                "~/Content/PagedList.css"
                ));

            bundles.Add(new StyleBundle("~/bundles/dealercss").Include(
                "~/Assets/showroom/vendor/fontawesome-free/css/all.min.css",
                "~/Assets/dealer/css/sb-admin-2.min.css",
                "~/Content/PagedList.css"
                ));

            BundleTable.EnableOptimizations = true;
        }
    }
}

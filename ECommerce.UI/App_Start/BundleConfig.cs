using System.Web;
using System.Web.Optimization;

namespace ECommerce.UI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/site/css").Include(
                "~/Theme/css/bootstrap.css",
                "~/Theme/css/style.css",
                "~/Theme/css/menu.css",
                "~/Theme/css/ken-burns.css",
                "~/Theme/css/animate.min.css",
                "~/Theme/css/owl.carousel.css",
                "~/Theme/css/font-awesome.css"
                ));

            bundles.Add(new ScriptBundle("~/site/js").Include(
                "~/Theme/js/owl.carousel.js",
                "~/Theme/js/owl.carousel.js",
                "~/Theme/js/owl.carousel.js",
                "~/Theme/js/move-top.js",
                "~/Theme/js/easing.js",
                "~/Theme/js/bootstrap.js",
                "~/Theme/js/jquery.marquee.min.js",
                "~/Theme/js/jquery-scrolltofixed-min.js"
                ));
        }
    }
}

using System.Web;
using System.Web.Optimization;

namespace TaxManagementSystem.Web
{
    public class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/js/jquery-3.3.1.min.js",
                        "~/Content/js/bootstrap.js"
                        ));
            

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/bootstrap.min.css"));
        }
    }
}

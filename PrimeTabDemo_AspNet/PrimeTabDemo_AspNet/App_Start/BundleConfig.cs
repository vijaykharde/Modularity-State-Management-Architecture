using System.Web;
using System.Web.Optimization;

namespace PrimeTabDemo_AspNet
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            /*bundles.Add(new StyleBundle("~/Content/PrimeTabNg/css").Include(
                      "~/Content/PrimeTabDemoNgCss/styles.0d9dd8f239cf21f51dde.css"));

            bundles.Add(new ScriptBundle("~/bundles/PrimeTabNg/scripts").Include(
                      "~/Scripts/PrimeTabDemoNg/main-es2015.js",
                      "~/Scripts/PrimeTabDemoNg/main-es5.js",
                      "~/Scripts/PrimeTabDemoNg/polyfills-es2015.js",
                      "~/Scripts/PrimeTabDemoNg/polyfills-es5.js",
                      "~/Scripts/PrimeTabDemoNg/runtime-es2015.js",
                      "~/Scripts/PrimeTabDemoNg/runtime-es5.js",
                      "~/Scripts/PrimeTabDemoNg/styles-es5.js",
                      "~/Scripts/PrimeTabDemoNg/styles-es2015.js",
                      "~/Scripts/PrimeTabDemoNg/vendor-es5.js",
                      "~/Scripts/PrimeTabDemoNg/vendor-es2015.js"));*/
        }
    }
}

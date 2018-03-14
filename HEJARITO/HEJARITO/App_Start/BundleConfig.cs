﻿using System.Web;
using System.Web.Optimization;

namespace HEJARITO
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        // JG 2018-03-12: Scripts for rendering the Teacher tables.
                        "~/Scripts/jquery.dataTables.min.js",
                        "~/Scripts/dataTables.bootstrap.min.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                       "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",

                      // JG 2018-03-12: Styling for rendering the Teacher tables.
                      "~/Content/bootstrap.min.css",
                      "~/Content/dataTables.bootstrap.min.css",

                      "~/Content/site.css"));
        }
    }
}

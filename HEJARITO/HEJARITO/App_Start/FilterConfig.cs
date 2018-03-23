using HEJARITO.Models;
using System.Web;
using System.Web.Mvc;

namespace HEJARITO
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new HejaRiToExceptionFilter());
        }
    }
}

using System.Web;
using System.Web.Mvc;

namespace LibraryManagement
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeAttribute());

            HttpContext.Current.Application.Lock();
            HttpContext.Current.Application["Limit"] = 10;
            HttpContext.Current.Application.UnLock();

        }
    }
}

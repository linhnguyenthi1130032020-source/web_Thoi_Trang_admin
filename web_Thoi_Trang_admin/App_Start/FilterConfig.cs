using System.Web;
using System.Web.Mvc;

namespace web_Thoi_Trang_admin
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}

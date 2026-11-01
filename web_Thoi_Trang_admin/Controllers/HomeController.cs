using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_Thoi_Trang_admin.Models;

namespace web_Thoi_Trang_admin.Controllers
{
    public class HomeController : Controller
    {
        //private WebThoiTrangEntities db = new WebThoiTrangEntities();
        public ActionResult Index()
        {
            return View();
        }

        ////public ActionResult About()
        ////{
        ////    ViewBag.Message = "Giới thiệu về thương hiệu thời trang NOVEA - Phong cách hiện đại và thanh lịch.";

        ////    return View();
        ////}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Liên hệ với bộ phận chăm sóc của cửa hàng thời trang NOVEA để được tư vấn và hỗ trợ nhanh nhất.";

        //    return View();
        //}
    }
}
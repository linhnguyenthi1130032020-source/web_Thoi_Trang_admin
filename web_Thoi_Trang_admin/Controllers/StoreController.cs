using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_Thoi_Trang_admin.Models; // Sử dụng đúng Namespace của dự án

namespace web_Thoi_Trang_admin.Controllers
{
    public class StoreController : Controller
    {
        // Khởi tạo kết nối cơ sở dữ liệu Entity Framework thực tế
        private WebThoiTrangEntities db = new WebThoiTrangEntities();

        // GET: Store/Index
        public ActionResult Index()
        {
            // Khởi tạo ViewModel tổng hợp chứa cả Danh mục và Sản phẩm
            var model = new StoreHomeViewModel
            {
                // Lấy danh mục từ bảng DanhMuc trong CSDL của bạn
                Categories = db.DanhMuc
                               .OrderBy(c => c.TenDanhMuc)
                               .ToList(),

                // Lấy danh sách sản phẩm và lấy giá từ bảng Biến thể sản phẩm
                Products = db.SanPham.Select(p => new ProductViewModel
                {
                    ProductID = p.MaSanPham,
                    NamePro = p.TenSanPham,
                    Price = p.BienTheSanPham.FirstOrDefault() != null
                            ? p.BienTheSanPham.FirstOrDefault().GiaBan
                            : 0,
                    ImagePro = p.HinhAnhDaiDien,
                    CategoryName = p.DanhMuc != null ? p.DanhMuc.TenDanhMuc : ""
                }).ToList()
            };

            // Truyền StoreHomeViewModel sang View Index.cshtml
            return View(model);
        }

        // Giải phóng kết nối cơ sở dữ liệu sau khi hoàn tất request
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
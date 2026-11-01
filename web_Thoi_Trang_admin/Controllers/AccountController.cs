using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using web_Thoi_Trang_admin.Models;

namespace web_Thoi_Trang_admin.Controllers
{
    public class AccountController : Controller
    {
        // Khởi tạo đối tượng kết nối cơ sở dữ liệu Entity Framework
        private WebThoiTrangEntities db = new WebThoiTrangEntities();

        // ================= 1. ĐĂNG NHẬP (LOGIN) =================
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password, bool rememberMe = false)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.");
                return View();
            }

            try
            {
                // Truy vấn tìm người dùng trong cơ sở dữ liệu
                var user = db.NguoiDung.FirstOrDefault(u => u.TenDangNhap == username && u.MatKhau == password && u.TrangThai == true);

                if (user != null)
                {
                    // Lưu thông tin phiên làm việc vào Session
                    Session["MaNguoiDung"] = user.MaNguoiDung;
                    Session["TenDangNhap"] = user.TenDangNhap;
                    Session["HoVaTen"] = user.HoVaTen;
                    Session["VaiTro"] = user.VaiTro;

                    // Tạo cookie xác thực
                    FormsAuthentication.SetAuthCookie(user.TenDangNhap, rememberMe);

                    // --- PHÂN QUYỀN ĐĂNG NHẬP ---
                    if (user.VaiTro == "Admin")
                    {
                        return RedirectToAction("Index", "SanPhams");
                    }
                    return RedirectToAction("Index", "Home");
                }
                // 🛑 NẾU SAI TÀI KHOẢN HOẶC MẬT KHẨU -> BÁO LỖI VÀ TRẢ VỀ VIEW
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không chính xác.");
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi hệ thống: " + ex.Message);
                return View();
            }
            if (Session["VaiTro"] == null || Session["VaiTro"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }
            ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không chính xác.");
            return View();
        }


        // ================= 2. ĐĂNG KÝ (REGISTER) =================
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(string fullname, string email, string password, string confirmPassword)
        {
            // Kiểm tra khớp mật khẩu
            if (password != confirmPassword)
            {
                ModelState.AddModelError("", "Mật khẩu xác nhận không khớp.");
                return View();
            }

            try
            {
                // Kiểm tra xem Email/Tên đăng nhập đã tồn tại trong CSDL chưa
                bool isExist = db.NguoiDung.Any(u => u.TenDangNhap == email || u.Email == email);
                if (isExist)
                {
                    ModelState.AddModelError("", "Email hoặc tài khoản này đã tồn tại trong hệ thống.");
                    return View();
                }

                // Tạo đối tượng NguoiDung mới
                NguoiDung newUser = new NguoiDung
                {
                    TenDangNhap = email,       // Dùng email làm tên đăng nhập
                    Email = email,
                    MatKhau = password,
                    HoVaTen = fullname,
                    VaiTro = "KhachHang",      // Phân quyền mặc định là Khách hàng
                    TrangThai = true,          // Kích hoạt tài khoản
                    NgayTao = DateTime.Now
                };

                // Thêm vào bảng NguoiDung và lưu xuống CSDL
                db.NguoiDung.Add(newUser);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Đăng ký tài khoản thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi đăng ký: " + ex.Message);
                return View();
            }
        }


        // ================= 3. ĐĂNG XUẤT (LOGOUT) =================
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

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
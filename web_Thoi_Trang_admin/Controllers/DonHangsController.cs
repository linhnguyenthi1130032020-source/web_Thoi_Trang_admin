using System;
using System.Collections.Generic;

using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using web_Thoi_Trang_admin.Models;

namespace web_Thoi_Trang_admin.Controllers
{
    public class DonHangsController : Controller
    {
        private WebThoiTrangEntities db = new WebThoiTrangEntities();

        // GET: DonHangs
        public ActionResult Index()
        {
            var donHang = db.DonHang.Include(d => d.NguoiDung);
            return View(donHang.ToList());
        }

        // GET: DonHangs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHang.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }

        // GET: DonHangs/Create
        public ActionResult Create()
        {
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDung, "MaNguoiDung", "TenDangNhap");
            return View();
        }

        // POST: DonHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDonHang,MaNguoiDung,TenNguoiNhan,SoDienThoaiNhan,EmailNguoiNhan,DiaChiGiaoHang,TongTien,PhuongThucThanhToan,TrangThaiThanhToan,GhiChu,NgayDatHang,TrangThaiDonHang")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                db.DonHang.Add(donHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaNguoiDung = new SelectList(db.NguoiDung, "MaNguoiDung", "TenDangNhap", donHang.MaNguoiDung);
            return View(donHang);
        }

        // GET: DonHangs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHang.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDung, "MaNguoiDung", "TenDangNhap", donHang.MaNguoiDung);
            return View(donHang);
        }

        // POST: DonHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDonHang,MaNguoiDung,TenNguoiNhan,SoDienThoaiNhan,EmailNguoiNhan,DiaChiGiaoHang,TongTien,PhuongThucThanhToan,TrangThaiThanhToan,GhiChu,NgayDatHang,TrangThaiDonHang")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDung, "MaNguoiDung", "TenDangNhap", donHang.MaNguoiDung);
            return View(donHang);
        }

        // GET: DonHangs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHang.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }

        // POST: DonHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DonHang donHang = db.DonHang.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }

            // 1. Xóa các chi tiết đơn hàng liên quan trước để tránh lỗi ràng buộc khóa ngoại (Constraint Conflict)
            var chiTietList = db.ChiTietDonHang.Where(ct => ct.MaDonHang == id).ToList();
            if (chiTietList != null && chiTietList.Count > 0)
            {
                db.ChiTietDonHang.RemoveRange(chiTietList);
            }

            // 2. Sau đó mới tiến hành xóa đơn hàng chính
            db.DonHang.Remove(donHang);
            db.SaveChanges();

            return RedirectToAction("Index");
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

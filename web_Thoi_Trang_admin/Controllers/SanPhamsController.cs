using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using web_Thoi_Trang_admin.Models;

namespace web_Thoi_Trang_admin.Controllers
{
    public class SanPhamsController : Controller
    {
        private WebThoiTrangEntities db = new WebThoiTrangEntities();

        public ActionResult Index()
        {
            var sanPhams = db.SanPham.Include(s => s.DanhMuc).ToList();
            return View(sanPhams);
        }
        // GET: SanPhams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SanPham sanPham = db.SanPham
                .Include(s => s.DanhMuc)
                .Include(s => s.BienTheSanPham)
                .FirstOrDefault(s => s.MaSanPham == id);

            if (sanPham == null)
            {
                return HttpNotFound();
            }

            return View(sanPham);
        }

        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                SanPham sanPham = db.SanPham
                    .Include(s => s.BienTheSanPham)
                    .FirstOrDefault(s => s.MaSanPham == id);

                if (sanPham != null)
                {
                    // 1. Xóa các biến thể thuộc sản phẩm trước để tránh lỗi khóa ngoại
                    if (sanPham.BienTheSanPham != null && sanPham.BienTheSanPham.Count > 0)
                    {
                        db.BienTheSanPham.RemoveRange(sanPham.BienTheSanPham);
                    }

                    // 2. Xóa sản phẩm chính
                    db.SanPham.Remove(sanPham);
                    db.SaveChanges();
                }

                TempData["SuccessMessage"] = "Xóa sản phẩm thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Không thể xóa sản phẩm này do đang có dữ liệu liên quan: " + ex.Message);
                var sanPham = db.SanPham.Find(id);
                return View(sanPham);
            }
        }
        // GET: SanPhams/Create
        public ActionResult Create()
        {
            ViewBag.MaDanhMuc = new SelectList(db.DanhMuc, "MaDanhMuc", "TenDanhMuc");
            var model = new ProductEditViewModel
            {
                BienThes = new List<ProductVariantViewModel>() // Khởi tạo danh sách biến thể tránh null
            };
            return View(model);
        }

        // POST: SanPhams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string fileName = "default.png";

                    // 1. Xử lý upload file hình ảnh đại diện
                    if (model.HinhAnhDaiDien != null && model.HinhAnhDaiDien.ContentLength > 0)
                    {
                        fileName = Path.GetFileName(model.HinhAnhDaiDien.FileName);
                        string path = Path.Combine(Server.MapPath("~/Content/images/products/"), fileName);

                        Directory.CreateDirectory(Server.MapPath("~/Content/images/products/"));
                        model.HinhAnhDaiDien.SaveAs(path);
                    }

                    // 2. Tạo đối tượng SanPham để lưu vào CSDL
                    var sanPham = new SanPham
                    {
                        TenSanPham = model.TenSanPham,
                        MoTa = model.MoTa,
                        HinhAnhDaiDien = fileName, // Lưu tên file dạng string xuống DB
                        MaDanhMuc = model.MaDanhMuc,
                        TrangThai = model.TrangThai
                    };

                    db.SanPham.Add(sanPham);
                    db.SaveChanges(); // Lưu để lấy ra MaSanPham tự tăng

                    // 3. Thêm các biến thể đi kèm (nếu có)
                    if (model.BienThes != null && model.BienThes.Count > 0)
                    {
                        foreach (var item in model.BienThes)
                        {
                            db.BienTheSanPham.Add(new BienTheSanPham
                            {
                                MaSanPham = sanPham.MaSanPham,
                                MaSKU = item.MaSKU,
                                KichCo = item.KichCo,
                                MauSac = item.MauSac,
                                GiaBan = item.GiaBan,
                                SoLuongTon = item.SoLuongTon,
                                TrangThai = item.TrangThai
                            });
                        }
                        db.SaveChanges();
                    }

                    TempData["SuccessMessage"] = "Thêm mới sản phẩm thành công!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi hệ thống khi lưu: " + ex.Message);
                }
            }

            ViewBag.MaDanhMuc = new SelectList(db.DanhMuc, "MaDanhMuc", "TenDanhMuc", model.MaDanhMuc);
            return View(model);
        }

        // GET: SanPhams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SanPham sanPham = db.SanPham.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }

            var model = new ProductEditViewModel
            {
                MaSanPham = sanPham.MaSanPham,
                TenSanPham = sanPham.TenSanPham,
                MoTa = sanPham.MoTa,
                MaDanhMuc = sanPham.MaDanhMuc,
                TrangThai = sanPham.TrangThai,
                BienThes = db.BienTheSanPham
                    .Where(b => b.MaSanPham == id)
                    .Select(b => new ProductVariantViewModel
                    {
                        MaBienThe = b.MaBienThe,
                        MaSKU = b.MaSKU,
                        KichCo = b.KichCo,
                        MauSac = b.MauSac,
                        GiaBan = b.GiaBan,
                        SoLuongTon = b.SoLuongTon,
                        TrangThai = b.TrangThai
                    }).ToList()
            };

            ViewBag.MaDanhMuc = new SelectList(db.DanhMuc, "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);
            return View(model);
        }

        // POST: SanPhams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var sanPham = db.SanPham.Find(model.MaSanPham);
                    if (sanPham == null) return HttpNotFound();

                    // Xử lý upload ảnh mới nếu có chọn file, nếu không giữ nguyên ảnh cũ
                    if (model.HinhAnhDaiDien != null && model.HinhAnhDaiDien.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(model.HinhAnhDaiDien.FileName);
                        string path = Path.Combine(Server.MapPath("~/Content/images/products/"), fileName);

                        Directory.CreateDirectory(Server.MapPath("~/Content/images/products/"));
                        model.HinhAnhDaiDien.SaveAs(path);

                        sanPham.HinhAnhDaiDien = fileName;
                    }

                    sanPham.TenSanPham = model.TenSanPham;
                    sanPham.MoTa = model.MoTa;
                    sanPham.MaDanhMuc = model.MaDanhMuc;
                    sanPham.TrangThai = model.TrangThai;
                    db.Entry(sanPham).State = EntityState.Modified;

                    // Đồng bộ danh sách biến thể (Thêm mới, Cập nhật, Xóa)
                    var existingVariants = db.BienTheSanPham.Where(b => b.MaSanPham == model.MaSanPham).ToList();
                    var formVariantIds = model.BienThes.Select(b => b.MaBienThe).ToList();

                    var variantsToRemove = existingVariants.Where(b => !formVariantIds.Contains(b.MaBienThe)).ToList();
                    db.BienTheSanPham.RemoveRange(variantsToRemove);

                    foreach (var item in model.BienThes)
                    {
                        if (item.MaBienThe == 0)
                        {
                            db.BienTheSanPham.Add(new BienTheSanPham
                            {
                                MaSanPham = model.MaSanPham,
                                MaSKU = item.MaSKU,
                                KichCo = item.KichCo,
                                MauSac = item.MauSac,
                                GiaBan = item.GiaBan,
                                SoLuongTon = item.SoLuongTon,
                                TrangThai = item.TrangThai

                            });
                        }
                        else
                        {
                            var variant = db.BienTheSanPham.Find(item.MaBienThe);
                            if (variant != null)
                            {
                                variant.MaSKU = item.MaSKU;
                                variant.KichCo = item.KichCo;
                                variant.MauSac = item.MauSac;
                                variant.GiaBan = item.GiaBan;
                                variant.SoLuongTon = item.SoLuongTon;
                                variant.TrangThai = item.TrangThai;
                                db.Entry(variant).State = EntityState.Modified;
                            }
                        }
                    }

                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi cập nhật: " + ex.Message);
                }
            }

            ViewBag.MaDanhMuc = new SelectList(db.DanhMuc, "MaDanhMuc", "TenDanhMuc", model.MaDanhMuc);
            return View(model);
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
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using web_Thoi_Trang_admin.Models;
using System.IO;
using System.Web;
namespace web_Thoi_Trang_admin.Controllers
{
    public class DanhMucsController : Controller
    {
        // Khởi tạo ngữ cảnh cơ sở dữ liệu thời trang
        private WebThoiTrangEntities db = new WebThoiTrangEntities();

        // GET: DanhMucs (Trang danh sách danh mục)
        public ActionResult Index()
        {
            return View(db.DanhMuc.ToList());
        }

        // GET: DanhMucs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMuc category = db.DanhMuc.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: DanhMucs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DanhMucs/Create
        // POST: DanhMucs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDanhMuc,TenDanhMuc,MoTa,TrangThai")] DanhMuc danhMuc)
        {
            if (ModelState.IsValid)
            {
                // Đảm bảo TrangThai có giá trị mặc định true nếu checkbox không được tích
                // (Tránh lỗi null hoặc vi phạm ràng buộc NOT NULL trong CSDL)

                db.DanhMuc.Add(danhMuc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(danhMuc);
        }

        // GET: DanhMucs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMuc category = db.DanhMuc.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: DanhMucs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryID,CategoryName")] DanhMuc category) // Đã đồng bộ thành CategoryID
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: DanhMucs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMuc category = db.DanhMuc.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: DanhMucs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DanhMuc category = db.DanhMuc.Find(id);
            db.DanhMuc.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        private string SaveImage(HttpPostedFileBase imageFile)
        {
            string originalFileName = Path.GetFileName(imageFile.FileName);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
            string extension = Path.GetExtension(originalFileName).ToLower();

            // Thư mục vật lý dùng để lưu ảnh trong project
            string folder = Server.MapPath("~/Content/images");

            // Nếu thư mục images chưa tồn tại thì tạo mới
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string fileName = fileNameWithoutExtension + extension;
            string path = Path.Combine(folder, fileName);

            // Nếu đã có file trùng tên, tự thêm -1, -2, ... để tránh ghi đè
            int count = 1;
            while (System.IO.File.Exists(path))
            {
                fileName = fileNameWithoutExtension + "-" + count + extension;
                path = Path.Combine(folder, fileName);
                count++;
            }

            // Lưu file vào thư mục
            imageFile.SaveAs(path);

            // Trả về tên file để lưu vào cơ sở dữ liệu
            return fileName;
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

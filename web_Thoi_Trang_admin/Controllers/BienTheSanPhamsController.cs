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
    public class BienTheSanPhamsController : Controller
    {
        private WebThoiTrangEntities db = new WebThoiTrangEntities();

        // GET: BienTheSanPhams
        public ActionResult Index()
        {
            var bienTheSanPham = db.BienTheSanPham.Include(b => b.SanPham);
            return View(bienTheSanPham.ToList());
        }

        // GET: BienTheSanPhams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BienTheSanPham bienTheSanPham = db.BienTheSanPham.Find(id);
            if (bienTheSanPham == null)
            {
                return HttpNotFound();
            }
            return View(bienTheSanPham);
        }

        // GET: BienTheSanPhams/Create
        public ActionResult Create()
        {
            ViewBag.MaSanPham = new SelectList(db.SanPham, "MaSanPham", "TenSanPham");
            return View();
        }

        // POST: BienTheSanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaBienThe,MaSanPham,MaSKU,KichCo,MauSac,GiaBan,SoLuongTon,TrangThai")] BienTheSanPham bienTheSanPham)
        {
            if (ModelState.IsValid)
            {
                db.BienTheSanPham.Add(bienTheSanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaSanPham = new SelectList(db.SanPham, "MaSanPham", "TenSanPham", bienTheSanPham.MaSanPham);
            return View(bienTheSanPham);
        }

        // GET: BienTheSanPhams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BienTheSanPham bienTheSanPham = db.BienTheSanPham.Find(id);
            if (bienTheSanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaSanPham = new SelectList(db.SanPham, "MaSanPham", "TenSanPham", bienTheSanPham.MaSanPham);
            return View(bienTheSanPham);
        }

        // POST: BienTheSanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaBienThe,MaSanPham,MaSKU,KichCo,MauSac,GiaBan,SoLuongTon,TrangThai")] BienTheSanPham bienTheSanPham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bienTheSanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaSanPham = new SelectList(db.SanPham, "MaSanPham", "TenSanPham", bienTheSanPham.MaSanPham);
            return View(bienTheSanPham);
        }

        // GET: BienTheSanPhams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BienTheSanPham bienTheSanPham = db.BienTheSanPham.Find(id);
            if (bienTheSanPham == null)
            {
                return HttpNotFound();
            }
            return View(bienTheSanPham);
        }

        // POST: BienTheSanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BienTheSanPham bienTheSanPham = db.BienTheSanPham.Find(id);
            db.BienTheSanPham.Remove(bienTheSanPham);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopBanAlbum.DAL;
using System.Linq.Dynamic.Core;
using System.Net;
using ShopBanAlbum.Models;
using System.Data.Entity;
using ShopBanAlbum.Filters;

namespace ShopBanAlbum.Areas.Admin.Controllers
{
    [LoginAdmin]
    public class KhachHangController : Controller
    {
        private ShopMusicAlbumContext db = new ShopMusicAlbumContext();
        // GET: Admin/KhachHang
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData()
        {
            try
            {
                //Creating instance of DatabaseContext class
                using (ShopMusicAlbumContext _context = new ShopMusicAlbumContext())
                {
                    var draw = Request.Form.GetValues("draw").FirstOrDefault();
                    var start = Request.Form.GetValues("start").FirstOrDefault();
                    var length = Request.Form.GetValues("length").FirstOrDefault();
                    var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                    var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                    var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();


                    //Paging Size (10,20,50,100)  
                    int pageSize = length != null ? Convert.ToInt32(length) : 0;
                    int skip = start != null ? Convert.ToInt32(start) : 0;
                    int recordsTotal = 0;

                    // Getting all Book data  
                    var khachHangData = (from khachHang in _context.KhachHangs
                                       select new
                                       {
                                           khachHang.KhachHangID,
                                           khachHang.TenKhachHang,
                                           khachHang.SDT,
                                           khachHang.DiaChi,
                                           khachHang.EmailKhachHang,
                                           khachHang.DiemKhachHang
                                       });


                    //Sorting  
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        khachHangData = khachHangData.OrderBy(sortColumn + " " + sortColumnDir);
                    }

                    //Search  
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        khachHangData = khachHangData.Where(m => m.TenKhachHang.Contains(searchValue)
                        || m.SDT.ToString().Contains(searchValue) || m.EmailKhachHang.ToString().Contains(searchValue) 
                        || m.DiaChi.ToString().Contains(searchValue));
                    }

                    //total number of rows count   
                    recordsTotal = khachHangData.Count();
                    //Paging   
                    var data = khachHangData.Skip(skip).Take(pageSize).ToList();
                    //Returning Json Data  
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public JsonResult DeleteKhachHang(int? ID)
        {
            using (ShopMusicAlbumContext _context = new ShopMusicAlbumContext())
            {
                var khachHang = _context.KhachHangs.Find(ID);
                if (ID == null)
                    return Json(data: "Not Deleted", behavior: JsonRequestBehavior.AllowGet);
                _context.KhachHangs.Remove(khachHang);
                _context.SaveChanges();

                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Admin/DonHang/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.DonHang = khachHang;
            var chiTietDonHang = db.ChiTietDonHangs.Where(m => m.DonHangID == id);
            ViewBag.Total = db.ChiTietDonHangs.Sum(m => m.SoLuong * m.GiaBan);
            return View(chiTietDonHang.ToList());
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KhachHangID,TenKhachHang,EmailKhachHang,DiaChi,DienThoai,DiemKhachHang")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khachHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(khachHang);
        }
    }
}

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
using System.IO;

namespace ShopBanAlbum.Areas.Admin.Controllers
{
    [LoginAdmin]
    public class AlbumController : Controller
    {
        private ShopMusicAlbumContext db = new ShopMusicAlbumContext();
        // GET: Admin/Album
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
                    var albumData = (from album in _context.Albums
                                       join tacGia in _context.TacGias on album.TacGiaID equals tacGia.TacGiaID
                                       join theLoai in _context.TheLoais on album.TheLoaiID equals theLoai.TheLoaiID
                                       join quocGia in _context.QuocGias on album.QuocGiaID equals quocGia.QuocGiaID
                                       select new
                                       {
                                           album.AlbumID,
                                           album.TenAlbum,
                                           album.GiaBan,
                                           album.SoLuong,
                                           album.NgayPhatHanh,
                                           album.DaBan,
                                           album.XuatXu,
                                           album.PhuKien,
                                           album.HinhAnh,
                                           TheLoai = theLoai.TenTheLoai,
                                           TacGia = tacGia.TenTacGia,
                                           QuocGia = quocGia.TenQuocGia,
                                       });


                    //Sorting  
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        albumData = albumData.OrderBy(sortColumn + " " + sortColumnDir);
                    }

                    //Search  
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        albumData = albumData.Where(m => m.TenAlbum.Contains(searchValue)
                        || m.GiaBan.ToString().Contains(searchValue) || m.SoLuong.ToString().Contains(searchValue));
                    }

                    //total number of rows count   
                    recordsTotal = albumData.Count();
                    //Paging   
                    var data = albumData.Skip(skip).Take(pageSize).ToList();
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
        public JsonResult DeleteAlbum(int? ID)
        {
            using (ShopMusicAlbumContext _context = new ShopMusicAlbumContext())
            {
                var album = _context.Albums.Find(ID);
                if (ID == null)
                    return Json(data: "Not Deleted", behavior: JsonRequestBehavior.AllowGet);
                _context.Albums.Remove(album);
                _context.SaveChanges();

                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
        }
        // GET: Admin/Album/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }
        // GET: Admin/Album/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            ViewBag.QuocGiaID = new SelectList(db.QuocGias, "QuocGiaID", "TenQuocGia", album.QuocGiaID);
            ViewBag.TheLoaiID = new SelectList(db.TheLoais, "TheLoaiID", "TenTheLoai", album.TheLoaiID);
            ViewBag.TacGiaID = new SelectList(db.TacGias, "TacGiaID", "TenTacGia", album.TacGiaID);
            return View(album);
        }

        // POST: Admin/Album/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AlbumID,TenAlbum,HinhAnh,NgayPhatHanh,GiaBan,SoLuong,DaBan,DiemDanhGia,XuatXu,PhuKien,TheLoaiID,QuocGiaID,TacGiaID")] Album album, HttpPostedFileBase HinhAnh)
        {
            if (ModelState.IsValid)
            {
                if (HinhAnh != null)
                {
                    var fileName = Path.GetFileName(HinhAnh.FileName);
                    album.HinhAnh = fileName;
                    string path = Path.Combine(Server.MapPath("~/Contents/images/album"), fileName);
                    HinhAnh.SaveAs(path);
                    db.Entry(album).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            ViewBag.QuocGiaID = new SelectList(db.QuocGias, "QuocGiaID", "TenQuocGia", album.QuocGiaID);
            ViewBag.TheLoaiID = new SelectList(db.TheLoais, "TheLoaiID", "TenTheLoai", album.TheLoaiID);
            ViewBag.TacGiaID = new SelectList(db.TacGias, "TacGiaID", "TenTacGia", album.TacGiaID);
            return View(album);
        }
        // GET: Admin/SanPham/Create
        public ActionResult Create()
        {
            ViewBag.QuocGiaID = new SelectList(db.QuocGias, "QuocGiaID", "TenQuocGia");
            ViewBag.TheLoaiID = new SelectList(db.TheLoais, "TheLoaiID", "TenTheLoai");
            ViewBag.TacGiaID = new SelectList(db.TacGias, "TacGiaID", "TenTacGia");
            return View();
        }

        // POST: Admin/SanPham/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AlbumID,TenAlbum,HinhAnh,NgayPhatHanh,GiaBan,SoLuong,DaBan,DiemDanhGia,XuatXu,PhuKien,TheLoaiID,QuocGiaID,TacGiaID")] Album album, HttpPostedFileBase HinhAnh)
        {
            if (ModelState.IsValid)
            {
                var fileName = Path.GetFileName(HinhAnh.FileName);
                album.HinhAnh = fileName;
                album.DiemDanhGia = 5;
                string path = Path.Combine(Server.MapPath("~/Contents/images/album"), fileName);
                HinhAnh.SaveAs(path);
                db.Albums.Add(album);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.QuocGiaID = new SelectList(db.QuocGias, "QuocGiaID", "TenQuocGia",album.QuocGiaID);
            ViewBag.TheLoaiID = new SelectList(db.TheLoais, "TheLoaiID", "TenTheLoai", album.TheLoaiID);
            ViewBag.TacGiaID = new SelectList(db.TacGias, "TacGiaID", "TenTacGia", album.TacGiaID); 
            return View(album);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopBanAlbum.DAL;
using ShopBanAlbum.Models;
using System.Linq.Dynamic.Core;
using ShopBanAlbum.Filters;

namespace ShopBanAlbum.Areas.Admin.Controllers
{
    [LoginAdmin]
    public class NhanVienController : Controller
    {
        private ShopMusicAlbumContext db = new ShopMusicAlbumContext();

        // GET: Admin/NhanVien
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
                    var nhanVienData = (from nhanVien in _context.NhanViens
                                         select new
                                         {
                                             nhanVien.NhanVienID,
                                             nhanVien.TenNhanVien,
                                             nhanVien.EmailNhanVien,
                                             nhanVien.MatKhauNhanVien,
                                         });


                    //Sorting  
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        nhanVienData = nhanVienData.OrderBy(sortColumn + " " + sortColumnDir);
                    }

                    //Search  
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        nhanVienData = nhanVienData.Where(m => m.TenNhanVien.Contains(searchValue)
                        || m.EmailNhanVien.ToString().Contains(searchValue));
                    }

                    //total number of rows count   
                    recordsTotal = nhanVienData.Count();
                    //Paging   
                    var data = nhanVienData.Skip(skip).Take(pageSize).ToList();
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
        public JsonResult DeleteNhanVien(int? ID)
        {
            using (ShopMusicAlbumContext _context = new ShopMusicAlbumContext())
            {
                var nhanVien = _context.NhanViens.Find(ID);
                if (ID == null)
                    return Json(data: "Not Deleted", behavior: JsonRequestBehavior.AllowGet);
                _context.NhanViens.Remove(nhanVien);
                _context.SaveChanges();

                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Admin/NhanVien/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/NhanVien/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NhanVienID,TenNhanVien,EmailNhanVien,MatKhauNhanVien")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                db.NhanViens.Add(nhanVien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nhanVien);
        }

        // GET: Admin/NhanVien/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // POST: Admin/NhanVien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NhanVienID,TenNhanVien,EmailNhanVien,MatKhauNhanVien")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhanVien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nhanVien);
        }
    }
}

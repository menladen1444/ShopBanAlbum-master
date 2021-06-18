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
    public class BaiHatController : Controller
    {
        private ShopMusicAlbumContext db = new ShopMusicAlbumContext();

        // GET: Admin/BaiHat
        public ActionResult Index()
        {
            var baiHats = db.BaiHats.Include(b => b.Album);
            return View(baiHats.ToList());
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
                    var baiHatData = (from baiHat in _context.BaiHats
                                      join album in _context.Albums on baiHat.AlbumID equals album.AlbumID
                                      select new
                                      {
                                          baiHat.BaiHatID,
                                          baiHat.TenBaiHat,
                                          baiHat.ThoiLuong,
                                          Album = album.TenAlbum,
                                      }) ;


                    //Sorting  
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        baiHatData = baiHatData.OrderBy(sortColumn + " " + sortColumnDir);
                    }

                    //Search  
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        baiHatData = baiHatData.Where(m => m.TenBaiHat.Contains(searchValue));
                    }

                    //total number of rows count   
                    recordsTotal = baiHatData.Count();
                    //Paging   
                    var data = baiHatData.Skip(skip).Take(pageSize).ToList();
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
        public JsonResult DeleteBaiHat(int? ID)
        {
            using (ShopMusicAlbumContext _context = new ShopMusicAlbumContext())
            {
                var baiHat = _context.BaiHats.Find(ID);
                if (ID == null)
                    return Json(data: "Not Deleted", behavior: JsonRequestBehavior.AllowGet);
                _context.BaiHats.Remove(baiHat);
                _context.SaveChanges();

                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
        }
        // GET: Admin/BaiHat/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaiHat baiHat = db.BaiHats.Find(id);
            if (baiHat == null)
            {
                return HttpNotFound();
            }
            return View(baiHat);
        }

        // GET: Admin/BaiHat/Create
        public ActionResult Create()
        {
            ViewBag.AlbumID = new SelectList(db.Albums, "AlbumID", "TenAlbum");
            return View();
        }

        // POST: Admin/BaiHat/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BaiHatID,TenBaiHat,LoiBaiHat,ThoiLuong,AlbumID")] BaiHat baiHat)
        {
            if (ModelState.IsValid)
            {
                db.BaiHats.Add(baiHat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AlbumID = new SelectList(db.Albums, "AlbumID", "TenAlbum", baiHat.AlbumID);
            return View(baiHat);
        }

        // GET: Admin/BaiHat/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaiHat baiHat = db.BaiHats.Find(id);
            if (baiHat == null)
            {
                return HttpNotFound();
            }
            ViewBag.AlbumID = new SelectList(db.Albums, "AlbumID", "TenAlbum", baiHat.AlbumID);
            return View(baiHat);
        }

        // POST: Admin/BaiHat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BaiHatID,TenBaiHat,LoiBaiHat,ThoiLuong,AlbumID")] BaiHat baiHat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(baiHat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AlbumID = new SelectList(db.Albums, "AlbumID", "TenAlbum", baiHat.AlbumID);
            return View(baiHat);
        }

    }
}

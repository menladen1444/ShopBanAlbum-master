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
    public class QuocGiaController : Controller
    {
        private ShopMusicAlbumContext db = new ShopMusicAlbumContext();

        // GET: Admin/QuocGia
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
                    var quocGiaData = (from quocGia in _context.QuocGias
                                       select new
                                       {
                                           quocGia.QuocGiaID,
                                           quocGia.TenQuocGia,
                                       });


                    //Sorting  
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        quocGiaData = quocGiaData.OrderBy(sortColumn + " " + sortColumnDir);
                    }

                    //Search  
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        quocGiaData = quocGiaData.Where(m => m.TenQuocGia.Contains(searchValue));
                    }

                    //total number of rows count   
                    recordsTotal = quocGiaData.Count();
                    //Paging   
                    var data = quocGiaData.Skip(skip).Take(pageSize).ToList();
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
        public JsonResult DeleteQuocGia(int? ID)
        {
            using (ShopMusicAlbumContext _context = new ShopMusicAlbumContext())
            {
                var quocGia = _context.QuocGias.Find(ID);
                if (ID == null)
                    return Json(data: "Not Deleted", behavior: JsonRequestBehavior.AllowGet);
                _context.QuocGias.Remove(quocGia);
                _context.SaveChanges();

                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Admin/QuocGia/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/QuocGia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QuocGiaID,TenQuocGia")] QuocGia quocGia)
        {
            if (ModelState.IsValid)
            {
                db.QuocGias.Add(quocGia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(quocGia);
        }

        // GET: Admin/QuocGia/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuocGia quocGia = db.QuocGias.Find(id);
            if (quocGia == null)
            {
                return HttpNotFound();
            }
            return View(quocGia);
        }

        // POST: Admin/QuocGia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QuocGiaID,TenQuocGia")] QuocGia quocGia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quocGia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(quocGia);
        }
    }
}

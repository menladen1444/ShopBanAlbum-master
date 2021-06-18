using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopBanAlbum.DAL;
using ShopBanAlbum.Filters;
using ShopBanAlbum.Models;

namespace ShopBanAlbum.Areas.Admin.Controllers
{
    [LoginAdmin]
    public class TacGiaController : Controller
    {
        private ShopMusicAlbumContext db = new ShopMusicAlbumContext();

        // GET: Admin/TacGia
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
                    var tacGiaData = (from tacGia in _context.TacGias
                                      select new
                                      {
                                          tacGia.TacGiaID,
                                          tacGia.TenTacGia,
                                          tacGia.GioiThieu,
                                          tacGia.NamSinh,
                                          tacGia.NoiSinh,
                                          tacGia.HinhAnh,
                                      });


                    //Sorting  
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        tacGiaData = tacGiaData.OrderBy(sortColumn + " " + sortColumnDir);
                    }

                    //Search  
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        tacGiaData = tacGiaData.Where(m => m.TenTacGia.Contains(searchValue)
                        || m.NoiSinh.Contains(searchValue) || m.NamSinh.ToString().Contains(searchValue));
                    }

                    //total number of rows count   
                    recordsTotal = tacGiaData.Count();
                    //Paging   
                    var data = tacGiaData.Skip(skip).Take(pageSize).ToList();
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
        public JsonResult DeleteTacGia(int? ID)
        {
            using (ShopMusicAlbumContext _context = new ShopMusicAlbumContext())
            {
                var tacGia = _context.TacGias.Find(ID);
                if (ID == null)
                    return Json(data: "Not Deleted", behavior: JsonRequestBehavior.AllowGet);
                _context.TacGias.Remove(tacGia);
                _context.SaveChanges();

                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Admin/TacGia/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/TacGia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TacGiaID,TenTacGia,GioiThieu,NamSinh,NoiSinh")] TacGia tacGia, HttpPostedFileBase HinhAnh)
        {
            if (ModelState.IsValid)
            {
                var fileName = Path.GetFileName(HinhAnh.FileName);
                tacGia.HinhAnh = fileName;
                string path = Path.Combine(Server.MapPath("~/Contents/images/Artist"), fileName);
                HinhAnh.SaveAs(path);
                db.TacGias.Add(tacGia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tacGia);
        }

        // GET: Admin/TacGia/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TacGia tacGia = db.TacGias.Find(id);
            if (tacGia == null)
            {
                return HttpNotFound();
            }
            return View(tacGia);
        }

        // POST: Admin/TacGia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TacGiaID,TenTacGia,GioiThieu,NamSinh,NoiSinh")] TacGia tacGia, HttpPostedFileBase HinhAnh)
        {
            if (ModelState.IsValid)
            {
                if (HinhAnh!=null)
                {
                    var fileName = Path.GetFileName(HinhAnh.FileName);
                    tacGia.HinhAnh = fileName;
                    string path = Path.Combine(Server.MapPath("~/Contents/images/Artist"), fileName);
                    HinhAnh.SaveAs(path);
                    db.Entry(tacGia).State = EntityState.Modified;
                    db.SaveChanges();
                }    
                
                return RedirectToAction("Index");
            }
            return View(tacGia);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopBanAlbum.DAL;
using System.Linq.Dynamic.Core;
using System.Net;
using ShopBanAlbum.Models;
using ShopBanAlbum.Filters;
using System.Data.Entity;

namespace ShopBanAlbum.Areas.Admin.Controllers
{
    [LoginAdmin]
    public class DonHangController : Controller
    {
        private ShopMusicAlbumContext db = new ShopMusicAlbumContext();
        // GET: Admin/DonHang
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
                    var donHangData = (from donHang in _context.DonHangs
                                       select new
                                       {
                                           donHang.DonHangID,
                                           donHang.TenKhachHang,
                                           donHang.Email,
                                           donHang.DiaChi,
                                           donHang.SoDienThoai,
                                           donHang.NgayDatHang,
                                           donHang.GhiChu,
                                           TenNguoiDatHang = donHang.KhachHang.TenKhachHang,
                                           TrangThaiDonHang = donHang.TrangThaiDonHang.TenTrangThaiDonHang,
                                           donHang.TrangThaiThanhToan,
                                           HinhThucThanhToan = donHang.HinhThucThanhToan.TenHinhThucThanhToan,
                                       });


                    //Sorting  
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        donHangData = donHangData.OrderBy(sortColumn + " " + sortColumnDir);
                    }

                    //Search  
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        donHangData = donHangData.Where(m => m.TenKhachHang.Contains(searchValue)
                        || m.TenNguoiDatHang.ToString().Contains(searchValue) || m.SoDienThoai.ToString().Contains(searchValue)
                        || m.TrangThaiDonHang.ToString().Contains(searchValue));
                    }

                    //total number of rows count   
                    recordsTotal = donHangData.Count();
                    //Paging   
                    var data = donHangData.Skip(skip).Take(pageSize).ToList();
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
        public JsonResult DeleteDonHang(int? ID)
        {
            using (ShopMusicAlbumContext _context = new ShopMusicAlbumContext())
            {
                var donHang = _context.DonHangs.Find(ID);
                var chiTietDonHang = _context.ChiTietDonHangs.Where(m => m.DonHangID == ID);
                if (ID == null)
                    return Json(data: "Not Deleted", behavior: JsonRequestBehavior.AllowGet);
                _context.DonHangs.Remove(donHang);
                foreach (var item in chiTietDonHang.ToList())
                {
                    var diemDanhGia = _context.DiemDanhGias.Where(m => m.ChiTietDonHangID == item.ChiTietDonHangID);
                    foreach (var item2 in diemDanhGia.ToList())
                    {
                        _context.DiemDanhGias.Remove(item2);
                    }
                    _context.ChiTietDonHangs.Remove(item);
                }
                var thongBao = _context.ThongBaos.Where(m => m.DongHangID == ID);
                foreach (var item3 in thongBao.ToList())
                {
                    _context.ThongBaos.Remove(item3);
                }
                _context.SaveChanges();

                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Admin/DonHang/Details/5
        public ActionResult Details(int? id, int? TrangThaiDonHangID)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            
            if (donHang.TrangThaiDonHangID != TrangThaiDonHangID && TrangThaiDonHangID != null)
            {
                ThongBao thongBao = new ThongBao();
                thongBao.KhachHangID = donHang.KhachHangID;
                thongBao.ThoiGian = DateTime.Now;
                thongBao.DongHangID = id;
                thongBao.TrangThai = 1;
                thongBao.TrangThaiDonHangID = TrangThaiDonHangID;
                db.ThongBaos.Add(thongBao);
                db.SaveChanges();
            }
            if (TrangThaiDonHangID == 6)
            {
                Album album = new Album();
                foreach (var item in donHang.ChiTietDonHangs)
                {
                    album = item.Album;
                    album.SoLuong += item.SoLuong;
                    db.Entry(album).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            if (donHang == null)
            {
                return HttpNotFound();
            }
            if (TrangThaiDonHangID != null && db.TrangThaiDonHangs.Find(TrangThaiDonHangID) != null)
            {
                if (TrangThaiDonHangID == 4)
                {
                    var ctdh = db.ChiTietDonHangs.Where(x => x.DonHangID == donHang.DonHangID).ToList();
                    var album = db.Albums.ToList();
                    foreach (var item in ctdh)
                    {
                        foreach (var item1 in album)
                        {
                            if (item1.AlbumID == item.AlbumID)
                            {
                                item1.DaBan += item.SoLuong;
                                db.Entry(item1).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                    }
                    if (donHang.TrangThaiThanhToan == "Chưa thanh toán")
                    {
                        donHang.TrangThaiThanhToan = "Đã thanh toán tiền mặt";
                    }    
                    if (donHang.KhachHangID != null)
                    {
                        KhachHang kh = db.KhachHangs.Find(donHang.KhachHangID);
                        decimal diem = donHang.TongTien / 100000;
                        kh.DiemKhachHang += diem;
                        db.Entry(kh).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                
                donHang.TrangThaiDonHangID = TrangThaiDonHangID;
                db.Entry(donHang).State = EntityState.Modified;
                db.SaveChanges();

            }
            ViewBag.tt = donHang.TrangThaiDonHangID;
            //ViewBag.DonHang = donHang;
            var chiTietDonHang = db.ChiTietDonHangs.Where(m => m.DonHangID == id);
            ViewBag.Total = db.ChiTietDonHangs.Sum(m => m.SoLuong * m.GiaBan);
            ViewBag.TrangThaiDonHangID = new SelectList(db.TrangThaiDonHangs, "TrangThaiDonHangID", "TenTrangThaiDonHang", donHang.TrangThaiDonHangID);
            return View(donHang);
        }
    }
}
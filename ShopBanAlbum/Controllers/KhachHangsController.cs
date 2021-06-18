using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopBanAlbum.Models;
using ShopBanAlbum.DAL;

namespace ShopBanAlbum.Controllers
{
    public class KhachHangsController : Controller
    {
        ShopMusicAlbumContext db = new ShopMusicAlbumContext();
        // GET: KhachHangs
        public ActionResult Index()
        {
            return View(db.KhachHangs.ToList());
        }

        // GET: KhachHangs/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["userName"] == null)
            {
                return RedirectToAction("DangNhap");
            }
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
        public ActionResult DangKy()
        {
            return View();
        }
        public ActionResult DatLai(int? DonHangID)
        {
            if (DonHangID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(DonHangID);
            Album album = new Album();
            foreach (var item in donHang.ChiTietDonHangs)
            {
                album = item.Album;
                album.SoLuong -= item.SoLuong;
                db.Entry(album).State = EntityState.Modified;
                db.SaveChanges();
            }
            donHang.TrangThaiDonHangID = 1;
            db.Entry(donHang).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ThongTinDonHang", new { id = DonHangID });
        }
        public ActionResult HuyDonHang(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            Album album = new Album();
            foreach (var item in donHang.ChiTietDonHangs)
            {
                album = item.Album;
                album.SoLuong += item.SoLuong;
                db.Entry(album).State = EntityState.Modified;
                db.SaveChanges();
            }
            donHang.TrangThaiDonHangID = 6;
            db.Entry(donHang).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ThongTinDonHang", new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy([Bind(Include = "KhachHangID,TenKhachHang,EmailKhachHang,SDT,MatKhauKhachHang")] KhachHang khachHang, string NhapLaiMatKhau)
        {
            if (NhapLaiMatKhau == null)
            {
                ViewBag.nhaplaimatkhau = "Trường này không được bỏ trống";
                return View(khachHang);
            }

            if (ModelState.IsValid)
            {
                var user = db.KhachHangs.Where(x => x.EmailKhachHang == khachHang.EmailKhachHang).FirstOrDefault();
                if (user != null)
                {
                    ViewBag.email = "Email đã tồn tại";
                    return View(khachHang);
                }
                else
                {
                    if (khachHang.MatKhauKhachHang != NhapLaiMatKhau)
                    {
                        ViewBag.repassword = "Mật khẩu không trùng khớp!";
                        return View(khachHang);
                    }
                    else
                    {
                        ViewBag.success = "CHÚC MỪNG BẠN ĐÃ ĐĂNG KÍ THÀNH CÔNG!";
                        db.KhachHangs.Add(khachHang);
                        db.SaveChanges();
                        return RedirectToAction("DangKyThanhCong", "KhachHangs", new { id = khachHang.KhachHangID });
                    }
                }
            }
            return View();
        }

        public ActionResult DangKyThanhCong(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang kh = db.KhachHangs.Find(id);
            return View(kh);
        }
        public ActionResult ThongTinDonHang(int? id, int? tt )
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            DonHang donHang = db.DonHangs.Find(id);
            if (tt != null)
            {
                ThongBao tb = db.ThongBaos.Find(tt);
                tb.TrangThai = 2;
                db.Entry(tb).State = EntityState.Modified;
                db.SaveChanges();
            }
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult KiemTraDangNhap(KhachHang khachHang)
        {
            var userDetail = db.KhachHangs.Where(x => x.EmailKhachHang == khachHang.EmailKhachHang && x.MatKhauKhachHang == khachHang.MatKhauKhachHang).FirstOrDefault();
            if (userDetail == null)
            {
                ViewBag.saidangnhap = "Nhập sai tên đăng nhập hoặc mật khẩu!";
                return View("DangNhap", khachHang);
            }
            else
            {
                KhachHang khachHangs = db.KhachHangs.Find(userDetail.KhachHangID);
                Session["userName"] = khachHangs.TenKhachHang;
                Session["email"] = khachHangs.EmailKhachHang;
                Session["KhachHangID"] = khachHangs.KhachHangID;
                Session["sdt"] = khachHangs.SDT;
                Session["diachi"] = khachHang.DiaChi;
                Session["KhachHang"] = khachHangs;

                Cart cart = Session["Cart"] as Cart;
                cart = null;
                if (cart == null || Session["Cart"] == null)
                {
                    cart = new Cart();
                    if (Session["KhachHangID"] != null)
                    {
                        int KhachHangID = (int)Session["KhachHangID"];
                        foreach (GioHang item in db.GioHangs.Where(x => x.KhachHangID == KhachHangID).ToList())
                        {
                            cart.Add(db.Albums.Find(item.AlbumID), item.SoLuong);
                        }
                    }
                    Session["Cart"] = cart;
                }

                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult DonHang(int? id)
        {
            if (Session["userName"] == null)
            {
                return RedirectToAction("DangNhap");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var donhang = db.DonHangs.Where(x => x.KhachHangID == id);
            var tt1 = donhang.Where(x => x.TrangThaiDonHangID == 1);
            var tt2 = donhang.Where(x => x.TrangThaiDonHangID == 2);
            var tt3 = donhang.Where(x => x.TrangThaiDonHangID == 3);
            var tt4 = donhang.Where(x => x.TrangThaiDonHangID == 4);
            var tt5 = donhang.Where(x => x.TrangThaiDonHangID == 5);
            var tt6 = donhang.Where(x => x.TrangThaiDonHangID == 6);
            ListTrangThaiDonHang list = new ListTrangThaiDonHang
            {
                donHang = donhang.ToList(),
                donHang1 = tt1.ToList(),
                donHang2 = tt2.ToList(),
                donHang3 = tt3.ToList(),
                donHang4 = tt4.ToList(),
                donHang5 = tt5.ToList(),
                donHang6 = tt6.ToList()
            };
            return View(list);
        }

        public ActionResult ThemVaoYeuThich(int? id)
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
            if (Session["userName"] == null)
            {
                return RedirectToAction("DangNhap");
            }
            var SanPhamYeuThich = db.SanPhamYeuThiches.Where(x => x.AlbumID == album.AlbumID).FirstOrDefault();
            SanPhamYeuThich sanphamyeuthich = new SanPhamYeuThich();
            sanphamyeuthich.AlbumID = album.AlbumID;
            sanphamyeuthich.KhachHangID = Convert.ToInt32(Session["KhachHangID"]);
            if (SanPhamYeuThich == null)
            {
                sanphamyeuthich.TrangThai = 1;
                db.SanPhamYeuThiches.Add(sanphamyeuthich);
                db.SaveChanges();
            }
            else
            {
                SanPhamYeuThich sanpham = db.SanPhamYeuThiches.Find(SanPhamYeuThich.SanPhamYeuThichID);
                if (sanpham.TrangThai == 0)
                {
                    sanpham.TrangThai = 1;
                }
                else
                {
                    sanpham.TrangThai = 0;
                }

                db.Entry(sanpham).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("ThongTinSanPham", "Home", new { id = album.AlbumID });
        }

        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ThongBao()
        {
            int id = Convert.ToInt32(Session["KhachHangID"]);
            var thongBaos = db.ThongBaos.Include(t => t.DonHang).Include(t => t.KhachHang).Where(x=>x.KhachHangID == id).OrderByDescending(x=>x.ThoiGian);
            return View(thongBaos.ToList());
        }

        public ActionResult BinhLuan(string binhluan)
        {
            if (Session["userName"] == null)
            {
                return RedirectToAction("DangNhap");
            }
            Album album = db.Albums.Find(Convert.ToInt32(Session["AlbumID"]));
            if (binhluan == null)
            {
                ViewBag.binhluan = "Nội dung bình luận không được bỏ trống";
                return RedirectToAction("ThongTinSanPham", "Home", new { id = album.AlbumID });
            }
            KhachHang kh = db.KhachHangs.Find(Convert.ToInt32(Session["KhachHangID"]));
            BinhLuan binhluan1 = new BinhLuan();
            binhluan1.KhachHangID = kh.KhachHangID;
            binhluan1.AlbumID = album.AlbumID;
            binhluan1.NoiDung = binhluan;
            binhluan1.ThoiGian = DateTime.Now;
            db.BinhLuans.Add(binhluan1);
            db.SaveChanges();
            return RedirectToAction("ThongTinSanPham", "Home", new { id = album.AlbumID });

        }
        public ActionResult LuuDiemDanhGia(int id, int diem)
        {
            ChiTietDonHang chiTietDonHang = db.ChiTietDonHangs.Find(id);
            Album album = db.Albums.Find(chiTietDonHang.AlbumID);
            DiemDanhGia diemDanhGia = new DiemDanhGia();
            diemDanhGia.Diem = diem;
            diemDanhGia.ChiTietDonHangID = id;
            Decimal diemTB = Convert.ToDecimal((album.DiemDanhGia + diem) / 2);
            album.DiemDanhGia = diemTB;
            db.Entry(album).State = EntityState.Modified;
            db.DiemDanhGias.Add(diemDanhGia);
            db.SaveChanges();
            return RedirectToAction("ThongTinDonHang", new { id = chiTietDonHang.DonHangID });

        }
        // GET: KhachHangs/Edit/5
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

        // POST: KhachHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KhachHangID,TenKhachHang,SDT,DiaChi")] KhachHang khachHang)
        {
            KhachHang kh = db.KhachHangs.Find(khachHang.KhachHangID);
            kh.TenKhachHang = khachHang.TenKhachHang;
            kh.SDT = khachHang.SDT;
            kh.DiaChi = khachHang.DiaChi;
            if (ModelState.IsValid)
            {
                db.Entry(kh).State = EntityState.Modified;
                db.SaveChanges();
            }
            ViewBag.capnhatthanhcong = "CẬP NHẬT THÀNH CÔNG";
            Session["userName"] = kh.TenKhachHang;
            Session["email"] = kh.EmailKhachHang;
            Session["KhachHangID"] = kh.KhachHangID;
            Session["sdt"] = kh.SDT;
            Session["diachi"] = kh.DiaChi;
            return View(kh);
        }

        public ActionResult ThayDoiMatKhau()
        {
            int id = Convert.ToInt32(Session["KhachHangID"]);
            if (Session["KhachHangID"] == null)
            {
                return View("DangNhap");
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
        public ActionResult ThayDoiMatKhau([Bind(Include = "KhachHangID")] KhachHang khachHang, string MatKhauHienTai, string MatKhauMoi, string NhapLaiMatKhauMoi)
        {
            KhachHang kh = db.KhachHangs.Find(khachHang.KhachHangID);
            if (MatKhauHienTai == "" || MatKhauMoi == "" || NhapLaiMatKhauMoi == "")
            {
                if (MatKhauHienTai == "")
                {
                    ViewBag.MatKhauHienTaierror = "Trường này không được bỏ trống";
                }
                if (MatKhauMoi == "")
                {
                    ViewBag.MatKhauMoierror = "Trường này không được bỏ trống";
                }
                if (NhapLaiMatKhauMoi == "")
                {
                    ViewBag.NhapLaiMatKhauMoierror = "Trường này không được bỏ trống";
                }
            }
            else
            {
                if (kh.MatKhauKhachHang == MatKhauHienTai)
                {
                    if (ModelState.IsValid)
                    {
                        if (MatKhauMoi == NhapLaiMatKhauMoi)
                        {
                            if (MatKhauHienTai == MatKhauMoi)
                            {
                                ViewBag.error = "MẬT KHẨU MỚI KHÔNG ĐƯỢC TRÙNG VỚI MẬT KHẨU CŨ";
                            }
                            else
                            {
                                ViewBag.tb = "CẬP NHẬT MẬT KHẨU THÀNH CÔNG";
                                kh.MatKhauKhachHang = MatKhauMoi;
                                db.Entry(kh).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            ViewBag.NhapLaiMatKhauMoierror = "Nhập lại mật khẩu mới không trùng khớp";
                        }
                    }
                }
                else
                {
                    ViewBag.MatKhauHienTaierror = "Mật khẩu cũ không trùng khớp";
                }
            }
            ViewBag.MatKhauHienTai = MatKhauHienTai;
            return View(kh);
        }

        // GET: KhachHangs/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: KhachHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KhachHang khachHang = db.KhachHangs.Find(id);
            db.KhachHangs.Remove(khachHang);
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

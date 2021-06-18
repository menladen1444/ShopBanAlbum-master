using ShopBanAlbum.DAL;
using ShopBanAlbum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopBanAlbum.Controllers
{
    public class NhanVienController : Controller
    {
        ShopMusicAlbumContext db = new ShopMusicAlbumContext();
        // GET: NhanVien
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult KiemTraDangNhap(NhanVien nhanvien)
        {
            var userDetail = db.NhanViens.Where(x => x.EmailNhanVien == nhanvien.EmailNhanVien && x.MatKhauNhanVien == nhanvien.MatKhauNhanVien).FirstOrDefault();
            if (userDetail == null)
            {
                ViewBag.saidangnhap = "Nhập sai tên đăng nhập hoặc mật khẩu!";
                return View("DangNhap", nhanvien);
            }
            else
            {
                NhanVien nhanVien = db.NhanViens.Find(userDetail.NhanVienID);
                Session["userName"] = nhanVien.TenNhanVien;
                Session["email"] = nhanVien.EmailNhanVien;
                Session["nhanVienID"] = nhanVien.NhanVienID;
                return RedirectToAction("Index", "Admin");
            }
        }
    }
}
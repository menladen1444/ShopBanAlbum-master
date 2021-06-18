using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopBanAlbum.DAL;
using ShopBanAlbum.Filters;

namespace ShopBanAlbum.Areas.Admin.Controllers
{
    [LoginAdmin]
    public class HomeController : Controller
    {
        private ShopMusicAlbumContext db = new ShopMusicAlbumContext();
        // GET: Admin/Home
        public ActionResult Index()
        {
            if (Session["NhanVien"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.TongKhachHang = db.KhachHangs.Count();
                ViewBag.TongDonHang = db.DonHangs.Count();
                var dhthanhcong = db.DonHangs.Where(x => x.TrangThaiDonHangID == 4);
                var totalitem = 0;
                if (dhthanhcong.Count() > 0)
                {
                    var dh = db.DonHangs.Where(x => x.TrangThaiDonHangID == 4).Sum(x => x.TongTien);

                    ViewBag.TongDoanhThu = dh;
                }
                else
                {
                    ViewBag.TongDoanhThu = 0;
                }

                foreach (var item in dhthanhcong.ToList())
                {
                    var ct = db.ChiTietDonHangs.Where(x => x.DonHangID == item.DonHangID);
                    foreach (var item2 in item.ChiTietDonHangs.ToList())
                        totalitem += item2.SoLuong;
                }
                ViewBag.TongSanPhamDaBan = totalitem;

                return View();
            }
        }
    }
}
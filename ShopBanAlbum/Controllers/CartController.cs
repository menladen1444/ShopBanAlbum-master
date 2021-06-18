using ShopBanAlbum.DAL;
using ShopBanAlbum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;


namespace ShopBanAlbum.Controllers
{
    public class CartController : Controller
    {
        ShopMusicAlbumContext db = new ShopMusicAlbumContext();

        // GET: Cart
        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
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
            return cart;
        }
        public ActionResult AddtoCart(int id, int? quantity_input)
        {

            var pro = db.Albums.SingleOrDefault(s => s.AlbumID == id);
            if (pro.SoLuong < 1)
            {
                return RedirectToAction("ThongTinSanPham", "Home", new { id = id });
            }
            if (quantity_input > pro.SoLuong)
            {
                return RedirectToAction("ThongTinSanPham", "Home", new { id = id, over = true });
            }
            if (pro != null)
            {
                if (quantity_input == null)
                {
                    quantity_input = 1;
                }
                for (int i = 0; i < quantity_input; i++)
                {
                    GetCart().Add(pro);
                    if (Session["KhachHangID"] != null)
                    {
                        int KhachHangID = (int)Session["KhachHangID"];
                        if (db.GioHangs.Where(x => x.AlbumID == id && x.KhachHangID == KhachHangID).FirstOrDefault() != null)
                        {
                            foreach (GioHang item in db.GioHangs.Where(x => x.AlbumID == id && x.KhachHangID == KhachHangID).ToList())
                            {
                                item.SoLuong += 1;
                            }
                        }
                        else
                        {
                            db.GioHangs.Add(new GioHang()
                            {
                                AlbumID = id,
                                KhachHangID = (int)Session["KhachHangID"],
                                SoLuong = 1
                            });
                        }
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("ShowToCart", "Cart");
        }
        public ActionResult ShowToCart(bool? over, int? id, int? quantity)
        {
            if (over == true)
            {
                Album album = db.Albums.Find(id);
                ViewBag.soluong = album.SoLuong;
                ViewBag.quantity = quantity;
                ViewBag.id = id;
            }
            Cart cart = Session["Cart"] as Cart;
            return View(cart);
        }
        public ActionResult ShowToCartKhachHang()
        {
            KhachHang kh = db.KhachHangs.Find(Convert.ToInt32(Session["KhachHangID"]));
            var giohang = db.GioHangs.Where(x => x.KhachHangID == kh.KhachHangID).ToList();
            int tongtien = 0;
            foreach (var item in giohang)
            {
                tongtien += item.Album.GiaBan * item.SoLuong;
            }
            ViewBag.tongtien = tongtien;
            return View(giohang.ToList());
        }
        public ActionResult Update_Quantity_Cart(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            int id_pro = int.Parse(form["AlbumID"]);
            int quantity = int.Parse(form["SoLuong"]);
            var pro = db.Albums.SingleOrDefault(s => s.AlbumID == id_pro);
            if (quantity > pro.SoLuong || quantity < 1)
            {
                return RedirectToAction("ShowToCart", "Cart", new { cart, over = true, id = id_pro, quantity = quantity });
            }
            cart.Update_Quantity_Shopping(id_pro, quantity);
            if (Session["KhachHangID"] != null)
            {
                int KhachHangID = (int)Session["KhachHangID"];
                foreach (GioHang item in db.GioHangs.Where(x => x.AlbumID == id_pro && x.KhachHangID == KhachHangID).ToList())
                {
                    item.SoLuong = quantity;
                    Session["soluong"] = item.SoLuong;
                }
                db.SaveChanges();
            }
            return RedirectToAction("ShowToCart", "Cart");
        }
        public ActionResult RemoveCart(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.Remove_CartItem(id);

            if (Session["KhachHangID"] != null)
            {
                int KhachHangID = (int)Session["KhachHangID"];
                foreach (GioHang item in db.GioHangs.Where(x => x.AlbumID == id && x.KhachHangID == KhachHangID).ToList())
                {
                    db.GioHangs.Remove(item);
                }
                db.SaveChanges();
            }
            return RedirectToAction("ShowToCart", "Cart");
        }
        public PartialViewResult BagCart()
        {
            int total_item = 0;
            double total_money = 0;
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
            {
                total_item = cart.Total_Quantity_in_Cart();
                total_money = cart.Total_Money();
                ViewBag.QuantityCart = total_item;
                ViewBag.TotalMoney = total_money;
            }
            else
            {
                ViewBag.QuantityCart = total_item;
                ViewBag.TotalMoney = total_money;
            }
            if (Session["KhachHangID"] != null)
            {
                KhachHang kh = db.KhachHangs.Find(Convert.ToInt32(Session["KhachHangID"]));
                var giohangs = db.GioHangs.Where(x => x.KhachHangID == kh.KhachHangID).ToList();
                int soluong = 0;
                foreach (var item in giohangs)
                {
                    soluong += item.SoLuong;
                }
                Session["soluong"] = soluong;
            }
            return PartialView("BagCart");
        }
        public ActionResult CheckOut(int? id, bool? fastbuy)
        {
            var pro = db.Albums.SingleOrDefault(s => s.AlbumID == id);
            if (pro != null)
            {
                if (pro.SoLuong < 1)
                {
                    return RedirectToAction("ThongTinSanPham", "Home", new { id = id });
                }
            }

            ViewBag.HinhThucThanhToanID = new SelectList(db.HinhThucThanhToans, "HinhThucThanhToanID", "TenHinhThucThanhToan");

            ViewBag.fastbuy = fastbuy;
            ViewBag.id = id;


            if (Session["KhachHangID"] != null)
            {
                KhachHang khachHang = db.KhachHangs.Find(Convert.ToInt32(Session["KhachHangID"]));
                DonHang donHang = new DonHang();
                donHang.TenKhachHang = khachHang.TenKhachHang;
                donHang.Email = khachHang.EmailKhachHang;
                donHang.SoDienThoai = khachHang.SDT;
                donHang.DiaChi = khachHang.DiaChi;
                return View(donHang);
            }
            DonHang donHang1 = new DonHang();
            return View(donHang1);
        }
        public PartialViewResult CheckoutDetail(int? id, bool? fastbuy)
        {
            if (fastbuy == true)
            {
                Album album = db.Albums.Find(id);
                Cart cart1 = new Cart();
                cart1.Add(album);
                return PartialView(cart1);
            }
            Cart cart = Session["Cart"] as Cart;
            return PartialView(cart);
        }
        public ActionResult Order(int? id)
        {
            Cart cart = Session["Cart"] as Cart;
            return RedirectToAction("CheckOut", new { cart, id = id });
        }
        [HttpPost]
        public ActionResult DoCheckout(DonHang donHang, bool? fastbuy, int? id)
        {

            if (ModelState.IsValid)
            {
                if (donHang.HinhThucThanhToanID == 2)
                {
                    TempData["fastbuy"] = fastbuy;
                    TempData["donHang"] = donHang;
                    TempData["id"] = id;
                    return RedirectToAction("PaymentWithPaypal", "Home");
                }
                Cart cart = Session["Cart"] as Cart;

                donHang.NgayDatHang = DateTime.Now;
                donHang.TrangThaiDonHangID = 1;
                if (Session["KhachHangID"] == null)
                {
                    donHang.KhachHangID = null;
                }
                else
                {
                    KhachHang kh = db.KhachHangs.Find(Convert.ToInt32(Session["KhachHangID"]));
                    donHang.KhachHangID = kh.KhachHangID;
                }

                int tongtien = 0;
                try
                {
                    if (fastbuy == true)
                    {
                        Cart cart1 = new Cart();
                        Album album = db.Albums.Find(id);
                        cart1.Add(album);
                        foreach (var item in cart1.Items)
                        {
                            ChiTietDonHang chiTietDonHang = new ChiTietDonHang();
                            chiTietDonHang.DonHangID = donHang.DonHangID;
                            chiTietDonHang.AlbumID = item._shopping_product.AlbumID;
                            chiTietDonHang.GiaBan = item._shopping_product.GiaBan;
                            chiTietDonHang.SoLuong = item._shopping_quantity;
                            db.ChiTietDonHangs.Add(chiTietDonHang);
                            tongtien += item._shopping_product.GiaBan * item._shopping_quantity;
                        }
                        donHang.TongTien = tongtien;
                        donHang.TrangThaiThanhToan = "Chưa thanh toán";
                        db.DonHangs.Add(donHang);
                        db.SaveChanges();
                        foreach (var item in cart1.Items)
                        {
                            Album a = db.Albums.Find(item._shopping_product.AlbumID);
                            a.SoLuong -= item._shopping_quantity;
                            db.Entry(a).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        if (donHang.HinhThucThanhToanID == 1)
                        {
                            return RedirectToAction("Shopping_Success", "Cart");
                        }
                    }
                }
                catch
                {
                    return RedirectToAction("CheckOut", new { id = id, fastbuy = fastbuy });
                }


                foreach (var item in cart.Items)
                {
                    ChiTietDonHang chiTietDonHang = new ChiTietDonHang();
                    chiTietDonHang.DonHangID = donHang.DonHangID;
                    chiTietDonHang.AlbumID = item._shopping_product.AlbumID;
                    chiTietDonHang.GiaBan = item._shopping_product.GiaBan;
                    chiTietDonHang.SoLuong = item._shopping_quantity;
                    db.ChiTietDonHangs.Add(chiTietDonHang);
                    tongtien += item._shopping_product.GiaBan * item._shopping_quantity;
                }
                donHang.TongTien = tongtien;
                donHang.TrangThaiThanhToan = "Chưa thanh toán";
                db.DonHangs.Add(donHang);
                db.SaveChanges();
                foreach (var item in cart.Items)
                {
                    Album a = db.Albums.Find(item._shopping_product.AlbumID);
                    a.SoLuong -= item._shopping_quantity;
                    db.Entry(a).State = EntityState.Modified;
                    db.SaveChanges();
                }
                cart.ClearCart();
                if (Session["KhachHangID"] != null)
                {
                    int KhachHangID = (int)Session["KhachHangID"];
                    if (db.GioHangs.Where(x => x.KhachHangID == KhachHangID).FirstOrDefault() != null)
                    {
                        foreach (GioHang item in db.GioHangs.Where(x => x.KhachHangID == KhachHangID).ToList())
                        {
                            db.GioHangs.Remove(item);
                        }
                    }
                    db.SaveChanges();
                }

                if (donHang.HinhThucThanhToanID == 1)
                {
                    return RedirectToAction("Shopping_Success", "Cart");

                }
            }
            ViewBag.HinhThucThanhToanID = new SelectList(db.HinhThucThanhToans, "HinhThucThanhToanID", "TenHinhThucThanhToan");

            ViewBag.fastbuy = fastbuy;
            ViewBag.id = id;
            return View("CheckOut");


            //return RedirectToAction("CheckOut", new { id = id, fastbuy = fastbuy});
        }
        public ActionResult Shopping_Success()
        {
            return View();
        }
    }
}

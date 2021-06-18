using ShopBanAlbum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using ShopBanAlbum.DAL;
using System.Net;
using PayPal.Api;
using System.Globalization;

namespace ShopBanAlbum.Controllers
{
    public class HomeController : Controller
    {
        private ShopMusicAlbumContext db = new ShopMusicAlbumContext();
        public ActionResult Index(string search)
        {
            var albums = db.Albums.Include(a => a.TacGia).Include(a => a.TheLoai).Where(x => x.SoLuong != 0);
            var AlbumNoiBat = albums.OrderByDescending(x => x.NgayPhatHanh).Where(x=>x.SoLuong!=0).Take(4);
            var AlbumBanChay = albums.OrderByDescending(x => x.DaBan).Where(x => x.SoLuong != 0).Take(3);
            var carousels = db.Carousels;

            HomeModel homemodel = new HomeModel
            {
                AllAlbum = albums.Take(8).ToList(),
                AlbumBanChay = AlbumBanChay.ToList(),
                AlbumNoiBat = AlbumNoiBat.ToList(),
                Carousels = carousels.ToList()
            };
            return View(homemodel);
        }

        public ActionResult ThongTinSanPham(int? id,bool? over)
        {
            if (over == true)
            {
                ViewBag.message = Convert.ToString("Số lượng mua vượt quá số lượng tồn");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            var SanPhamYeuThich = db.SanPhamYeuThiches.Where(x => x.AlbumID == album.AlbumID).FirstOrDefault();
            Session["AlbumID"] = album.AlbumID;
            if (SanPhamYeuThich == null)
            {
                ViewBag.sanphamyeuthich = true;            
            }
            else
            {
                if(Session["userName"] == null)
                {
                    ViewBag.sanphamyeuthich = true;
                }
                else
                {
                    SanPhamYeuThich sanpham = db.SanPhamYeuThiches.Find(SanPhamYeuThich.SanPhamYeuThichID);
                    if(sanpham.TrangThai == 0)
                    {
                        ViewBag.sanphamyeuthich = true;
                    }
                    else
                    {
                        ViewBag.sanphamyeuthich = false;
                    }                    
                }              
            }
            var albums = db.Albums.Include(a => a.TacGia).Include(a => a.TheLoai);
            var AlbumList = albums.Where(x => x.AlbumID != album.AlbumID).Take(4);
            var BinhLuanList = db.BinhLuans.Where(x => x.AlbumID == id);
            AlbumDetailAndListAlbum all = new AlbumDetailAndListAlbum
            {
                Album = album,
                ListAlbum = AlbumList.ToList(),
                BinhLuan = BinhLuanList.ToList()
            };
            return View(all);
        }

        public ActionResult Search(string search)
        {
            //trả về kết quả khi tìm kiếm được
            var tacgia = db.TacGias.Where(x => x.TenTacGia.Contains(search));
            var album = db.Albums.Include(s => s.TacGia).Where(x => x.TenAlbum.Contains(search));

            AllModel allModel = new AllModel
            {
                TacGias = tacgia.ToList(),
                Albums = album.ToList(),
            };

            //khi chưa nhập từ khóa mà nhấn search => trả về danh sách rỗng
            var tacgianull = db.TacGias.Where(x => x.TacGiaID < 0);
            var albumnull = db.Albums.Include(s => s.TacGia).Where(x => x.AlbumID < 0);

            AllModel allModelnull = new AllModel
            {
                TacGias = tacgianull.ToList(),
                Albums = albumnull.ToList(),

            };
            ViewBag.Title = search;
            if (search == "")
            {
                return View(allModelnull);
            }
            else
            {
                ViewBag.countTacgia = tacgia.Count();
                ViewBag.countAlbum = album.Count();
                ViewBag.SumCount = tacgia.Count() + album.Count();
                return View(allModel);
            }
        }

        public ActionResult SanPham(int id)
        {
            if(id == 1)
            {
                ViewBag.tt0 = "block";
                ViewBag.tt1 = "none";
                ViewBag.tt2 = "none";
                ViewBag.btn = "button-phanloai-active";
                ViewBag.btn1 = "button-phanloai";
                ViewBag.btn2 = "button-phanloai";
            }
            else if (id == 2)
            {
                ViewBag.tt0 = "none";
                ViewBag.tt1 = "block";
                ViewBag.tt2 = "none";
                ViewBag.btn = "button-phanloai";
                ViewBag.btn1 = "button-phanloai-active";
                ViewBag.btn2 = "button-phanloai";
            }
            else
            {
                ViewBag.tt0 = "none";
                ViewBag.tt1 = "none";
                ViewBag.tt2 = "block";
                ViewBag.btn = "button-phanloai";
                ViewBag.btn1 = "button-phanloai";
                ViewBag.btn2 = "button-phanloai-active";
            }
            var albums = db.Albums.Include(a => a.TacGia).Include(a => a.TheLoai);
            var AlbumNoiBat = albums.OrderByDescending(x => x.NgayPhatHanh);
            var AlbumBanChay = albums.OrderByDescending(x => x.DaBan);

            HomeModel homemodel = new HomeModel
            {
                AllAlbum = albums.ToList(),
                AlbumBanChay = AlbumBanChay.ToList(),
                AlbumNoiBat = AlbumNoiBat.ToList(),
            };
            return View(homemodel);
        }
        public ActionResult LoiBaiHat(int? id)
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
            var baihats = db.BaiHats.Include(a => a.Album);
            var BaiHatList = baihats.Where(x => x.BaiHatID != baiHat.BaiHatID && x.AlbumID == baiHat.AlbumID);
            var albums = db.Albums.Include(a => a.TacGia).Include(a => a.TheLoai);
            var AlbumList = albums.Where(x => x.AlbumID != baiHat.AlbumID).Take(6);
            BaiHatDetailAndListBaiHat all = new BaiHatDetailAndListBaiHat
            {
                baiHat = baiHat,
                ListBaiHat = BaiHatList.ToList(),
                albums = AlbumList.ToList()
            };
            return View(all);
        }

        public ActionResult LienHe()
        {
            return View();
        }
        public ActionResult OneArtist(int? id)
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
            TacGia tacgia = db.TacGias.Find(id);
            return View(tacgia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LienHe([Bind(Include = "YKienID,TenKhachHang,Email,NoiDung")] YKienKhachHang yKienKhachHang)
        {
            if (ModelState.IsValid)
            {
                yKienKhachHang.ThoiGian = DateTime.Now;
                db.YKienKhachHangs.Add(yKienKhachHang);
                db.SaveChanges();
                return RedirectToAction("LienHe");
            }

            return View(yKienKhachHang);
        }
        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Home/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }
            //on successful payment, show success page to user.  
            Cart cart = Session["Cart"] as Cart;
            DonHang donHang = (DonHang)TempData["donHang"];
            bool fastbuy = Convert.ToBoolean(TempData["fastbuy"]);
            int id = Convert.ToInt32(TempData["id"]);
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
            float tongtienpaypal = 0;
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
                        float tempPrice = item._shopping_product.GiaBan / 23000;
                        tongtienpaypal += tempPrice * item._shopping_quantity;
                    }
                    donHang.TongTien = tongtien;
                    db.DonHangs.Add(donHang); 
                    donHang.ThuTienPayPal = tongtienpaypal + 2 ;
                    donHang.TrangThaiThanhToan = "Đã thanh toán qua Paypal";
                    db.SaveChanges();
                    foreach (var item in cart1.Items)
                    {
                        Album a = db.Albums.Find(item._shopping_product.AlbumID);
                        a.SoLuong -= item._shopping_quantity;
                        db.Entry(a).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    if (donHang.HinhThucThanhToanID == 2)
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
                float tempPrice = item._shopping_product.GiaBan / 23000;
                tongtienpaypal += tempPrice * item._shopping_quantity;
            }
            donHang.TongTien = tongtien;
            donHang.ThuTienPayPal = tongtienpaypal + 2;
            donHang.TrangThaiThanhToan = "Đã thanh toán qua Paypal";
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
            //return View("SuccessView");
            return RedirectToAction("Shopping_Success", "Cart");
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            Cart cart = Session["Cart"] as Cart;
            if (Convert.ToBoolean(TempData["fastbuy"]) == true)
            {
                cart = new Cart();
                Album album = db.Albums.Find(Convert.ToInt32(TempData["id"]));
                cart.Add(album);
            }
            float _subtotal = 0;
            foreach (var item in cart.Items)
            {
                float tempPrice = item._shopping_product.GiaBan / 23000;
                //Adding Item Details like name, currency, price etc  
                itemList.items.Add(new Item()
                {
                    name = item._shopping_product.TenAlbum.ToString(),
                    currency = "USD",
                    price = tempPrice.ToString(),
                    quantity = item._shopping_quantity.ToString(),
                    sku = "SKU#" + item._shopping_product.AlbumID
                });
                _subtotal += tempPrice * item._shopping_quantity;
            }

            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "1",
                shipping = "1",
                subtotal = _subtotal.ToString(),
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = (_subtotal + 2).ToString(), // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            DateTime dt = DateTime.Now;
            String date;
            date = dt.ToString("yyyyddMMhhmmss", DateTimeFormatInfo.InvariantInfo);
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            transactionList.Add(new Transaction()
            {
                description = "Transaction description",
                invoice_number = date, //Generate an Invoice No  
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }
        public ActionResult FailureView()
        {
            return View();
        }
    }
}
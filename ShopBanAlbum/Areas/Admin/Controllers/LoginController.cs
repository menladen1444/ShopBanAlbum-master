using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopBanAlbum.Areas.Admin.Models;
using ShopBanAlbum.DAL;

namespace ShopBanAlbum.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        private ShopMusicAlbumContext db = new ShopMusicAlbumContext();
        // GET: Admin/Login
        public ActionResult Index()
        {
            if (Session["NhanVien"] == null)
            {
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult DoLogin(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var check = db.NhanViens.SingleOrDefault(x => x.EmailNhanVien == loginModel.Email && x.MatKhauNhanVien == loginModel.MatKhau);
                if (check != null)
                {
                    Session["NhanVien"] = check;
                    //Session["Role"] = check.GetRole();
                    //Session.Add("Role", check.GetRole());
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("CredentialError", "Sai Tên đăng nhập hoặc Mật khẩu! Vui lòng thử lại!");
                    return View("Index");
                }
            }
            else
            {
                return View("Index");
            }
        }
        public ActionResult Logout()
        {
            Session.Remove("NhanVien");
            Session.Remove("Role");
            return RedirectToAction("Index");
        }
        public ActionResult Warning()
        {
            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShopBanAlbum.Filters
{
    public class LoginAdmin : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var url = filterContext.HttpContext.Request.Url;
            if (HttpContext.Current.Session["NhanVien"] == null)
            {
                HttpContext.Current.Session["returnUrl"] = url;
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    {"Controller", "Login"},
                    {"Action", "Index"},
                });
            }
            if (HttpContext.Current.Session["NhanVien"] != null)
            {
                if (HttpContext.Current.Session["returnUrl"] != null)
                {
                    var link = HttpContext.Current.Session["returnUrl"].ToString();
                    HttpContext.Current.Session.Remove("returnUrl");
                    filterContext.Result = new RedirectResult(link);
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
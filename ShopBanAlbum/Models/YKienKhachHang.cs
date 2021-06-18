using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopBanAlbum.Models
{
    public class YKienKhachHang
    {
        [Key]
        public int YKienID { get; set; }
        public string TenKhachHang { get; set; }
        public string Email { get; set; }
        public string NoiDung { get; set; }
        public DateTime? ThoiGian { get; set; }
    }
}
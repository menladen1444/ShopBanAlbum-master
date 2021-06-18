using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopBanAlbum.Models
{
    public class NhanVien
    {
        [Key]
        public int NhanVienID { get; set; }
        public string TenNhanVien { get; set; }
        public string EmailNhanVien { get; set; }
        public string MatKhauNhanVien { get; set; }
    }
}

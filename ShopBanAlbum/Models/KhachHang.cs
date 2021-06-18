using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopBanAlbum.Models
{
    public class KhachHang
    {
        [Key]
        public int KhachHangID { get; set; }
        public string TenKhachHang { get; set; }
        public string SDT { get; set; }
        public decimal DiemKhachHang { get; set; }
        public string DiaChi { get; set; }
        public string EmailKhachHang { get; set; }
        public string MatKhauKhachHang { get; set; }
        public string NhapLaiMatKhau { get; set; }

        public virtual ICollection<DonHang> DonHangs { get; set; }
        public virtual ICollection<GioHang> GioHangs { get; set; }
        public virtual ICollection<SanPhamYeuThich> SanPhamYeuThiches { get; set; }
        public virtual ICollection<KhachHang> KhachHangs { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShopBanAlbum.Models
{
    public class ThongBao
    {
        [Key]
        public int ThongBaoID { get; set; }
        public int TrangThai { get; set; }

        public DateTime? ThoiGian { get; set; }

        [ForeignKey("DonHang")]     
        public int? DongHangID { get; set; }

        public virtual DonHang DonHang { get; set; }
        [ForeignKey("KhachHang")]
        public int? KhachHangID { get; set; }

        public virtual KhachHang KhachHang { get; set; }
        [ForeignKey("TrangThaiDonHang")]
        public int? TrangThaiDonHangID { get; set; }

        public virtual TrangThaiDonHang TrangThaiDonHang { get; set; }
    }
}
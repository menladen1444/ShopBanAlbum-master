using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShopBanAlbum.Models
{
    public class DiemDanhGia
    {
        [Key]
        public int DiemDanhGiaID { get; set; }
        public int Diem { get; set; }
        [ForeignKey("ChiTietDonHang")]
        public int? ChiTietDonHangID { get; set; }
        public virtual ChiTietDonHang ChiTietDonHang { get; set; }
    }
}
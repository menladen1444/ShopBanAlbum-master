using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopBanAlbum.Models
{
    public class ChiTietDonHang
    {
        [Key]
        public int ChiTietDonHangID { get; set; }
        [ForeignKey("DonHang")]
        public int? DonHangID { get; set; }
        public int SoLuong { get; set; }
        [ForeignKey("Album")]
        public int? AlbumID { get; set; }
        public int GiaBan { get; set; }
    
        public virtual Album Album { get; set; }
        public virtual DonHang DonHang { get; set; }
        public virtual ICollection<DiemDanhGia> DiemDanhGias { get; set; }
        public int TongTien()
        {
            return SoLuong * GiaBan;
        }
    }
}

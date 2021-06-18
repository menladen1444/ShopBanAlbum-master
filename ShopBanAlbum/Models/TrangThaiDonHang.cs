using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopBanAlbum.Models
{
    public class TrangThaiDonHang
    {
        [Key]
        public int TrangThaiDonHangID { get; set; }
        public string TenTrangThaiDonHang { get; set; }

        public virtual ICollection<DonHang> DonHangs { get; set; }
    }
}

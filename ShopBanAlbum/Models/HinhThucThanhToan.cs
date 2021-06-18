using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace ShopBanAlbum.Models
{
    public class HinhThucThanhToan
    {
        [Key]
        public int HinhThucThanhToanID { get; set; }
        public string TenHinhThucThanhToan { get; set; }
    
        public virtual ICollection<DonHang> DonHangs { get; set; }
    }
}

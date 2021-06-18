using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopBanAlbum.Models
{
    public class SanPhamYeuThich
    {
        [Key]
        public int SanPhamYeuThichID { get; set; }
        [ForeignKey("KhachHang")]
        public int? KhachHangID { get; set; }
        [ForeignKey("Album")]
        public int? AlbumID { get; set; }
        public int TrangThai { get; set; }
    
        public virtual Album Album { get; set; }
        public virtual KhachHang KhachHang { get; set; }
    }
}

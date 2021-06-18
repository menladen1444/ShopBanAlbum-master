using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopBanAlbum.Models
{
    public class GioHang
    {
        [Key]
        public int GioHangID { get; set; }
        [ForeignKey("Album")]
        public int? AlbumID { get; set; }
        [ForeignKey("KhachHang")]
        public int? KhachHangID { get; set; }
        public int SoLuong { get; set; }
    
        public virtual Album Album { get; set; }
        public virtual KhachHang KhachHang { get; set; }
    }
}

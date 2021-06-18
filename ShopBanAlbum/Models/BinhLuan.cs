using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShopBanAlbum.Models
{
    public class BinhLuan
    {
        [Key]
        public int BinhLuanID { get; set; }
        public string NoiDung { get; set; }
        public DateTime ThoiGian { get; set; }

        [ForeignKey("KhachHang")]
        public int? KhachHangID { get; set; }

        public virtual KhachHang KhachHang { get; set; }

        [ForeignKey("Album")]
        public int? AlbumID { get; set; }

        public virtual Album Album { get; set; }
    }
}
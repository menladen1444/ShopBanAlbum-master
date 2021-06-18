using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopBanAlbum.Models
{
    public class BaiHat
    {
        [Key]
        public int BaiHatID { get; set; }
        public string TenBaiHat { get; set; }
        public string LoiBaiHat { get; set; }
        public string ThoiLuong { get; set; }
        [ForeignKey("Album")]
        public int? AlbumID { get; set; }
    
        public virtual Album Album { get; set; }
    }
}

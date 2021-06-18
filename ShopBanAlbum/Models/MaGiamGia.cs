using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopBanAlbum.Models
{
    public class MaGiamGia
    {
        [Key]
        public int MaGiamGiaID { get; set; }
        public string TenMaGiamGia { get; set; }
        public string CodeMaGiamGia { get; set; }
        public decimal TiLeGiamGia { get; set; }
    }
}
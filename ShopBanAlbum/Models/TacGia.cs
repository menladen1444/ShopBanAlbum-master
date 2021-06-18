using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopBanAlbum.Models
{
    public class TacGia
    {
        [Key]
        public int TacGiaID { get; set; }
        public string TenTacGia { get; set; }
        public string GioiThieu { get; set; }
        public int? NamSinh { get; set; }
        public string NoiSinh { get; set; }
        public string HinhAnh { get; set; }
    
        public virtual ICollection<Album> Albums { get; set; }
    }
}

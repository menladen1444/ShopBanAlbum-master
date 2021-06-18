using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopBanAlbum.Models
{    
    public class QuocGia
    {
        [Key]
        public int QuocGiaID { get; set; }
        public string TenQuocGia { get; set; }
    
        public virtual ICollection<Album> Albums { get; set; }
    }
}

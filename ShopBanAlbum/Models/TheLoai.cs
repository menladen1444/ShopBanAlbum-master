using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopBanAlbum.Models
{

    public class TheLoai
    {
        [Key]
        public int TheLoaiID { get; set; }
        public string TenTheLoai { get; set; }
    
        public virtual ICollection<Album> Albums { get; set; }
    }
}

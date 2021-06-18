using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopBanAlbum.Models
{
    public class Carousel
    {
        [Key]
        public int CarouselID { get; set; }
        public string HinhAnh { get; set; }
        public string TenCarousel { get; set; }
        public string ClassName { get; set; }
    }
}

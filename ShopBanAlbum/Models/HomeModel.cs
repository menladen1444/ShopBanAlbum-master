using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopBanAlbum.Models
{
    public class HomeModel
    {
        public List<Album> AlbumNoiBat { get; set; }
        public List<Album> AlbumBanChay { get; set; }
        public List<Album> AllAlbum { get; set; }
        public List<Carousel> Carousels { get; set; }
    }
}
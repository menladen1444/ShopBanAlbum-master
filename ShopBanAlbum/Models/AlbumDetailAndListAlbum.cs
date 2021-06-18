using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopBanAlbum.Models
{
    public class AlbumDetailAndListAlbum
    {
        public List<Album> ListAlbum { get; set; }

        public Album Album { get; set; }
        public List<BinhLuan> BinhLuan { get; set; }
    }
}
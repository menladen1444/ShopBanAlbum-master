using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopBanAlbum.Models
{
    public class BaiHatDetailAndListBaiHat
    {
        public List<BaiHat> ListBaiHat { get; set; }
        public BaiHat baiHat { get; set; }

        public List<Album> albums { get; set; }
    }
}
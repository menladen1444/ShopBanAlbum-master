using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopBanAlbum.Models
{

    public class Album
    {
        [Key]
        public int AlbumID { get; set; }
        public string TenAlbum { get; set; }
        public string HinhAnh { get; set; }
        public DateTime? NgayPhatHanh { get; set; }
        public int GiaBan { get; set; }
        public int SoLuong { get; set; }
        [ForeignKey("TacGia")]
        public int? TacGiaID { get; set; }
        public int? DaBan { get; set; }
        public decimal? DiemDanhGia { get; set; }
        public string XuatXu { get; set; }
        public string PhuKien { get; set; }
        [ForeignKey("TheLoai")]
        public int? TheLoaiID { get; set; }
        [ForeignKey("QuocGia")]
        public int? QuocGiaID { get; set; }
    
        public virtual QuocGia QuocGia { get; set; }
        public virtual TacGia TacGia { get; set; }
        public virtual TheLoai TheLoai { get; set; }
        public virtual ICollection<BaiHat> BaiHats { get; set; }
        public virtual ICollection<GioHang> GioHangs { get; set; }
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public virtual ICollection<SanPhamYeuThich> SanPhamYeuThiches { get; set; }
        public virtual ICollection<KhachHang> KhachHangs { get; set; }
    }
}

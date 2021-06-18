using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using ShopBanAlbum.Models;

namespace ShopBanAlbum.DAL
{
    public class ShopMusicAlbumContext : DbContext
    {
        public ShopMusicAlbumContext() : base("name=ShopMusicAlbumEntities1")
        {
        }

        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<BaiHat> BaiHats { get; set; }
        public virtual DbSet<Carousel> Carousels { get; set; }
        public virtual DbSet<DonHang> DonHangs { get; set; }
        public virtual DbSet<GioHang> GioHangs { get; set; }
        public virtual DbSet<HinhThucThanhToan> HinhThucThanhToans { get; set; }
        public virtual DbSet<KhachHang> KhachHangs { get; set; }
        public virtual DbSet<NhanVien> NhanViens { get; set; }
        public virtual DbSet<QuocGia> QuocGias { get; set; }
        public virtual DbSet<SanPhamYeuThich> SanPhamYeuThiches { get; set; }
        public virtual DbSet<TacGia> TacGias { get; set; }
        public virtual DbSet<TheLoai> TheLoais { get; set; }
        public virtual DbSet<TrangThaiDonHang> TrangThaiDonHangs { get; set; }
        public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public virtual DbSet<YKienKhachHang> YKienKhachHangs { get; set; }
        public virtual DbSet<BinhLuan> BinhLuans { get; set; }
        public virtual DbSet<DiemDanhGia> DiemDanhGias { get; set; }
        public virtual DbSet<ThongBao> ThongBaos { get; set; }
    }

}
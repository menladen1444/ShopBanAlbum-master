namespace ShopBanAlbum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Albums",
                c => new
                    {
                        AlbumID = c.Int(nullable: false, identity: true),
                        TenAlbum = c.String(),
                        HinhAnh = c.String(),
                        NgayPhatHanh = c.DateTime(),
                        GiaBan = c.Int(nullable: false),
                        SoLuong = c.Int(nullable: false),
                        TacGiaID = c.Int(),
                        DaBan = c.Int(),
                        DiemDanhGia = c.Decimal(precision: 18, scale: 2),
                        XuatXu = c.String(),
                        PhuKien = c.String(),
                        TheLoaiID = c.Int(),
                        QuocGiaID = c.Int(),
                    })
                .PrimaryKey(t => t.AlbumID)
                .ForeignKey("dbo.QuocGias", t => t.QuocGiaID)
                .ForeignKey("dbo.TacGias", t => t.TacGiaID)
                .ForeignKey("dbo.TheLoais", t => t.TheLoaiID)
                .Index(t => t.TacGiaID)
                .Index(t => t.TheLoaiID)
                .Index(t => t.QuocGiaID);
            
            CreateTable(
                "dbo.BaiHats",
                c => new
                    {
                        BaiHatID = c.Int(nullable: false, identity: true),
                        TenBaiHat = c.String(),
                        LoiBaiHat = c.String(),
                        ThoiLuong = c.String(),
                        AlbumID = c.Int(),
                    })
                .PrimaryKey(t => t.BaiHatID)
                .ForeignKey("dbo.Albums", t => t.AlbumID)
                .Index(t => t.AlbumID);
            
            CreateTable(
                "dbo.ChiTietDonHangs",
                c => new
                    {
                        ChiTietDonHangID = c.Int(nullable: false, identity: true),
                        DonHangID = c.Int(),
                        SoLuong = c.Int(nullable: false),
                        AlbumID = c.Int(),
                        GiaBan = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ChiTietDonHangID)
                .ForeignKey("dbo.Albums", t => t.AlbumID)
                .ForeignKey("dbo.DonHangs", t => t.DonHangID)
                .Index(t => t.DonHangID)
                .Index(t => t.AlbumID);
            
            CreateTable(
                "dbo.DiemDanhGias",
                c => new
                    {
                        DiemDanhGiaID = c.Int(nullable: false, identity: true),
                        Diem = c.Int(nullable: false),
                        ChiTietDonHangID = c.Int(),
                    })
                .PrimaryKey(t => t.DiemDanhGiaID)
                .ForeignKey("dbo.ChiTietDonHangs", t => t.ChiTietDonHangID)
                .Index(t => t.ChiTietDonHangID);
            
            CreateTable(
                "dbo.DonHangs",
                c => new
                    {
                        DonHangID = c.Int(nullable: false, identity: true),
                        TrangThaiDonHangID = c.Int(),
                        TenKhachHang = c.String(nullable: false),
                        DiaChi = c.String(nullable: false),
                        Email = c.String(),
                        SoDienThoai = c.String(nullable: false),
                        NgayDatHang = c.DateTime(nullable: false),
                        GhiChu = c.String(),
                        HinhThucThanhToanID = c.Int(),
                        TongTien = c.Int(nullable: false),
                        ThuTienPayPal = c.Single(nullable: false),
                        KhachHangID = c.Int(),
                    })
                .PrimaryKey(t => t.DonHangID)
                .ForeignKey("dbo.HinhThucThanhToans", t => t.HinhThucThanhToanID)
                .ForeignKey("dbo.KhachHangs", t => t.KhachHangID)
                .ForeignKey("dbo.TrangThaiDonHangs", t => t.TrangThaiDonHangID)
                .Index(t => t.TrangThaiDonHangID)
                .Index(t => t.HinhThucThanhToanID)
                .Index(t => t.KhachHangID);
            
            CreateTable(
                "dbo.HinhThucThanhToans",
                c => new
                    {
                        HinhThucThanhToanID = c.Int(nullable: false, identity: true),
                        TenHinhThucThanhToan = c.String(),
                    })
                .PrimaryKey(t => t.HinhThucThanhToanID);
            
            CreateTable(
                "dbo.KhachHangs",
                c => new
                    {
                        KhachHangID = c.Int(nullable: false, identity: true),
                        TenKhachHang = c.String(),
                        SDT = c.String(),
                        DiemKhachHang = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiaChi = c.String(),
                        EmailKhachHang = c.String(),
                        MatKhauKhachHang = c.String(),
                        NhapLaiMatKhau = c.String(),
                        KhachHang_KhachHangID = c.Int(),
                        Album_AlbumID = c.Int(),
                    })
                .PrimaryKey(t => t.KhachHangID)
                .ForeignKey("dbo.KhachHangs", t => t.KhachHang_KhachHangID)
                .ForeignKey("dbo.Albums", t => t.Album_AlbumID)
                .Index(t => t.KhachHang_KhachHangID)
                .Index(t => t.Album_AlbumID);
            
            CreateTable(
                "dbo.GioHangs",
                c => new
                    {
                        GioHangID = c.Int(nullable: false, identity: true),
                        AlbumID = c.Int(),
                        KhachHangID = c.Int(),
                        SoLuong = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GioHangID)
                .ForeignKey("dbo.Albums", t => t.AlbumID)
                .ForeignKey("dbo.KhachHangs", t => t.KhachHangID)
                .Index(t => t.AlbumID)
                .Index(t => t.KhachHangID);
            
            CreateTable(
                "dbo.SanPhamYeuThiches",
                c => new
                    {
                        SanPhamYeuThichID = c.Int(nullable: false, identity: true),
                        KhachHangID = c.Int(),
                        AlbumID = c.Int(),
                        TrangThai = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SanPhamYeuThichID)
                .ForeignKey("dbo.Albums", t => t.AlbumID)
                .ForeignKey("dbo.KhachHangs", t => t.KhachHangID)
                .Index(t => t.KhachHangID)
                .Index(t => t.AlbumID);
            
            CreateTable(
                "dbo.ThongBaos",
                c => new
                    {
                        ThongBaoID = c.Int(nullable: false, identity: true),
                        TrangThai = c.Int(nullable: false),
                        ThoiGian = c.DateTime(),
                        DongHangID = c.Int(),
                        KhachHangID = c.Int(),
                        TrangThaiDonHangID = c.Int(),
                    })
                .PrimaryKey(t => t.ThongBaoID)
                .ForeignKey("dbo.DonHangs", t => t.DongHangID)
                .ForeignKey("dbo.KhachHangs", t => t.KhachHangID)
                .ForeignKey("dbo.TrangThaiDonHangs", t => t.TrangThaiDonHangID)
                .Index(t => t.DongHangID)
                .Index(t => t.KhachHangID)
                .Index(t => t.TrangThaiDonHangID);
            
            CreateTable(
                "dbo.TrangThaiDonHangs",
                c => new
                    {
                        TrangThaiDonHangID = c.Int(nullable: false, identity: true),
                        TenTrangThaiDonHang = c.String(),
                    })
                .PrimaryKey(t => t.TrangThaiDonHangID);
            
            CreateTable(
                "dbo.QuocGias",
                c => new
                    {
                        QuocGiaID = c.Int(nullable: false, identity: true),
                        TenQuocGia = c.String(),
                    })
                .PrimaryKey(t => t.QuocGiaID);
            
            CreateTable(
                "dbo.TacGias",
                c => new
                    {
                        TacGiaID = c.Int(nullable: false, identity: true),
                        TenTacGia = c.String(),
                        GioiThieu = c.String(),
                        NamSinh = c.Int(),
                        NoiSinh = c.String(),
                        HinhAnh = c.String(),
                    })
                .PrimaryKey(t => t.TacGiaID);
            
            CreateTable(
                "dbo.TheLoais",
                c => new
                    {
                        TheLoaiID = c.Int(nullable: false, identity: true),
                        TenTheLoai = c.String(),
                    })
                .PrimaryKey(t => t.TheLoaiID);
            
            CreateTable(
                "dbo.BinhLuans",
                c => new
                    {
                        BinhLuanID = c.Int(nullable: false, identity: true),
                        NoiDung = c.String(),
                        ThoiGian = c.DateTime(nullable: false),
                        KhachHangID = c.Int(),
                        AlbumID = c.Int(),
                    })
                .PrimaryKey(t => t.BinhLuanID)
                .ForeignKey("dbo.Albums", t => t.AlbumID)
                .ForeignKey("dbo.KhachHangs", t => t.KhachHangID)
                .Index(t => t.KhachHangID)
                .Index(t => t.AlbumID);
            
            CreateTable(
                "dbo.Carousels",
                c => new
                    {
                        CarouselID = c.Int(nullable: false, identity: true),
                        HinhAnh = c.String(),
                        TenCarousel = c.String(),
                        ClassName = c.String(),
                    })
                .PrimaryKey(t => t.CarouselID);
            
            CreateTable(
                "dbo.NhanViens",
                c => new
                    {
                        NhanVienID = c.Int(nullable: false, identity: true),
                        TenNhanVien = c.String(),
                        EmailNhanVien = c.String(),
                        MatKhauNhanVien = c.String(),
                    })
                .PrimaryKey(t => t.NhanVienID);
            
            CreateTable(
                "dbo.YKienKhachHangs",
                c => new
                    {
                        YKienID = c.Int(nullable: false, identity: true),
                        TenKhachHang = c.String(),
                        Email = c.String(),
                        NoiDung = c.String(),
                        ThoiGian = c.DateTime(),
                    })
                .PrimaryKey(t => t.YKienID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BinhLuans", "KhachHangID", "dbo.KhachHangs");
            DropForeignKey("dbo.BinhLuans", "AlbumID", "dbo.Albums");
            DropForeignKey("dbo.Albums", "TheLoaiID", "dbo.TheLoais");
            DropForeignKey("dbo.Albums", "TacGiaID", "dbo.TacGias");
            DropForeignKey("dbo.Albums", "QuocGiaID", "dbo.QuocGias");
            DropForeignKey("dbo.KhachHangs", "Album_AlbumID", "dbo.Albums");
            DropForeignKey("dbo.ChiTietDonHangs", "DonHangID", "dbo.DonHangs");
            DropForeignKey("dbo.DonHangs", "TrangThaiDonHangID", "dbo.TrangThaiDonHangs");
            DropForeignKey("dbo.ThongBaos", "TrangThaiDonHangID", "dbo.TrangThaiDonHangs");
            DropForeignKey("dbo.ThongBaos", "KhachHangID", "dbo.KhachHangs");
            DropForeignKey("dbo.ThongBaos", "DongHangID", "dbo.DonHangs");
            DropForeignKey("dbo.DonHangs", "KhachHangID", "dbo.KhachHangs");
            DropForeignKey("dbo.SanPhamYeuThiches", "KhachHangID", "dbo.KhachHangs");
            DropForeignKey("dbo.SanPhamYeuThiches", "AlbumID", "dbo.Albums");
            DropForeignKey("dbo.KhachHangs", "KhachHang_KhachHangID", "dbo.KhachHangs");
            DropForeignKey("dbo.GioHangs", "KhachHangID", "dbo.KhachHangs");
            DropForeignKey("dbo.GioHangs", "AlbumID", "dbo.Albums");
            DropForeignKey("dbo.DonHangs", "HinhThucThanhToanID", "dbo.HinhThucThanhToans");
            DropForeignKey("dbo.DiemDanhGias", "ChiTietDonHangID", "dbo.ChiTietDonHangs");
            DropForeignKey("dbo.ChiTietDonHangs", "AlbumID", "dbo.Albums");
            DropForeignKey("dbo.BaiHats", "AlbumID", "dbo.Albums");
            DropIndex("dbo.BinhLuans", new[] { "AlbumID" });
            DropIndex("dbo.BinhLuans", new[] { "KhachHangID" });
            DropIndex("dbo.ThongBaos", new[] { "TrangThaiDonHangID" });
            DropIndex("dbo.ThongBaos", new[] { "KhachHangID" });
            DropIndex("dbo.ThongBaos", new[] { "DongHangID" });
            DropIndex("dbo.SanPhamYeuThiches", new[] { "AlbumID" });
            DropIndex("dbo.SanPhamYeuThiches", new[] { "KhachHangID" });
            DropIndex("dbo.GioHangs", new[] { "KhachHangID" });
            DropIndex("dbo.GioHangs", new[] { "AlbumID" });
            DropIndex("dbo.KhachHangs", new[] { "Album_AlbumID" });
            DropIndex("dbo.KhachHangs", new[] { "KhachHang_KhachHangID" });
            DropIndex("dbo.DonHangs", new[] { "KhachHangID" });
            DropIndex("dbo.DonHangs", new[] { "HinhThucThanhToanID" });
            DropIndex("dbo.DonHangs", new[] { "TrangThaiDonHangID" });
            DropIndex("dbo.DiemDanhGias", new[] { "ChiTietDonHangID" });
            DropIndex("dbo.ChiTietDonHangs", new[] { "AlbumID" });
            DropIndex("dbo.ChiTietDonHangs", new[] { "DonHangID" });
            DropIndex("dbo.BaiHats", new[] { "AlbumID" });
            DropIndex("dbo.Albums", new[] { "QuocGiaID" });
            DropIndex("dbo.Albums", new[] { "TheLoaiID" });
            DropIndex("dbo.Albums", new[] { "TacGiaID" });
            DropTable("dbo.YKienKhachHangs");
            DropTable("dbo.NhanViens");
            DropTable("dbo.Carousels");
            DropTable("dbo.BinhLuans");
            DropTable("dbo.TheLoais");
            DropTable("dbo.TacGias");
            DropTable("dbo.QuocGias");
            DropTable("dbo.TrangThaiDonHangs");
            DropTable("dbo.ThongBaos");
            DropTable("dbo.SanPhamYeuThiches");
            DropTable("dbo.GioHangs");
            DropTable("dbo.KhachHangs");
            DropTable("dbo.HinhThucThanhToans");
            DropTable("dbo.DonHangs");
            DropTable("dbo.DiemDanhGias");
            DropTable("dbo.ChiTietDonHangs");
            DropTable("dbo.BaiHats");
            DropTable("dbo.Albums");
        }
    }
}

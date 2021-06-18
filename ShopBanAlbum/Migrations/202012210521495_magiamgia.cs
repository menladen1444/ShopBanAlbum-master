namespace ShopBanAlbum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class magiamgia : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MaGiamGias",
                c => new
                    {
                        MaGiamGiaID = c.Int(nullable: false, identity: true),
                        TenMaGiamGia = c.String(),
                        CodeMaGiamGia = c.String(),
                        TiLeGiamGia = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.MaGiamGiaID);
            
            AddColumn("dbo.DonHangs", "TienShip", c => c.Int(nullable: false));
            AddColumn("dbo.DonHangs", "TienKhuyenMai", c => c.Int(nullable: false));
            AddColumn("dbo.DonHangs", "MaGiamGiaID", c => c.Int());
            CreateIndex("dbo.DonHangs", "MaGiamGiaID");
            AddForeignKey("dbo.DonHangs", "MaGiamGiaID", "dbo.MaGiamGias", "MaGiamGiaID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DonHangs", "MaGiamGiaID", "dbo.MaGiamGias");
            DropIndex("dbo.DonHangs", new[] { "MaGiamGiaID" });
            DropColumn("dbo.DonHangs", "MaGiamGiaID");
            DropColumn("dbo.DonHangs", "TienKhuyenMai");
            DropColumn("dbo.DonHangs", "TienShip");
            DropTable("dbo.MaGiamGias");
        }
    }
}

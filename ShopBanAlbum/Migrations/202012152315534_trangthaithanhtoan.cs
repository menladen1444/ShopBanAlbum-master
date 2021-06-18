namespace ShopBanAlbum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class trangthaithanhtoan : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DonHangs", "TrangThaiThanhToan", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DonHangs", "TrangThaiThanhToan");
        }
    }
}

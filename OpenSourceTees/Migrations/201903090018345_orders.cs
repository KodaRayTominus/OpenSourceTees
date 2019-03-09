namespace OpenSourceTees.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orders : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PurchaseOrders",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TotalPrice = c.Double(nullable: false),
                        BuyerId = c.String(),
                        ImageId = c.String(maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Images", t => t.ImageId)
                .Index(t => t.ImageId)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseOrders", "ImageId", "dbo.Images");
            DropForeignKey("dbo.PurchaseOrders", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.PurchaseOrders", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "ImageId" });
            DropTable("dbo.PurchaseOrders");
        }
    }
}

namespace OpenSourceTees.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BlobStorage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ImageUrl = c.String(),
                        UserId = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Images", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Images", new[] { "ApplicationUser_Id" });
            DropTable("dbo.Images");
        }
    }
}

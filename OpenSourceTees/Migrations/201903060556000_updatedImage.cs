namespace OpenSourceTees.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedImage : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Images", "ImageUrl", c => c.String(nullable: false));
            AlterColumn("dbo.Images", "DesignName", c => c.String(nullable: false));
            AlterColumn("dbo.Images", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Images", "Description", c => c.String());
            AlterColumn("dbo.Images", "DesignName", c => c.String());
            AlterColumn("dbo.Images", "ImageUrl", c => c.String());
        }
    }
}

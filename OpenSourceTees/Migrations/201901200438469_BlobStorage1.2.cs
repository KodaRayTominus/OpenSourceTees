namespace OpenSourceTees.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BlobStorage12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Images", "DesignName", c => c.String());
            AddColumn("dbo.Images", "DesignerName", c => c.String());
            AddColumn("dbo.Images", "DesignerId", c => c.String());
            AddColumn("dbo.Images", "Description", c => c.String());
            AddColumn("dbo.Images", "Price", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Images", "Price");
            DropColumn("dbo.Images", "Description");
            DropColumn("dbo.Images", "DesignerId");
            DropColumn("dbo.Images", "DesignerName");
            DropColumn("dbo.Images", "DesignName");
        }
    }
}

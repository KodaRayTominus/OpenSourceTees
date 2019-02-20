namespace OpenSourceTees.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BlobStorage13 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Images", "DesignerName");
            DropColumn("dbo.Images", "DesignerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Images", "DesignerId", c => c.String());
            AddColumn("dbo.Images", "DesignerName", c => c.String());
        }
    }
}

namespace OpenSourceTees.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedPurchaseOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseOrders", "ItemPrice", c => c.Double(nullable: false));
            AddColumn("dbo.PurchaseOrders", "Quantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PurchaseOrders", "Quantity");
            DropColumn("dbo.PurchaseOrders", "ItemPrice");
        }
    }
}

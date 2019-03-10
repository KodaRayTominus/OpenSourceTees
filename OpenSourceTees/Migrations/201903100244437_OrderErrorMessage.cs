namespace OpenSourceTees.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderErrorMessage : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PurchaseOrders", "BuyerId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PurchaseOrders", "BuyerId", c => c.String());
        }
    }
}

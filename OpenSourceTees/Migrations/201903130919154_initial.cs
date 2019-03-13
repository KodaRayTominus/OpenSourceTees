namespace OpenSourceTees.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ImageUrl = c.String(nullable: false),
                        UserId = c.String(maxLength: 128),
                        DesignName = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Price = c.Double(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserRole = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.KeyRanks",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Ranking = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderProcessings",
                c => new
                    {
                        OrderId = c.String(nullable: false, maxLength: 128),
                        CreateDate = c.String(nullable: false, defaultValueSql: "GETUTCDATE()"),
                        IsEmailSent = c.Boolean(nullable: false),
                        IsAccepted = c.Boolean(nullable: false),
                        IsProcessed = c.Boolean(nullable: false),
                        ProcessorId = c.String(maxLength: 128),
                        IsShipped = c.Boolean(nullable: false),
                        IsDelivered = c.Boolean(nullable: false),
                        IsCanceled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.AspNetUsers", t => t.ProcessorId)
                .ForeignKey("dbo.PurchaseOrders", t => t.OrderId)
                .Index(t => t.OrderId)
                .Index(t => t.ProcessorId);
            
            CreateTable(
                "dbo.PurchaseOrders",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TotalPrice = c.Double(nullable: false),
                        ItemPrice = c.Double(nullable: false),
                        Quantity = c.Int(nullable: false),
                        BuyerId = c.String(nullable: false, maxLength: 128),
                        ImageId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.BuyerId, cascadeDelete: true)
                .ForeignKey("dbo.Images", t => t.ImageId)
                .Index(t => t.BuyerId)
                .Index(t => t.ImageId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");

            Sql(@"CREATE UNIQUE INDEX PK_Images_Id ON Images(Id) 
                GO
                CREATE FULLTEXT CATALOG tags AS DEFAULT
                GO
                CREATE FULLTEXT INDEX ON Images(
                Id,
                DesignName,
                Description
                )
                KEY INDEX PK_Images_Id

                ON tags;", true);

            Sql(@" create function udf_imageSearch
                    (@keywords nvarchar(4000),
                        @SkipN int,
                        @TakeN int)
                    returns @srch_rslt table (Id bigint not null, Ranking int not null )
                    as
                    begin

                        declare @TakeLast int
                        set @TakeLast = @SkipN + @TakeN
                        set @SkipN = @SkipN + 1

                        insert into @srch_rslt
                        select Images.Id, Ranking
                        from 
                        (
                            select t.[KEY] as Id, t.[RANK] as Ranking, ROW_NUMBER() over (order by t.[Rank] desc) row_num
                            from containstable(Images,(Description, DesignName),@keywords)
                            as t        
                        ) as r
                        join Images on r.Id = Images.Id
                        where r.row_num between @SkipN and @TakeLast
                        order by r.Ranking desc

                        return
                    end "
);

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.OrderProcessings", "OrderId", "dbo.PurchaseOrders");
            DropForeignKey("dbo.PurchaseOrders", "ImageId", "dbo.Images");
            DropForeignKey("dbo.PurchaseOrders", "BuyerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.OrderProcessings", "ProcessorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Images", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.PurchaseOrders", new[] { "ImageId" });
            DropIndex("dbo.PurchaseOrders", new[] { "BuyerId" });
            DropIndex("dbo.OrderProcessings", new[] { "ProcessorId" });
            DropIndex("dbo.OrderProcessings", new[] { "OrderId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Images", new[] { "UserId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.PurchaseOrders");
            DropTable("dbo.OrderProcessings");
            DropTable("dbo.KeyRanks");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Images");
        }
    }
}

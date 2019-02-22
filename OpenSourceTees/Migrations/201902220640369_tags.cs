namespace OpenSourceTees.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tags : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE UNIQUE INDEX PK_Images_Id ON Images(Id) 
                GO
                CREATE FULLTEXT CATALOG tags AS DEFAULT
                GO
                CREATE FULLTEXT INDEX ON Images(
                    DesignName,
                    Description
                )
                KEY INDEX PK_Images_Id

                    ON tags; ");


            Sql(@" create function udf_imageSearch
                    (@keywords nvarchar(4000))
                        returns table
                    as
                    return (select *
                            from containstable(Images,(Description, DesignName),@keywords))"
                );
        }
        
        public override void Down()
        {
        }
    }
}

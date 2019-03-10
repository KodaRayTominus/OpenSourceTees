namespace OpenSourceTees.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tagsearch : DbMigration
    {
        public override void Up()
        {
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
        }
    }
}

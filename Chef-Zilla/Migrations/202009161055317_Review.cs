namespace Chef_Zilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Review : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        ReviewId = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        UserId = c.String(),
                        Ratings = c.Int(nullable: false),
                        ReviewTexr = c.String(),
                        ProductID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReviewId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Reviews");
        }
    }
}

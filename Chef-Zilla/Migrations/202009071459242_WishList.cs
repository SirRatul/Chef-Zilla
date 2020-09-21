namespace Chef_Zilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WishList : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WishLists",
                c => new
                    {
                        WishListID = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.WishListID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WishLists");
        }
    }
}

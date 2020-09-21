namespace Chef_Zilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderProductExtraItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderProductExtraItems",
                c => new
                    {
                        OrderProductExtraItemID = c.Int(nullable: false, identity: true),
                        CartID = c.Int(nullable: false),
                        ExtraItemID = c.Int(nullable: false),
                        ExtraItemQuantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderProductExtraItemID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OrderProductExtraItems");
        }
    }
}

namespace Chef_Zilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CartProductExtraItems",
                c => new
                    {
                        CartProductExtraItemID = c.Int(nullable: false, identity: true),
                        CartID = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                        ExtraItemQuantity = c.Int(nullable: false),
                        TotalPriceExtra = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CartProductExtraItemID);
            
            CreateTable(
                "dbo.CartProducts",
                c => new
                    {
                        CartProductID = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(nullable: false),
                        CartID = c.Int(nullable: false),
                        ProductQuantity = c.Int(nullable: false),
                        TotalPrice = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CartProductID);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        CartID = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ProductID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CartID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Carts");
            DropTable("dbo.CartProducts");
            DropTable("dbo.CartProductExtraItems");
        }
    }
}
